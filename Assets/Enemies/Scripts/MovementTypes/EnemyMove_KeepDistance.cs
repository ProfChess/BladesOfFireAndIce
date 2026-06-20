using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove_KeepDistance : BaseEnemyMovement
{
    [SerializeField] private float EscapeDistance = 5f;
    [SerializeField] private float RepathCooldown = 2f;
    [SerializeField] private float MaxSnapDistance = 1f;
    private float repathTimer = 0f;
    private float[] angles = { 0f, 30f, -30f, 60f, -60f, 90f, -90f };

    public override void EnemyMove(NavMeshAgent agent, float speed)
    {
        agent.speed = speed;
        repathTimer -= GameTimeManager.GameDeltaTime;
        if (repathTimer > 0f) { return; }

        repathTimer = RepathCooldown;
        
        Vector2 EscapeDirection = ((Vector2)transform.position - GetPlayerLocation()).normalized;

        Vector2 destination = FindBestEscapePoint(transform.position, EscapeDirection);

        agent.SetDestination(destination);
    }
    private Vector2 FindBestEscapePoint(Vector2 Origin, Vector2 EscapeDirection)
    {
        Vector2 bestPoint = Origin;
        float highScore = float.MinValue;

        foreach(float angle in angles)
        {
            Vector2 dir = Quaternion.Euler(0, 0, angle) * EscapeDirection;
            Vector2 desiredPosition = Origin + dir * EscapeDistance;
            if(!TryGetPointOnMesh(desiredPosition, MaxSnapDistance, out Vector2 ValidPoint)) { continue; }

            float score = Vector2.Distance(ValidPoint, GetPlayerLocation());
            if (score > highScore) { highScore = score; bestPoint = ValidPoint; }
        }
        return bestPoint;
    }
    
}
