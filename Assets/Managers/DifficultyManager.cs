using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    //Difficulty Settings
    [SerializeField, Range(0, 3)] private int DifficultyLevel = 0;
    [SerializeField] private Vector2Int RoomSize = new Vector2Int(0, 0);
    [SerializeField] private Vector3Int MapSize = new Vector3Int(0, 0, 0);
    [SerializeField] private bool TestingMode = false;
    public bool isTesting => TestingMode;

    private void Awake()
    {
        DifficultyLevel = Mathf.Clamp(DifficultyLevel, 0, GlobalEnemySpawnChance.Length - 1);
    }

    public Vector2Int GetRoomSize() { return  RoomSize; }
    public Vector3Int GetMapSize() { return MapSize; }

    //ENEMY SPAWN CHANCES
    //Enemy and Difficulty Spawning
    private int[] GlobalEnemySpawnChance = new int[] { 15, 25, 35, 45 }; //Out of 100

    //Enum for Spawn Chances
    private enum EnemySpawnTypes
    {
        Slime, 
        Archer,
        Axeman,
        Caster,
        Elite1,
        Elite2,
        FailedSpawn,
    }

    //Enemy Chances to Spawn
    private Dictionary<EnemySpawnTypes, int[]> EnemyTypeSpawnChances = 
        new Dictionary<EnemySpawnTypes, int[]> 
        {
            { EnemySpawnTypes.Slime, new int[] {50, 35, 25, 15} },
            { EnemySpawnTypes.Archer, new int[] {40, 30, 20, 10} },
            { EnemySpawnTypes.Axeman, new int[] {40, 30, 20, 10} },
            { EnemySpawnTypes.Caster, new int[] {20, 10, 15, 25} },
            { EnemySpawnTypes.Elite1, new int[] {10, 10, 20, 30} },
            { EnemySpawnTypes.Elite2, new int[] {10, 10, 20, 30} },
        };
    //Access Dictionary
    public EnemyType GetEnemyToSpawn()
    {
        EnemySpawnTypes Enemy = SpawnRandomEnemy();
        EnemyType Choice = EnemyType.Invalid;
        if (Enemy != EnemySpawnTypes.FailedSpawn)
        {
            switch (Enemy)
            {
                default: Debug.Log("Enemy Type Not Found");
                    break;
                case EnemySpawnTypes.Slime:
                    Choice = EnemyType.Slime;
                    break;
                case EnemySpawnTypes.Archer:
                    Choice = EnemyType.Ranged;
                    break;
                case EnemySpawnTypes.Axeman:
                    Choice = EnemyType.Charger;
                    break;
                case EnemySpawnTypes.Caster:
                    Choice = EnemyType.Caster;
                    break;
                case EnemySpawnTypes.Elite1:
                    Choice = EnemyType.EliteEnemy1;
                    break;
                case EnemySpawnTypes.Elite2:
                    Choice = EnemyType.EliteEnemy2;
                    break;
            }
        }
        return Choice;
    } 
    
    //Calculate Chances
    private EnemySpawnTypes SpawnRandomEnemy()
    {
        int RollForGlobalSpawn = Random.Range(0, 100);
        if (RollForGlobalSpawn < GlobalEnemySpawnChance[DifficultyLevel])
        {
            int cumulativeChance = 0;
            //Successful Spawn
            foreach (var entry in EnemyTypeSpawnChances) //Combines each chance based on difficulty
            {
                int[] valueList = EnemyTypeSpawnChances[entry.Key];
                int correctValue = valueList[DifficultyLevel];
                cumulativeChance += correctValue;
            }

            //Pick a random number 
            int choice = Random.Range(0, cumulativeChance);
            int currentTotal = 0;

            //Check Each Enemy Chance
            foreach (var entry in EnemyTypeSpawnChances)
            {
                int[] valueList = entry.Value;
                int weight = valueList[DifficultyLevel];

                currentTotal += weight;
                if (choice < currentTotal)
                {
                    EnemySpawnTypes SelectedEnemy = entry.Key;
                    return SelectedEnemy;
                }
            }
        }
        return EnemySpawnTypes.FailedSpawn;
    }

}
