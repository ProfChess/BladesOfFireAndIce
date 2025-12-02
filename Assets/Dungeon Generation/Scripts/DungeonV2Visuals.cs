using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DungeonV2Visuals : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tilemap FloorTM;
    [SerializeField] private Tilemap WallTM;
    [SerializeField] private TileSet ChosenTileSet;
    [SerializeField] private List<Tilemap> DecorationTileMaps;
    

    //Floors And Base
    private HashSet<Vector2Int> WallPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> AdditionalFloorSpaces = new HashSet<Vector2Int>(); //For walls that must turn into floor
    private HashSet<Vector2Int> EmptyWallPositions = new HashSet<Vector2Int>();
    
    //Tops
    private HashSet<Vector2Int> BottomWallPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> EastWallTopPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> WestWallTopPositons = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> SouthWallTopPositions = new HashSet<Vector2Int>();

    //Outer Corners
    private HashSet<Vector2Int> BottomLeftWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> BottomRightWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> TopRightWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> TopLeftWallCorners = new HashSet<Vector2Int>();

    //Inner Corners
    private HashSet<Vector2Int> InnerNECorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> InnerNWCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> InnerSECorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> InnerSWCorners = new HashSet<Vector2Int>();

    //Additional Floor Options
    [Header("Additional Decoration Chances")]
    [SerializeField] private float FloorTileDecorChance = 0f;
    [SerializeField] private float WallTileDecorChance = 0f;
    [SerializeField] private List<TileWithChance> ExtraFloorTiles = new List<TileWithChance>();
    [SerializeField] private List<TileWithChance> ExtraWallTiles = new List<TileWithChance>();


    private static readonly Vector2Int[] CardinalDir = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };
    private static readonly Vector2Int[] AllDirections = new Vector2Int[]
    {
        new Vector2Int(1, 0),   //East
        new Vector2Int(1, -1),  //South East
        new Vector2Int(0, -1),  //South
        new Vector2Int(-1, -1), //South West
        new Vector2Int(-1, 0),  //West
        new Vector2Int(-1, 1),  //North West
        new Vector2Int(0, 1),   //North
        new Vector2Int(1, 1),   //North East
    };

    private HashSet<Vector2Int> FloorPositionSet = new HashSet<Vector2Int>();

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
        FloorPositionSet = DungeonCreationV2.Instance.GetTilePlaces;
        foreach (Vector2Int Position in FloorPositionSet)
        {
            //Place Floor Tiles
            //Place Default
            FloorTM.SetTile((Vector3Int)Position, ChosenTileSet.DefaultFloor);
        }
        //Place Decorated Floor Tiles
        PlaceDecorativePatches(FloorTM, FloorPositionSet, ExtraFloorTiles, FloorTileDecorChance);


        //WALLS
        //Get Walls
        WallPositions.UnionWith(GetAllRoomWalls(FloorPositionSet));
        

        //Remove Error Walls
        foreach (Vector2Int Pos in AdditionalFloorSpaces)
        {
            FloorPositionSet.Add(Pos);
            FloorTM.SetTile((Vector3Int)Pos, ChosenTileSet.DefaultFloor);
        }
        //Sort Walls
        foreach (Vector2Int Pos in WallPositions)
        {
            SortWallTile(Pos);
        }

        //Place Inner Corners
        DetermineInnerCornerLocations();
        PlaceAllWallTilesInList(InnerNWCorners, ChosenTileSet.WallInnerCornerNW, DecorationTileMaps[0]);
        PlaceAllWallTilesInList(InnerNECorners, ChosenTileSet.WallInnerCornerNE, DecorationTileMaps[1]);
        PlaceAllWallTilesInList(InnerSWCorners, ChosenTileSet.WallInnerCornerSW, DecorationTileMaps[2]);
        PlaceAllWallTilesInList(InnerSECorners, ChosenTileSet.WallInnerCornerSE, DecorationTileMaps[3]);


        //Place Wall Tops
        PlaceAllWallTilesInList(EastWallTopPositions, ChosenTileSet.WallTopE, WallTM);
        PlaceAllWallTilesInList(WestWallTopPositons, ChosenTileSet.WallTopW, WallTM);
        PlaceAllWallTilesInList(SouthWallTopPositions, ChosenTileSet.WallTopS, WallTM);
        PlaceWallTop(ChosenTileSet.WallTopN, BottomWallPositions);

        //Place Base Walls
        PlaceAllWallTilesInList(BottomLeftWallCorners, ChosenTileSet.WallBottomLeft, WallTM);
        PlaceAllWallTilesInList(BottomRightWallCorners, ChosenTileSet.WallBottomRight, WallTM);
        PlaceAllWallTilesInList(BottomWallPositions, ChosenTileSet.WallBottomMid, WallTM);


        //Place Outer Corners
        PlaceAllWallTilesInList(TopRightWallCorners, ChosenTileSet.WallOuterCornerNE, WallTM);
        PlaceAllWallTilesInList(TopLeftWallCorners, ChosenTileSet.WallOuterCornerNW, WallTM);
        PlaceWallTop(ChosenTileSet.WallOuterCornerSW, BottomLeftWallCorners);
        PlaceWallTop(ChosenTileSet.WallOuterCornerSE, BottomRightWallCorners);


        //Place Normal Wall Tops

        //Check for Isolated Walls/Corners
        foreach (Vector2Int position in WallPositions)
        {
            RuleTile Tile = PlaceIsolatedWalls(position);
            if (Tile != ChosenTileSet.WallEmpty)
            {
                WallTM.SetTile((Vector3Int)position, Tile);
            }
        }

        //FINAL FILL FOR EMPTY SPACES
        //Loop through total area, fill in any gaps with blank walls
        RectInt DungeonFillArea = DungeonCreationV2.Instance.FinalTotalDungeonSize;
        FinalFill(DungeonFillArea);

    }
    private void PlaceDecorativePatches(Tilemap TM, HashSet<Vector2Int> Positions, List<TileWithChance> TileList, float PercentOfTiles)
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

                TM.SetTile((Vector3Int)cur, SelectedTile);

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
    
    
    //PLACING WALLS
    //Gather All Walls from floors
    private HashSet<Vector2Int> GetAllRoomWalls(HashSet<Vector2Int> FloorPositions)
    {
        HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

        //Gather Walls
        int DirMultipleMax = DungeonCreationV2.Instance.GetRoomBuffer + 1;
        foreach (Vector2Int FloorPosition in FloorPositions)
        {
            foreach (Vector2Int direction in AllDirections)
            {
                for (int i = 1; i <= DirMultipleMax; i++)
                {
                    Vector2Int NewPos = FloorPosition + (direction * i);
                    if (!FloorPositionSet.Contains(NewPos))
                    {
                        //Check to See if Wall is Valid
                        if (FloorPositionSet.Contains(NewPos + Vector2Int.up) && FloorPositionSet.Contains(NewPos + Vector2Int.down))
                        {
                            AdditionalFloorSpaces.Add(NewPos);
                        }
                        else
                        {
                            WallPositions.Add(NewPos);
                        }
                    }
                }
            }
        }
        return walls;
    }

    //Walls Relative Direction Key
    private void SortWallTile(Vector2Int wallPos)
    {
        bool up = FloorPositionSet.Contains(wallPos + Vector2Int.up);
        bool down = FloorPositionSet.Contains(wallPos + Vector2Int.down);
        bool left = FloorPositionSet.Contains(wallPos + Vector2Int.left);
        bool right = FloorPositionSet.Contains(wallPos + Vector2Int.right);

        bool upLeft = FloorPositionSet.Contains(wallPos + new Vector2Int(-1, 1));
        bool upRight = FloorPositionSet.Contains(wallPos + new Vector2Int(1, 1));
        bool downLeft = FloorPositionSet.Contains(wallPos + new Vector2Int(-1, -1));
        bool downRight = FloorPositionSet.Contains(wallPos + new Vector2Int(1, -1));

        //Check Inner Corners
        if (!up && !left && !down && !right)//Must be Room Corner
        {
            if (!upLeft &&  !downLeft && !downRight && !upRight)
            {
                EmptyWallPositions.Add(wallPos); return;
            }
            if (downRight) { WestWallTopPositons.Add(wallPos); return; }
            if (downLeft) { EastWallTopPositions.Add(wallPos); return; }
        }

        //Check Corridor Corners
        if (up && left) { TopLeftWallCorners.Add(wallPos); return; }
        if (up && right) { TopRightWallCorners.Add(wallPos); return; }
        if (down && left) { BottomLeftWallCorners.Add(wallPos); return; }    //Requires additional logic so stores position
        if (down && right) { BottomRightWallCorners.Add(wallPos); return; } //Requires additional logic so stores position

        //Check Side Walls
        if (down) { BottomWallPositions.Add(wallPos); return; }
        if (up) { SouthWallTopPositions.Add(wallPos); return; }
        if (left) { EastWallTopPositions.Add(wallPos); return; }   //Stores position for later safety check
        if (right) { WestWallTopPositons.Add(wallPos); return; }  //Stores position for later safety check

    }
    private void PlaceAllWallTilesInList(HashSet<Vector2Int> List, RuleTile tile, Tilemap map)
    {
        foreach (Vector2Int wallPos in List)
        {
            map.SetTile((Vector3Int)wallPos, tile);
        }
    }

    private void DetermineInnerCornerLocations()
    {
        //South Wall
        //Take All Locations Along Wall -> Check upLeft and upRight -> If not Floor, Needs Inner Wall
        HashSet<Vector2Int> AllSouthWallPositions = new HashSet<Vector2Int>();
        AllSouthWallPositions.UnionWith(SouthWallTopPositions);
        AllSouthWallPositions.UnionWith(TopRightWallCorners);
        AllSouthWallPositions.UnionWith(TopLeftWallCorners);
        foreach (Vector2Int Position in AllSouthWallPositions)
        {
            Vector2Int left = Position + Vector2Int.left;
            Vector2Int right = Position + Vector2Int.right;
            if (!FloorPositionSet.Contains(left + Vector2Int.up))
            {
                InnerSWCorners.Add(left);
            }
            if (!FloorPositionSet.Contains(right + Vector2Int.up))
            {
                InnerSECorners.Add(right);
            }
        }

        //Bottom Wall
        HashSet<Vector2Int> AllBottomWallPositions = new HashSet<Vector2Int>();
        AllBottomWallPositions.UnionWith(BottomWallPositions);
        AllBottomWallPositions.UnionWith(BottomRightWallCorners);
        AllBottomWallPositions.UnionWith(BottomLeftWallCorners);
        foreach (Vector2Int Position in AllBottomWallPositions)
        {
            Vector2Int left = Position + Vector2Int.left;
            Vector2Int right = Position + Vector2Int.right;
            if (!FloorPositionSet.Contains(left + Vector2Int.down))
            {
                InnerNWCorners.Add(left + Vector2Int.up);
            }
            if (!FloorPositionSet.Contains(right + Vector2Int.down))
            {
                InnerNECorners.Add(right + Vector2Int.up);
            }
        }
    }
    //Additional Tile Placement Functions
    private void PlaceWallTop(RuleTile Tile, HashSet<Vector2Int> PossiblePositions)
    {
        //Loop Through all Given Positions
        foreach (Vector2Int position in PossiblePositions)
        {
            //Check Adjacent Tile to See if it Can be replaced with given tile
            Vector3Int NewPosition = (Vector3Int)position + Vector3Int.up;
            //Replace with given tile
            WallTM.SetTile(NewPosition, Tile);
        }
    }

    private RuleTile PlaceIsolatedWalls(Vector2Int Position)
    {
        HashSet<Vector2Int> AllBottomWallPositions = new HashSet<Vector2Int>();
        AllBottomWallPositions.UnionWith(BottomWallPositions);
        AllBottomWallPositions.UnionWith(BottomRightWallCorners);
        AllBottomWallPositions.UnionWith(BottomLeftWallCorners);

        bool FloorAbove = FloorPositionSet.Contains(Position + Vector2Int.up);
        bool FloorBelow = FloorPositionSet.Contains(Position + Vector2Int.down);
        bool FloorLeft = FloorPositionSet.Contains(Position + Vector2Int.left);
        bool FloorRight = FloorPositionSet.Contains(Position + Vector2Int.right);
        bool aboveWallBottom = AllBottomWallPositions.Contains(Position + Vector2Int.down);
        bool wallBotLeftorRight = AllBottomWallPositions.Contains(Position + Vector2Int.left) || AllBottomWallPositions.Contains(Position + Vector2Int.right);
        bool floorLeftOrRight = FloorLeft || FloorRight;

        //Horizontal Thin Wall
        if (aboveWallBottom && FloorAbove)
        {
            //Left Corner
            if (FloorLeft && !FloorRight) { return ChosenTileSet.WallNarrowW; }
            if (!FloorLeft && FloorRight) { return ChosenTileSet.WallNarrowE; }
            if (!FloorLeft && !FloorRight) { return ChosenTileSet.WallNarrowEW; }
        }

        //Vertical Thin Wall
        if (!aboveWallBottom && !FloorAbove && FloorLeft && FloorRight && FloorBelow) { return ChosenTileSet.WallNarrowBase; }
        if (aboveWallBottom && FloorLeft && FloorRight && !FloorAbove && !FloorBelow) { return ChosenTileSet.WallTopNarrowS; }
        if (!aboveWallBottom && FloorLeft && FloorRight && FloorAbove && !FloorBelow) { return ChosenTileSet.WallTopNarrowN; }
        if (!aboveWallBottom && FloorLeft && FloorRight && !FloorAbove && !FloorBelow) { return ChosenTileSet.WallTopNarrowNS; }
        if (!aboveWallBottom && wallBotLeftorRight && !FloorAbove && !FloorBelow && floorLeftOrRight) { return ChosenTileSet.WallTopNarrowNS; }

        return ChosenTileSet.WallEmpty;
    }

    private void FinalFill(RectInt Area)
    {
        for (int x = Area.xMin; x <= Area.xMax; x++)
        {
            for (int y = Area.yMin; y <= Area.yMax; y++)
            {
                Vector3Int Position = new Vector3Int(x, y);
                if (WallTM.GetTile(Position) == null)
                {
                    WallTM.SetTile(Position, ChosenTileSet.WallEmpty);
                }
            }
        }
    }
}
