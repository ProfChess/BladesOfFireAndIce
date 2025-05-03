using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : BaseEnemyMovement, IEnemyMovementBehaviour
{
    private List<Vector3Int> TeleportPositions = new List<Vector3Int>();
    public void Create()
    {
        TeleportPositions = DungeonInfo.Instance.GetEdgePositionFromPosition(gameObject.transform.position);
    }
    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        
    }

    public void IdleMove(NavMeshAgent agent, float speed)
    {
        
    }
}
