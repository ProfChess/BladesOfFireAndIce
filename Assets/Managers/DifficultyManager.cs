using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    //Difficulty Settings
    [SerializeField] private float DifficultyLevel = 0;
    [SerializeField] private Vector2Int RoomSize = new Vector2Int(0, 0);
    [SerializeField] private Vector3Int MapSize = new Vector3Int(0, 0, 0);
    
    public Vector2Int GetRoomSize() { return  RoomSize; }
    public Vector3Int GetMapSize() { return MapSize; }

}
