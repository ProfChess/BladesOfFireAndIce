using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonV2Visuals : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tilemap FloorTM;
    [SerializeField] private Tilemap WallTM;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTile;

    private void Awake()
    {
        DungeonCreationV2.DungeonDataCreated += PlaceTiles;
    }
    private void PlaceTiles()
    {
        HashSet<Vector2Int> PositionSet = DungeonCreationV2.GetTilePlaces;
        foreach (Vector3Int Position in PositionSet)
        {
            //Place Floor Tiles
            FloorTM.SetTile(Position, floorTile);
        }
    }
}
