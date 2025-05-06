using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : BaseEnemyMovement, IEnemyMovementBehaviour
{
    private DungeonRoom room;
    private List<Vector3Int> TeleportPositions = new List<Vector3Int>();
    public void Create()
    {
        room = DungeonInfo.Instance.GrabArea(gameObject.transform.position);
        TeleportPositions = DungeonInfo.Instance.GetEdgePositionFromPosition(gameObject.transform.position);
    }

    //Teleport Move
    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        CheckCurrentRoom();
        Vector3Int Location = PickRandomTeleportSpot();
        agent.Warp(GetPointOnMesh(Location));
    }
    private Vector3Int PickRandomTeleportSpot()
    {
        int random = Random.Range(0, TeleportPositions.Count);
        return TeleportPositions[random];
    }

    public void IdleMove(NavMeshAgent agent, float speed)
    {
        
    }

    private void CheckCurrentRoom()
    {
        if (room == null || room != DungeonInfo.Instance.GrabArea(gameObject.transform.position))
        {
            Create();
        }
        else
        {
            return;
        }
    }
}
