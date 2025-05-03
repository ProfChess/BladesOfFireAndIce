using System.Collections;
using UnityEngine;

public class RangeEnemy : BaseEnemy
{
    //ANIMATION STRINGS
    //States
    private static readonly int Attack = Animator.StringToHash("ArcherAttack");
    //For this Enemy -> Attacks at chase range and flees at Attack Range

    [Header("Run Away Stats")]
    private bool CanRunAway = true;
    [SerializeField] private float RunAwayCooldown;
    [SerializeField] private float RunAwayDistance;

    [Header("Idle Wander Stats")]
    [SerializeField] private float IdleWanderWaitTime;
    private float idleWanderTimer = 0f;
    private bool isWaiting = false;

    protected override void EnemyIdleState()
    {
        agent.isStopped = false;
        IdleWanderThenWait(ref isWaiting, ref idleWanderTimer, IdleWanderWaitTime);
        base.EnemyIdleState();

    }

    protected override void EnemyChaseState() 
    {
        if (canAttack)
        {
            agent.isStopped = true;
            canAttack = false; anim.Play(Attack);
        }
        else
        {
            agent.isStopped = false;
        }
        anim.SetBool(Walking, false);
        anim.SetBool(Running, agent.velocity.magnitude > 0);
    }

    protected override void EnemyAttackState()
    {
        agent.isStopped = false;

        anim.SetBool(Walking, false);
        anim.SetBool(Running, agent.velocity.magnitude > 0);
        
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
