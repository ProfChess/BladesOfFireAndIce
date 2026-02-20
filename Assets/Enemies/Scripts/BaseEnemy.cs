using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


//Interfaces For Enemy Uses
public interface IEnemyMovementBehaviour
{
    void IdleMove(NavMeshAgent agent, float speed);
    void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range);
}
public interface IEnemyAttackBehaviour
{
    void Attack(float Range, int Cooldown, float Offset, Transform playerTransform);
}


public abstract class BaseEnemy : MonoBehaviour
{
    //Stats
    [Header("Idle Settings")]
    [Tooltip("Speed Enemy Moves in Idle State")]
    [SerializeField] protected float IdleSpeed; 

    [Header("Chase Settings")]
    [Tooltip("Speed Enemy Moves in Chase State")]
    [SerializeField] protected float ChaseSpeed;
    [Tooltip("Distance From Player to Trigger Chase State")]
    [SerializeField] protected int ChaseRange;

    [Header("Basic Attack Settings")]
    [Tooltip("Time Inbetween Each Basic Attack")]
    [SerializeField] protected int AttackCooldown;
    [Tooltip("Offset of Basic Attack Box / Speed of Projectiles")]
    [SerializeField] protected float AttackOffset;
    [Tooltip("Distance From Player to Trigger Attack State")]
    [SerializeField] protected float AttackRange;

    //Animation
    protected Animator anim;
    protected SpriteRenderer EnemySprite;
    //Bools
    protected static readonly int Walking = Animator.StringToHash("IsWalking");
    protected static readonly int Running = Animator.StringToHash("IsRunning");

    public SpriteRenderer GetSpriteRenderer() { return EnemySprite; }
    public Animator GetAnimator() { return anim; }
    //States
    protected enum EnemyState {Idle, Chase, Attack}
    protected EnemyState CurrentEnemyState;

    //Spawning 
    [Header("Pool Type Selection")]
    [SerializeField] protected EnemyType EnemyPoolType;

    //Behaviour Components
    protected IEnemyMovementBehaviour EnemyMovementComponent;
    protected IEnemyAttackBehaviour EnemyAttackComponent;
    protected Transform playerLocation;
    protected Transform enemyTransform;
    protected LayerMask PlayerDetectionMask;

    //Pathfinding
    protected NavMeshAgent agent;

    //Timers
    protected const float checkInterval = 0.2f;
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

    public void DeactivateEnemy()
    {
        CustomEnemyDeathLogic();
        gameObject.SetActive(false);
        PM.ReturnObjectToPool(EnemyPoolType, gameObject);
    }
    protected virtual void CustomEnemyDeathLogic() { }
    
    protected virtual void Start()
    {
        //Visuals 
        anim = GetComponentInChildren<Animator>();
        EnemySprite = GetComponentInChildren<SpriteRenderer>();

        //Initialize State
        CurrentEnemyState = EnemyState.Idle;

        //Layer
        PlayerDetectionMask = LayerMask.GetMask("Player", "Walls");

        EnemyMovementComponent = GetComponent<IEnemyMovementBehaviour>();
        EnemyAttackComponent = GetComponent<IEnemyAttackBehaviour>();
        playerLocation = GameManager.Instance.getPlayer().transform;
        enemyTransform = GetComponent<Transform>();
        CreateAgent(); //NOTE --> WILL LIKELY GET CHANGED TO JUST ASSIGNING REFERENCE TO NAVAGENT
    }

    //Switches State Based Upon Player Distance to Enemy
    virtual protected void Update()
    {
        if (!canMove) { return; }

        //States
        else if (Time.time >= nextCheck)
        {
            nextCheck = Time.time + checkInterval;
            if (!PlayerWithinChaseRange()) 
            { CurrentEnemyState = EnemyState.Idle; }

            else if (PlayerWithinChaseRange() && !PlayerWithinAttackRange())
            { CurrentEnemyState = EnemyState.Chase; }

            else if (PlayerWithinChaseRange() && PlayerWithinAttackRange()) 
            { CurrentEnemyState = EnemyState.Attack; }

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
    }
    //State Functions (Override in Inherited Class)
    protected virtual void EnemyIdleState()
    {
        anim.SetBool(Running, false);

        if (agent.velocity.sqrMagnitude > 0)
        {
            anim.SetBool(Walking, true);
        }
        else { anim.SetBool(Walking, false); }
    }
    protected virtual void EnemyChaseState() { FlipSprite(); }
    protected virtual void EnemyAttackState() { FlipSprite(); }


    //Visuals
    protected void FlipSprite()
    {
        EnemySprite.flipX = playerLocation.position.x <= transform.position.x;
    }

    //Cooldowns
    protected virtual IEnumerator BasicAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }
    public void StartEnemyCooldown() { StartCoroutine(BasicAttackCooldown()); }
    public void StartEnemyAttackDamage() 
    { 
        EnemyAttackComponent.Attack(AttackRange, AttackCooldown, AttackOffset, playerLocation); 
    }
    //Checks
    protected bool PlayerWithinChaseRange() //Checks if player is within chase range
    {
        if (ChaseRange == -1) { return false; }

        RaycastHit2D sight = Physics2D.Raycast(enemyTransform.position, 
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

        RaycastHit2D sight = Physics2D.Raycast(enemyTransform.position,
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
    protected Vector2 GetPlayerDistance() { return (playerLocation.position - enemyTransform.position); }
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
        agent.speed = IdleSpeed;
        agent.acceleration = 500;
        agent.stoppingDistance = 0.5f;
        agent.autoBraking = false;
        agent.radius = 0.2f;
    }

    public bool Arrived() //Checks if Agent has Arrived at destination
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    //Default way for enemy to move in idle state, wandering around
    protected void IdleWanderThenWait(ref bool isWaiting, ref float Timer, float WaitTime)
    {
        if (Arrived())
        {
            //Start Waiting
            if (!isWaiting)
            {
                isWaiting = true;
                Timer = Time.time + WaitTime;
            }

            //Still Waiting
            if (Time.time >= Timer)
            {
                isWaiting = false;
                EnemyMovementComponent.IdleMove(agent, IdleSpeed);
                EnemySprite.flipX = agent.destination.x <= transform.position.x;
            }
        }
        else //Not at destination
        {
            isWaiting = false;
        }
    }
}
