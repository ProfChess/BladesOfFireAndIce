using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove_Wander : BaseEnemyMovement
{
    [Tooltip("Determines How Long Agent Waits After Arrived at Wandering Point")]
    [SerializeField] private float WanderInterval = 0.1f;
    private float WanderTimer = 0f;
    [Tooltip("Determines Min Distance Agent Can Wander")]
    [SerializeField] private float MinWanderRange = 1f;
    [Tooltip("Determines Max Distance Agent Can Wander")]
    [SerializeField] private float MaxWanderRange = 3f;
    public override void EnemyMove(NavMeshAgent agent, float speed)
    {
        agent.speed = speed;
        if (!isArrived(agent)) { return; }

        if (WanderTimer <= 0f)
        {
            float Distance = Random.Range(MinWanderRange, MaxWanderRange);
            Vector2 TargetPositon = GetPointAroundEnemy(Distance);
            agent.SetDestination(TargetPositon);

            WanderTimer = WanderInterval;
        }
        else { WanderTimer -= GameTimeManager.GameDeltaTime; }
    }
}
