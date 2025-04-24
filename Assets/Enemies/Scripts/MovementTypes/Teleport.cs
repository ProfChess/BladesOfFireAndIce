using UnityEngine;
using UnityEngine.AI;

public class Teleport : BaseEnemyMovement, IEnemyMovementBehaviour
{

    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        
    }

    public void IdleMove(NavMeshAgent agent, float speed)
    {

    }
}
