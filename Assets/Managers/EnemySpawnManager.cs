using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
    //Ref to PoolManager
    BasePoolManager<EnemyType> PM => EnemyPoolManager.Instance;

    private List<SingleDungeonRoom> dungeonRooms = new List<SingleDungeonRoom>();
    private Dictionary<SingleDungeonRoom, List<Vector3Int>> roomSpawnPos = 
        new Dictionary<SingleDungeonRoom, List<Vector3Int>>();
    private void Awake()
    {
        if (DungeonInfo.Instance != null)
        {
            dungeonRooms = DungeonInfo.Instance.GetDungeonRoomList();
        }
    }
    private void Start()
    {
        if (dungeonRooms.Count == 0) { return; }

        foreach (SingleDungeonRoom room in dungeonRooms)
        {
            RectInt reducedBounds = ReduceBounds(room.Area, 1);
            List<Vector3Int> positions = new List<Vector3Int>();
            for (int x = reducedBounds.xMin; x < reducedBounds.xMax; x++)
            {
                for (int y = reducedBounds.yMin; y < reducedBounds.yMax; y++)
                {
                    positions.Add(new Vector3Int(x,y,0));
                }
            }
            roomSpawnPos[room] = positions;
            
        }

        //Enemy Spawning
        SpawnEnemies();
    }

    private RectInt ReduceBounds(RectInt bounds, int amount)
    {
        return new RectInt(bounds.xMin + amount, bounds.yMin + amount, bounds.width - amount, bounds.height - amount);
    }


    //Spawning
    private void SpawnEnemies()
    {
        //Runs through each rooms spawn positions and
        //spawns enemies that each location based on chance
        foreach(var Entry in roomSpawnPos)
        {
            List<Vector3Int> CurrentRoomList = Entry.Value;
            for (int i = 0; i < CurrentRoomList.Count; i++)
            {
                EnemyType RandomEnemy = GameManager.Instance.difficultyManager.GetEnemyToSpawn();
                if (RandomEnemy == EnemyType.Invalid) { }
                else
                {
                    PlaceEnemy(RandomEnemy, CurrentRoomList[i]);
                }
            }
        }
    }
    private void PlaceEnemy(EnemyType Enemy, Vector3Int Location)
    {
        GameObject EnemyObject = PM.getObjectFromPool(Enemy);
        EnemyObject.transform.position = Location;
        //Generic Enemy Starting Function
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
