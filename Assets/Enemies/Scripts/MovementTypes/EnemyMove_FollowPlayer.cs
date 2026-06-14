using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove_FollowPlayer : BaseEnemyMovement
{
    [SerializeField] private float MovementUpdatenInterval = 0.2f;
    private float positionUpdateTimer = 0f;
    public override void EnemyMove(NavMeshAgent agent, float speed)
    {
        if(positionUpdateTimer <= 0f)
        {
            positionUpdateTimer = MovementUpdatenInterval;
            agent.speed = speed;
            Vector2 TargetLocation = GetPointOnMesh(GetPlayerLocation());
            agent.SetDestination(TargetLocation);
        }
        else { positionUpdateTimer -= GameTimeManager.GameDeltaTime; }

    }
}
