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
public class Enemy : BaseHealth
{
    //Stats
    [Header("Speeds")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float AttackSpeed;

    //Behaviour Components
    private IEnemyMovementBehaviour EnemyMovementComponent;
    private IEnemyAttackBehaviour EnemyAttackComponent;
    private Transform playerLocation;
    private Transform enemyTransform;

    //Pathfinding
    private NavMeshAgent agent;

    private void Start()
    {
        //Pathfinding Settings
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = MoveSpeed;

        EnemyMovementComponent = GetComponent<IEnemyMovementBehaviour>();
        EnemyAttackComponent = GetComponent<IEnemyAttackBehaviour>();
        playerLocation = GameManager.Instance.getPlayer().transform;
        enemyTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        agent.SetDestination(playerLocation.position);
    }
}


