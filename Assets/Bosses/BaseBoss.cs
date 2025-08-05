using UnityEngine;

public abstract class BaseBoss : BaseHealth
{

    //Movement
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float attackCooldown;

    //Attacking
    protected bool isAttacking;
    protected float attackTimer;
    protected Transform playerLocation;

    private void Start()
    {
        playerLocation = GameManager.Instance.getPlayer().transform;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            MoveUpdate();
            AttackSelection();
        }
    }

    protected abstract void MoveUpdate();       //Override logic to move around (toward player)
    protected abstract void AttackSelection();  //Override logic to determine attack (check distance, select randomly etc)


}
