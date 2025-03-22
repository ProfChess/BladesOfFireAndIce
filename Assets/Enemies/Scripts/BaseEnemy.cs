using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


//Interfaces For Enemy Uses
public interface IEnemyMovementBehaviour
{
    void Move(Transform enemyTransform, Transform playerTransform, float speed);
}
public interface IEnemyAttackBehaviour
{
    void Attack();
}

public class BaseEnemy : BaseHealth
{
    //Stats
    [Header("Speeds")]
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected int ChaseRange;

    //States
    protected enum EnemyState {Idle, Chase, Attack}
    protected EnemyState CurrentEnemyState;

    //Behaviour Components
    protected IEnemyMovementBehaviour EnemyMovementComponent;
    protected IEnemyAttackBehaviour EnemyAttackComponent;
    protected Transform playerLocation;
    protected Transform enemyTransform;
    protected LayerMask PlayerDetectionMask;

    //Pathfinding
    protected NavMeshAgent agent;

    //Events
    protected void OnEnable()
    {
        GameManager.Instance.getNavMesh().MeshCreated += CreateAgent;
    }
    protected void OnDisable()
    {
        GameManager.Instance.getNavMesh().MeshCreated -= CreateAgent;
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

    protected bool PlayerWithinRange() //Checks if player is within chase range
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
    protected Vector2 GetPlayerDirection() //Returns player direction from objects position
    {
        return (playerLocation.position - enemyTransform.position).normalized;
    }

    protected void CreateAgent() //Setup for adding agent at runtime
    {
        //Pathfinding Settings
        if (gameObject.GetComponent<NavMeshAgent>() == null) {  gameObject.AddComponent<NavMeshAgent>(); }
        agent = GetComponent<NavMeshAgent>();
        agent.agentTypeID = 0;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = MoveSpeed;
    }
}
