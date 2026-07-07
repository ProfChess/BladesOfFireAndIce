using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawnManager : MonoBehaviour
{
    //Ref to PoolManager
    BasePoolManager<EnemyType> PM => EnemyPoolManager.Instance;

    private List<SingleDungeonRoom> dungeonRooms = new();
    private List<CorridorDungeonRoom> corridors = new();
    private void Awake()
    {
        GameManager.Instance.getNavMesh().MeshCreated += EnemySpawnBegin;
    }
    private void EnemySpawnBegin()
    {
        if (DungeonCreationV2.Instance != null)
        {
            dungeonRooms = DungeonCreationV2.Instance.GetDungeonRooms;
            corridors = DungeonCreationV2.Instance.GetCorridors;
        }
        else { Debug.Log("Error: DungeonCreationV2 Instance was Null"); }

        if (dungeonRooms.Count == 0) { return; }

        //Enemy Spawning
        SpawnEnemies();
    }

    //Spawning
    private void SpawnEnemies()
    {
        //Spawn Enemies in Rooms
        foreach(var Entry in dungeonRooms)
        {
            foreach (Vector3Int Position in Entry.Area.allPositionsWithin)
            {
                EnemyType RandomEnemy = GameManager.Instance.difficultyManager.GetEnemyToSpawn();
                if (RandomEnemy == EnemyType.Invalid) { continue; }
                PlaceEnemy(RandomEnemy, Position, EnemyLeashType.Room, Entry);
            }
        }

        //Spawn Enemies in Corridors
        foreach (var Entry in corridors)
        {
            foreach (Vector3Int Position in Entry.Positions)
            {
                EnemyType RandomEnemy = GameManager.Instance.difficultyManager.GetEnemyToSpawn();
                if (RandomEnemy == EnemyType.Invalid) { continue; }
                PlaceEnemy(RandomEnemy, Position, EnemyLeashType.Point);
            }
        }
    }
    private void PlaceEnemy(EnemyType Enemy, Vector3Int Location, EnemyLeashType LeashType, SingleDungeonRoom room = null)
    {
        GameObject EnemyObject = PM.getObjectFromPool(Enemy);
        if (EnemyObject.TryGetComponent(out BaseEnemy EnemyComp))
        {
            EnemyComp.CreateEnemy((Vector2Int)Location, LeashType, room);
        }
    }
}
