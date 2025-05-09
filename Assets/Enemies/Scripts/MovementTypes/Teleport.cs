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
        Vector3 Pos = room.space.center;
        float Diff = 0f;
        Diff = playerTransform.position.x > gameObject.transform.position.x ? 1.5f : -1.5f;

        Vector3 Target = new Vector3(playerTransform.position.x + Diff, playerTransform.position.y, 0);
        Vector2 Attempt = GetPointOnMesh(Target);
        if (Attempt == Vector2.zero) { agent.Warp(GetPointOnMesh(Pos)); }

        else { agent.Warp(Attempt); }
    }
    private Vector3Int PickRandomTeleportSpot()
    {
        int random = Random.Range(0, TeleportPositions.Count);
        return TeleportPositions[random];
    }

    public void IdleMove(NavMeshAgent agent, float speed)
    {
        CheckCurrentRoom();
        Vector3Int Location = PickRandomTeleportSpot();
        agent.Warp(GetPointOnMesh(Location));
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
