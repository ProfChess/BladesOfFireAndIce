using UnityEngine;
using UnityEngine.AI;

public class WalkerEnemy : BaseEnemy
{

    //Wandering Behaviour Stats
    [Header("Wandering")]
    [SerializeField] private int WanderRadius;
    private float wanderInterval = 2f;
    private float nextWanderTime = 0f;

    //Idle
    protected override void EnemyIdleState() 
    {
        //Anim
        anim.SetBool("IsRunning", false);

        //Idle Specific Stats
        agent.speed = IdleSpeed;
        agent.stoppingDistance = 1;

        //Wanders around every interval
        if (Time.time >= nextWanderTime && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Wander();
            nextWanderTime = Time.time + wanderInterval;
        }

        //Anim Bools Sets
        if (agent.velocity != Vector3.zero) 
        { 
            anim.SetBool("IsWalking", true); anim.SetBool("IsRunning", false); 

            //Flip Sprie in Walking Direction
            EnemySprite.flipX = agent.destination.x <= transform.position.x;
        }
        else { anim.SetBool("IsWalking", false); }
    }
    //Wandering
    private void Wander() //Sets destination to wander to
    {
        Vector2 PointOfInterest = GetRandomWanderPoint();
        if (PointOfInterest != Vector2.zero) { agent.SetDestination(PointOfInterest); }
    }
    private Vector2 GetRandomWanderPoint() //Picks random spot on the navmesh within a circle around enemy
    {
        Vector3 RandomDirection = Random.insideUnitCircle * WanderRadius;
        RandomDirection += transform.position;

        if (NavMesh.SamplePosition(RandomDirection, out NavMeshHit hit, WanderRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }


    //Chase
    protected override void EnemyChaseState()
    {
        anim.SetBool("IsRunning", true); anim.SetBool("IsWalking", false);
        agent.stoppingDistance = AttackRange;
        EnemyMovementComponent.Move(agent, playerLocation, ChaseSpeed);
    }

    //Attack
    protected override void EnemyAttackState()
    {
        anim.SetBool("IsRunning", false); anim.SetBool("IsWalking", false);
        if (canAttack) { canAttack = false;  anim.Play("SlimeAttack"); }
    }


}


