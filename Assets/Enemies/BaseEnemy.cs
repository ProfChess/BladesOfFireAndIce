using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected EnemyAISettings AISettings;
    [SerializeField] protected float GlobalAttackCooldownTime;
    private bool isEnemyOnCooldown = false;

    //Animation
    public Animator anim;
    public SpriteRenderer EnemySprite;

    //States
    protected enum EnemyState {Idle, Chase, Attack, ReturnHome}
    protected EnemyState CurrentEnemyState;
    [SerializeField] protected EnemyLeashType LeashType;

    //Spawning 
    [Header("Pool Type Selection")]
    [SerializeField] protected EnemyType EnemyPoolType;

    //Behaviour Components
    [SerializeField] protected BaseEnemyMovement EnemyIdleMovement;
    [SerializeField] protected BaseEnemyMovement EnemyChaseMovement;
    [SerializeField] protected List<BaseEnemyAttack> EnemyAttacks;
    protected Transform playerLocation;
    protected Vector2 playerDistance; 
    protected LayerMask PlayerDetectionMask;
    
    //Point Leash
    [SerializeField] protected Vector2 SpawnLocation;
    private bool returningHome = false;
    private bool leashTriggered = false;
    private float leashTimer = 0f;
    [SerializeField] protected bool isMovementLocked = false;

    //Room Leash
    protected SingleDungeonRoom SpawnRoom; 

    //Pathfinding
    protected NavMeshAgent agent;
    protected bool attackLOS = false;
    protected bool chaseLOS = false;

    //Timers
    protected float stateSwitchTimer = 0f;
    protected const float stateSwitchInterval = 0.2f;
    protected float nextCheck = 0f;
    protected bool canAttack = true;


    //Access to Pool
    BasePoolManager<EnemyType> PM => EnemyPoolManager.Instance;

    //Init
    public void CreateEnemy(Vector2 Position, EnemyLeashType enemyLeashType, SingleDungeonRoom room = null)
    {
        //Move to Correct Position
        gameObject.transform.position = Position;
        isMovementLocked = false;
        returningHome = false;
        leashTriggered = false;
        leashTimer = 0f;

        //Restart State
        CurrentEnemyState = EnemyState.Idle;
        LeashType = enemyLeashType;
        gameObject.SetActive(true);
        stateSwitchTimer = UnityEngine.Random.Range(0, stateSwitchInterval);

        //Spawn Location 
        SpawnLocation = Position;
        if(room != null) { SpawnRoom = room; }
    }

    public virtual void DeactivateEnemy()
    {
        gameObject.SetActive(false);
        PM.ReturnObjectToPool(EnemyPoolType, gameObject);
    }
    protected virtual void Start()
    {
        //Initialize State
        CurrentEnemyState = EnemyState.Idle;

        //Layer
        PlayerDetectionMask = LayerMask.GetMask("Player", "Walls");

        playerLocation = GameManager.Instance.getPlayer().transform;
        CreateAgent(); //NOTE --> WILL LIKELY GET CHANGED TO JUST ASSIGNING REFERENCE TO NAVAGENT
        stateSwitchTimer = UnityEngine.Random.Range(0, stateSwitchInterval);
    }

    //Switches State Based Upon Player Distance to Enemy
    virtual protected void Update()
    {
        if(GetPlayerDistance().sqrMagnitude > 20 * 20) { DisableEnemyVisuals();}
        else if (GetPlayerDistance().sqrMagnitude <= 50 * 50) { EnableEnemyVisuals(); }

        if (isMovementLocked || !isActive) { return; }

        //Think 
        UpdateState();

        //Call functions for each state
        switch (CurrentEnemyState)
        {
            case EnemyState.Chase:
                EnemyChaseState();
                break;

            case EnemyState.Attack:
                EnemyAttackState();
                break;

            case EnemyState.ReturnHome:
                EnemyReturnHomeState();
                break;

            default:
                EnemyIdleState();
                break;
        }
    }
    public bool isActive = false;
    protected void DisableEnemyVisuals()
    {
        agent.enabled = false;
        anim.enabled = false;
        isActive = false;
    }
    protected void EnableEnemyVisuals()
    {
        agent.enabled = true;
        anim.enabled = true;
        isActive = true;
    }
    public void UpdateState()
    {
        if (stateSwitchTimer > 0f) { stateSwitchTimer -= GameTimeManager.GameDeltaTime; return; }
        stateSwitchTimer = stateSwitchInterval;

        AISightUpdate();
        HandleLeash();

        if (attackLOS)
        {
            returningHome = false;
            SetEnemyState(EnemyState.Attack); return;
        }
        if (returningHome)
        {
            SetEnemyState(EnemyState.ReturnHome); return;
        }
        if (chaseLOS)
        {
            SetEnemyState(EnemyState.Chase); return;
        }

        SetEnemyState(EnemyState.Idle);
    }
    protected void SetEnemyState(EnemyState newState)
    {
        if (CurrentEnemyState == newState) { return; }
        CurrentEnemyState = newState;
        OnStateEnter(newState);
    }
    protected void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                OnStateEnterIdle();
                break;
            case EnemyState.Chase:      
                OnStateEnterChase();
                break;
            case EnemyState.Attack:
                OnStateEnterAttack();
                break;
            case EnemyState.ReturnHome:
                OnStateEnterReturnHome();
                break;
        }
    }
    protected virtual void OnStateEnterIdle() { agent.ResetPath(); }
    protected virtual void OnStateEnterChase() { }
    protected virtual void OnStateEnterAttack() { agent.ResetPath(); }
    protected virtual void OnStateEnterReturnHome() { agent.ResetPath(); 
        agent.speed = AISettings.returnSpeed; agent.SetDestination(GetReturnPosition()); }

    //State Functions (Override in Inherited Class)
    protected virtual void EnemyIdleState()
    {
        EnemyIdleMovement.EnemyMove(agent, EnemyIdleMovement.GetMoveSpeed);
    }
    protected virtual void EnemyChaseState() 
    { 
        EnemyChaseMovement.EnemyMove(agent, EnemyChaseMovement.GetMoveSpeed);
    }
    //Returns a bool for child functions to use to know if an attack was chosen or not
    protected virtual bool EnemyAttackState() 
    {
        if (isEnemyOnCooldown) { return false; }

        BaseEnemyAttack ChosenAttack = ChooseAttack();
        if (ChosenAttack == null || !ChosenAttack.canAttack || ChosenAttack.attackInProgress) { return false; }

        ChosenAttack.Attack();

        StartCoroutine(BeginEnemyActionCooldown());
        return true;
    }
    protected virtual void EnemyReturnHomeState() 
    {
        if(isMovementLocked) { return; }

        //Attack if Player is Close, Otherwise Return to SpawnPoint
        if (attackLOS)
        {
            returningHome = false;
            agent.ResetPath();
            SetEnemyState(EnemyState.Attack);
            return;
        }

        Vector2 returnPos = GetReturnPosition();
        if (Vector2.Distance(transform.position, returnPos) <= 0.5f)
        {
            returningHome = false;
            leashTriggered = false;
            leashTimer = 0f;
            SetEnemyState(EnemyState.Idle);
        }
    }
    protected virtual Vector2 GetReturnPosition()
    {
        switch (LeashType)
        {
            case EnemyLeashType.Point:
                return SpawnLocation;
            case EnemyLeashType.Room:
                return new Vector2(Mathf.Clamp(transform.position.x, SpawnRoom.Area.xMin, SpawnRoom.Area.xMax),
                            Mathf.Clamp(transform.position.y, SpawnRoom.Area.yMin, SpawnRoom.Area.yMax));

            default: return SpawnLocation;
        }
    }
    private void HandleLeash()
    {
        bool outSideLeash = IsOutsideLeash();

        if(!outSideLeash && !returningHome)
        {
            leashTriggered = false;
            leashTimer = 0f;
            return;
        }
        if (outSideLeash && !leashTriggered)
        {
            leashTriggered = true; ; leashTimer = AISettings.ChasePastLeashTime;
        }
        if (!leashTriggered) { return; }
        if (!attackLOS) { BeginReturnHome(); return; }
        leashTimer -= GameTimeManager.GameDeltaTime;
        if (leashTimer <= 0f) { BeginReturnHome(); }
    }
    protected virtual bool IsOutsideLeash()
    {
        switch (LeashType)
        {
            case EnemyLeashType.Point:
                return Vector2.Distance(transform.position, SpawnLocation) > AISettings.LeashRange;
            case EnemyLeashType.Room:
                RectInt leashRect = new RectInt(SpawnRoom.Area.xMin - AISettings.RoomLeashPadding, SpawnRoom.Area.yMin - AISettings.RoomLeashPadding,
                                 SpawnRoom.Area.width + AISettings.RoomLeashPadding * 2, SpawnRoom.Area.height + AISettings.RoomLeashPadding * 2);

                return !leashRect.Contains(Vector2Int.RoundToInt(transform.position));
            default: return false;
        }
    }
    private void BeginReturnHome()
    {
        returningHome = true; SetEnemyState(EnemyState.ReturnHome);
    }
    public void LockEnemyMovement(bool isLocked) 
    { 
        isMovementLocked = isLocked;
        agent.isStopped = isMovementLocked;
        if (isMovementLocked ) { agent.ResetPath(); }
    }

    protected virtual IEnumerator BeginEnemyActionCooldown()
    {
        isEnemyOnCooldown = true;
        yield return GameTimeManager.WaitFor(GlobalAttackCooldownTime);
        isEnemyOnCooldown = false;
    }
    //Checks
    protected void AISightUpdate()
    {
        playerDistance = GetPlayerDistance(); float sqrDistance = playerDistance.sqrMagnitude;

        if (sqrDistance <= AISettings.ChaseRange * AISettings.ChaseRange)
        {
            chaseLOS = HasLineOfSight(AISettings.ChaseRange);
        }
        else { chaseLOS = false; }
        if (sqrDistance <= AISettings.AttackRange * AISettings.AttackRange)
        {
            attackLOS = HasLineOfSight(AISettings.AttackRange);
        }
        else { attackLOS = false; }
    }
    protected bool HasLineOfSight(float range)
    {
        RaycastHit2D sight = Physics2D.Raycast(gameObject.transform.position,
        GetPlayerDirection(),
        range,
        PlayerDetectionMask);

        if (sight.collider == null) { return false; }
        return sight.collider.GetComponentInParent<PlayerController>() != null;

    }
    protected Vector2 GetPlayerDirection() //Returns player direction from objects position
    {
        return GetPlayerDistance().normalized;
    }
    protected Vector2 GetPlayerDistance() { return (playerLocation.position - gameObject.transform.position); }
    //NavAgent
    protected void CreateAgent() //Setup for adding agent at runtime
    {
        //Pathfinding Settings
        if (gameObject.GetComponent<NavMeshAgent>() == null) 
        {  
            agent = gameObject.AddComponent<NavMeshAgent>(); 
        }
        agent = GetComponent<NavMeshAgent>();
        agent.agentTypeID = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = EnemyIdleMovement.GetMoveSpeed;
        agent.acceleration = 500;
        agent.stoppingDistance = 0.5f;
        agent.autoBraking = false;
        agent.radius = 0.2f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
    protected bool IsMoving()
    {
        return agent.velocity.sqrMagnitude > 0.01f;
    }

    //Enemy Attack Selection 
    protected BaseEnemyAttack ChooseAttack()
    {
        float totalChance = 0;
        foreach(var AttackEntry in EnemyAttacks)
        {
            if (AttackEntry.canAttack)
            { totalChance += AttackEntry.AttackChance; }
        }
        if (totalChance == 0f) { return null; }

        float Choice = UnityEngine.Random.Range(0f, totalChance);
        float choiceTrack = 0f;
        foreach (var AttackEntry in EnemyAttacks)
        {
            if (AttackEntry.canAttack)
            {
                choiceTrack += AttackEntry.AttackChance;
                if (Choice < choiceTrack)
                {
                    return AttackEntry;
                }
            }
        }
        return null;
    }
}
public enum EnemyLeashType { Point, Room, }