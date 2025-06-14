using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemySpawnManager : MonoBehaviour
{
    private List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();
    private Dictionary<DungeonRoom, List<Vector3Int>> roomSpawnPos = new Dictionary<DungeonRoom, List<Vector3Int>>();
    private void Awake()
    {
        dungeonRooms = DungeonInfo.Instance.GetDungeonRoomList();
    }
    private void Start()
    {
        foreach (DungeonRoom room in dungeonRooms)
        {
            BoundsInt reducedBounds = ReduceBounds(room.space, 1);
            List<Vector3Int> positions = new List<Vector3Int>();
            for (int x = reducedBounds.xMin; x < reducedBounds.xMax; x++)
            {
                for (int y = reducedBounds.yMin; y < reducedBounds.yMax; y++)
                {
                    positions.Add(new Vector3Int(x,y,0));
                }
            }
            roomSpawnPos[room] = positions;
            Debug.Log(positions.Count);
        }
    }

    private BoundsInt ReduceBounds(BoundsInt bounds, int amount)
    {
        return new BoundsInt(bounds.xMin + amount, bounds.yMin + amount,
            bounds.zMin, bounds.size.x - amount, bounds.size.y - amount, bounds.size.z);
    }


    void OnDrawGizmos()
    {
        if (roomSpawnPos == null) return;

        Gizmos.color = Color.red;

        foreach (var kvp in roomSpawnPos)
        {
            List<Vector3Int> positions = kvp.Value;

            foreach (Vector3Int gridPos in positions)
            {
                // Convert grid position to world position (center of cell)

                Gizmos.DrawSphere(gridPos, 0.1f);
            }
        }
    }

}
