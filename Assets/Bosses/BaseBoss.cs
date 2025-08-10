using UnityEngine;
using UnityEngine.AI;

public abstract class BaseBoss : BaseHealth
{
    //Movement
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float attackCooldown;
    protected NavMeshAgent BossAgent;

    //Attacking
    protected bool isAttacking = false;
    protected float attackTimer;
    protected Transform playerLocation;

    //State Updates
    private float UpdateDelay = 0.2f;
    private float nextUpdateCheck = 0f;


    private void OnEnable()
    {
        GameManager.Instance.getNavMesh().MeshCreated += CreateBossNavAgent;
    }
    private void OnDisable()
    {
        GameManager.Instance.getNavMesh().MeshCreated -= CreateBossNavAgent;
    }

    private void Start()
    {
        playerLocation = GameManager.Instance.getPlayer().transform;
        CreateBossNavAgent();
    }

    private void Update()
    {
        if (Time.time > nextUpdateCheck)
        {
            nextUpdateCheck = Time.time + UpdateDelay;
            if (!isAttacking)
            {
                MoveUpdate();
                AttackSelection();
            }
        }
    }

    protected abstract void MoveUpdate();       //Override logic to move around (toward player)
    protected abstract void AttackSelection();  //Override logic to determine attack (check distance, select randomly etc)
    protected void NowAttacking() { isAttacking = true; }   
    public void NotAttacking() { isAttacking = false; }



    protected virtual void CreateBossNavAgent()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }
        BossAgent = GetComponent<NavMeshAgent>();
        BossAgent.agentTypeID = 0;
        BossAgent.updateRotation = false;
        BossAgent.updateUpAxis = false;
        BossAgent.speed = MoveSpeed;
    }
    protected float GetDistanceToPlayer() { return Vector2.Distance(gameObject.transform.position, playerLocation.position); }
}
