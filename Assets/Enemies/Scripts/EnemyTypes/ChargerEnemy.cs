using System.Collections;
using UnityEngine;

public class ChargerEnemy : BaseEnemy
{
    [Tooltip("Controls How Long (Seconds) the Enemy Waits at Each Patrol Point")]
    [SerializeField] private float TimeBetweenPatrols;
    private float WaitTimeRemaining = 0;
    private bool isWaiting = false;

    //Extra Attack
    private SwordSwing NormalEnemyAttack;


    //Charge Attack Settings
    [SerializeField] private float ChargeCooldownTime;
    private bool CanChargeAttack;
    protected override void Start()
    {
        base.Start();
        NormalEnemyAttack = GetComponent<SwordSwing>();
    }

    //Enemy Will Patrol Between 2 Points of its Starting Room in its Idle State
    protected override void EnemyIdleState()
    {
        if (Arrived())
        {
            //Start Waiting
            if (!isWaiting)
            {
                isWaiting = true;
                WaitTimeRemaining = Time.time + TimeBetweenPatrols;
            }

            //Still Waiting
            if (Time.time >= WaitTimeRemaining)
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
        
        base.EnemyIdleState();
    }


    //Enemy Will Use Their Spin Attack On Cooldown When Player is Within Chase Range
    protected override void EnemyChaseState()
    {
        if (CanChargeAttack)
        {
            ChargeApproachState();
        }
        else
        {
            NormalApproachState();
        }
    }
    private void NormalApproachState()
    {

    }
    private void ChargeApproachState()
    {

    }
    private IEnumerator ChargeAttackCooldown()
    {
        CanChargeAttack = false;
        yield return new WaitForSeconds(ChargeCooldownTime);
        CanChargeAttack = true;
    }


    //Enemy Will Use Normal Attack When in Attack Range (Close)
    protected override void EnemyAttackState()
    {

    }


}
