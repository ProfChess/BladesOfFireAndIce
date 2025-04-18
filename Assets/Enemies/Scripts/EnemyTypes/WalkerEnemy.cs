using UnityEngine;
using UnityEngine.AI;

public class WalkerEnemy : BaseEnemy
{

    //Wandering Behaviour Stats
    private float wanderInterval = 2f;
    private float nextWanderTime = 0f;

    //Idle
    protected override void EnemyIdleState() 
    {
        //Idle Specific Stats
        agent.stoppingDistance = 1;

        //Wanders around every interval
        if (Time.time >= nextWanderTime && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            EnemyMovementComponent.IdleMove(agent, IdleSpeed);
            EnemySprite.flipX = agent.destination.x <= transform.position.x;
            nextWanderTime = Time.time + wanderInterval;
        }

        base.EnemyIdleState();
    }


    //Chase
    protected override void EnemyChaseState()
    {
        anim.SetBool("IsRunning", true); anim.SetBool("IsWalking", false);
        EnemyMovementComponent.ChaseMove(agent, playerLocation, ChaseSpeed, AttackRange);
    }

    //Attack
    protected override void EnemyAttackState()
    {
        anim.SetBool("IsRunning", false); anim.SetBool("IsWalking", false);
        if (canAttack) { canAttack = false;  anim.SetTrigger("AttackTrigger"); }
    }


}


