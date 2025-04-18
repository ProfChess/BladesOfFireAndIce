using System.Collections;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    //For this Enemy -> Attacks at chase range and flees at Attack Range

    [Header("Run Away Stats")]
    private bool CanRunAway = true;
    [SerializeField] private float RunAwayCooldown;
    [SerializeField] private float RunAwayDistance;

    [Header("Idle Wander Stats")]
    [SerializeField] private float IdleWanderInterval;
    private float idleWanderTimer = 0f;

    protected override void EnemyIdleState()
    {
        agent.isStopped = false;
       
        if (Time.time >= idleWanderTimer && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            EnemyMovementComponent.IdleMove(agent, IdleSpeed);
            EnemySprite.flipX = agent.destination.x <= transform.position.x;
            idleWanderTimer = Time.time + IdleWanderInterval;
        }
        base.EnemyIdleState();

    }

    protected override void EnemyChaseState() 
    {
        if (canAttack)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        anim.SetBool("IsRunning", false); anim.SetBool("IsWalking", false);
        if (canAttack) { canAttack = false; anim.Play("ArcherAttack"); }

    }

    protected override void EnemyAttackState()
    {
        agent.isStopped = false;

        anim.SetBool("IsWalking", false);
        anim.SetBool("IsRunning", agent.velocity.magnitude > 0);
        //Runs Away When too Close to Player
        if (CanRunAway)
        {
            EnemyMovementComponent.ChaseMove(agent, playerLocation, ChaseSpeed, RunAwayDistance);
            StartCoroutine(RunAwayAbilityCooldown());
        }
        if (!CanRunAway && agent.pathStatus == 0)
        {
            EnemyChaseState();
        }
    }
    private IEnumerator RunAwayAbilityCooldown()
    {
        CanRunAway = false;
        yield return new WaitForSeconds(RunAwayCooldown);
        CanRunAway = true;
    }
}
