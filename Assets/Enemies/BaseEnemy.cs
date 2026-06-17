using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Chase Settings")]
    [Tooltip("Distance From Player to Trigger Chase State")]
    [SerializeField] protected float ChaseRange;

    [Header("Basic Attack Settings")]
    [Tooltip("Distance From Player to Trigger Attack State")]
    [SerializeField] protected float AttackRange;
    [SerializeField] protected float GlobalAttackCooldownTime;
    private bool isEnemyOnCooldown = false;

    //Animation
    public Animator anim;
    public SpriteRenderer EnemySprite;

    //States
    protected enum EnemyState {Idle, Chase, Attack}
    protected EnemyState CurrentEnemyState;

    //Spawning 
    [Header("Pool Type Selection")]
    [SerializeField] protected EnemyType EnemyPoolType;

    //Behaviour Components
    [SerializeField] protected BaseEnemyMovement EnemyIdleMovement;
    [SerializeField] protected BaseEnemyMovement EnemyChaseMovement;
    [SerializeField] protected List<BaseEnemyAttack> EnemyAttacks;
    protected Transform playerLocation;
    protected LayerMask PlayerDetectionMask;

    //Pathfinding
    protected NavMeshAgent agent;

    //Timers
    protected const float stateSwitchInterval = 0.1f;
    private float stateSwitchTimer = 0f;
    protected float nextCheck = 0f;
    protected bool canAttack = true;
    [HideInInspector] public bool canMove = true;

    //Access to Pool
    BasePoolManager<EnemyType> PM => EnemyPoolManager.Instance;

    //Init
    public void CreateEnemy(Vector2 Position)
    {
        //Move to Correct Position
        gameObject.transform.position = Position;
        canMove = true;

        //Restart State
        CurrentEnemyState = EnemyState.Idle;
        gameObject.SetActive(true);
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
    }

    //Switches State Based Upon Player Distance to Enemy
    virtual protected void Update()
    {
        if (!canMove) { return; }

        //States
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

            default:
                EnemyIdleState();
                break;
        }
    }
    protected void UpdateState()
    {
        if (stateSwitchTimer <= 0)
        {
            stateSwitchTimer = stateSwitchInterval;
            if (!PlayerWithinChaseRange())
            { CurrentEnemyState = EnemyState.Idle; }

            else if (PlayerWithinChaseRange() && !PlayerWithinAttackRange())
            { CurrentEnemyState = EnemyState.Chase; }

            else if (PlayerWithinChaseRange() && PlayerWithinAttackRange())
            { CurrentEnemyState = EnemyState.Attack; }
        }
        else { stateSwitchTimer -= GameTimeManager.GameDeltaTime; }

    }
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

    protected virtual IEnumerator BeginEnemyActionCooldown()
    {
        isEnemyOnCooldown = true;
        yield return GameTimeManager.WaitFor(GlobalAttackCooldownTime);
        isEnemyOnCooldown = false;
    }
    //Checks
    protected bool PlayerWithinChaseRange() //Checks if player is within chase range
    {
        if (ChaseRange == -1) { return false; }

        RaycastHit2D sight = Physics2D.Raycast(gameObject.transform.position, 
            GetPlayerDirection(), 
            ChaseRange,
            PlayerDetectionMask);

        if (sight.collider != null)
        {
            return sight.collider.GetComponentInParent<PlayerController>();
        }
        return false;
    }
    protected bool PlayerWithinAttackRange() //Checks if player is within attack range
    {
        if (AttackRange == -1) { return false; }

        RaycastHit2D sight = Physics2D.Raycast(gameObject.transform.position,
            GetPlayerDirection(),
            AttackRange,
            PlayerDetectionMask);

        if (sight.collider != null)
        {
            return sight.collider.GetComponentInParent<PlayerController>();
        }
        return false;
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
