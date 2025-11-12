using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonV2Visuals : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tilemap FloorTM;
    [SerializeField] private Tilemap WallTM;
    [SerializeField] private RuleTile floorTileDefault;
    [SerializeField] private RuleTile wallTileDefault;

    //Additional Floor Options
    [Header("Additional Decoration Chances")]
    [SerializeField] private float FloorTileDecorChance = 0f;
    [SerializeField] private List<TileWithChance> ExtraFloorTiles = new List<TileWithChance>();


    private static readonly Vector2Int[] CardinalDir = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };


    [System.Serializable]
    private class TileWithChance
    {
        public RuleTile Tile;
        public float Chance;
    }

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
            //Place Default
            FloorTM.SetTile(Position, floorTileDefault);
        }
        //Place Decorated Floor Tiles
        PlaceDecorativePatches(PositionSet, ExtraFloorTiles, FloorTileDecorChance);
    }
    private void PlaceDecorativePatches(HashSet<Vector2Int> Positions, List<TileWithChance> TileList, float PercentOfTiles)
    {
        int patchCount = Mathf.CeilToInt(Positions.Count * PercentOfTiles);
        HashSet<Vector2Int> usedTiles = new HashSet<Vector2Int>();

        for (int i = 0; i < patchCount; i++)
        {
            //Select Random Starting Point
            Vector2Int seed = Positions.ElementAt(Random.Range(0, Positions.Count));
            if (usedTiles.Contains(seed)) { continue; }

            //Choose Patch Size
            int patchSize = Random.Range(4, 10);
            RuleTile SelectedTile = GetRandomTileFromList(TileList);

            //Create Patch
            Queue<Vector2Int> frontier = new Queue<Vector2Int>();
            frontier.Enqueue(seed);
            usedTiles.Add(seed);

            while (frontier.Count > 0 && patchSize > 0)
            {
                Vector2Int cur = frontier.Dequeue();
                patchSize--;

                FloorTM.SetTile((Vector3Int)cur, SelectedTile);

                //Expand to Random Neighbours
                foreach (Vector2Int dir in CardinalDir)
                {
                    Vector2Int neightbour = cur + dir;
                    if (Positions.Contains(neightbour) && !usedTiles.Contains(neightbour))
                    {
                        if(Random.value < 0.6f)
                        {
                            frontier.Enqueue(neightbour);
                            usedTiles.Add(neightbour);
                        }
                    }
                }
            }
        }
    }
    private RuleTile GetRandomTileFromList(List<TileWithChance> GivenList)
    {
        float TotalChance = 0f;
        foreach (TileWithChance Item in GivenList)
        {
            TotalChance += Item.Chance;
        }

        float choice = Random.Range(0, TotalChance);
        float cumulative = 0f;
        foreach (TileWithChance Item in GivenList)
        {
            cumulative += Item.Chance;
            if (choice <= cumulative)
            {
                return Item.Tile;
            }
        }

        //Fallback
        return GivenList[0].Tile;
        
    }
}
