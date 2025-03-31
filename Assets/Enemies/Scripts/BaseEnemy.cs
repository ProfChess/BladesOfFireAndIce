using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;


//Interfaces For Enemy Uses
public interface IEnemyMovementBehaviour
{
    void Move(NavMeshAgent agent, Transform playerTransform, float speed);
}
public interface IEnemyAttackBehaviour
{
    void Attack(float Damage, float Range, int Cooldown, int Speed, Transform playerTransform);
}

public enum PoolType { Slime }

public class BaseEnemy : MonoBehaviour
{
    //Stats
    [Header("Stats")]
    [Header("Idle")]
    [SerializeField] protected float IdleSpeed;
    [Header("Chase")]
    [SerializeField] protected float ChaseSpeed;
    [SerializeField] protected int ChaseRange;
    [Header("Attack")]
    [SerializeField] protected float AttackDamage;
    [SerializeField] protected int AttackCooldown;
    [SerializeField] protected int AttackSpeed;
    [SerializeField] protected float AttackRange;
    [Header("Visuals")]
    [SerializeField] protected SpriteRenderer EnemySprite;
    [SerializeField] protected Animator anim;

    //States
    protected enum EnemyState {Idle, Chase, Attack}
    protected EnemyState CurrentEnemyState;

    //Spawning 
    [SerializeField] protected PoolType EnemyPoolType;

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

    //Events
    protected void OnEnable()
    {
        GameManager.Instance.getNavMesh().MeshCreated += CreateAgent;
    }
    protected void OnDisable()
    {
        GameManager.Instance.getNavMesh().MeshCreated -= CreateAgent;
    }

    //Init
    public void CreateEnemy(Vector2 Position)
    {
        //Move to Correct Position
        gameObject.transform.position = Position;

        //Restart State
        CurrentEnemyState = EnemyState.Idle;
        gameObject.SetActive(true);
    }
    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
        PoolManager.Instance.ReturnObjectToPool(EnemyPoolType, gameObject);
    }

    protected void Start()
    {
        //Initialize State
        CurrentEnemyState = EnemyState.Idle;

        //Layer
        PlayerDetectionMask = LayerMask.GetMask("Player", "Walls");

        EnemyMovementComponent = GetComponent<IEnemyMovementBehaviour>();
        EnemyAttackComponent = GetComponent<IEnemyAttackBehaviour>();
        playerLocation = GameManager.Instance.getPlayer().transform;
        enemyTransform = GetComponent<Transform>();

    }

    virtual protected void Update()
    {
        //States
        if (Time.time >= nextCheck)
        {
            nextCheck = Time.time + checkInterval;
            if (!PlayerWithinChaseRange()) { CurrentEnemyState = EnemyState.Idle; }
            else if (PlayerWithinChaseRange() && !PlayerWithinAttackRange()) { CurrentEnemyState = EnemyState.Chase; }
            else if (PlayerWithinChaseRange() && PlayerWithinAttackRange()) { CurrentEnemyState = EnemyState.Attack; }

            //Call functions for each state
            switch (CurrentEnemyState)
            {
                case EnemyState.Chase:
                    EnemyChaseState();
                    FlipSprite();
                    break;

                case EnemyState.Attack:
                    EnemyAttackState();
                    FlipSprite();
                    break;

                default:
                    EnemyIdleState();
                    break;
            }
        }
    }
    //State Functions (Override in Inherited Class)
    protected virtual void EnemyIdleState() { }
    protected virtual void EnemyChaseState() { }
    protected virtual void EnemyAttackState() { }

    //Visuals
    protected void FlipSprite()
    {
        EnemySprite.flipX = playerLocation.position.x <= transform.position.x;
    }
    //Visual Gets
    public Animator GetAnim() {  return anim; }


    //Cooldowns
    protected IEnumerator BasicAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }
    public void StartEnemyCooldown() { StartCoroutine(BasicAttackCooldown()); }
    public void StartEnemyAttackDamage() 
    { 
        EnemyAttackComponent.Attack(AttackDamage, AttackRange, AttackCooldown, AttackSpeed, playerLocation); 
    }
    //Checks
    protected bool PlayerWithinChaseRange() //Checks if player is within chase range
    {
        RaycastHit2D sight = Physics2D.Raycast(enemyTransform.position, 
            GetPlayerDirection(), 
            ChaseRange,
            PlayerDetectionMask);

        if (sight.collider != null)
        {
            return sight.collider.CompareTag("Player");
        }
        return false;
    }
    protected bool PlayerWithinAttackRange() //Checks if player is within attack range
    {
        RaycastHit2D sight = Physics2D.Raycast(enemyTransform.position,
            GetPlayerDirection(),
            AttackRange,
            PlayerDetectionMask);

        if (sight.collider != null)
        {
            return sight.collider.CompareTag("Player");
        }
        return false;
    }
    protected Vector2 GetPlayerDirection() //Returns player direction from objects position
    {
        return (playerLocation.position - enemyTransform.position).normalized;
    }

    //NavAgent
    protected void CreateAgent() //Setup for adding agent at runtime
    {
        //Pathfinding Settings
        if (gameObject.GetComponent<NavMeshAgent>() == null) {  gameObject.AddComponent<NavMeshAgent>(); }
        agent = GetComponent<NavMeshAgent>();
        agent.agentTypeID = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = IdleSpeed;
        agent.acceleration = 500;
    }

    
}
