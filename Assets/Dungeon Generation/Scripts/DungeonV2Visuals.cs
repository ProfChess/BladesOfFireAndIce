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

    [Header("Walls")]
    [SerializeField] private RuleTile wallTileDefault;
    [SerializeField] private RuleTile wallTileEmpty;
    [SerializeField] private RuleTile wallTileTopMiddle;
    [SerializeField] private RuleTile wallTileTopMiddleEast;
    [SerializeField] private RuleTile wallTileTopMiddleWest;
    [SerializeField] private RuleTile wallTileTopMiddleSouth;
    [SerializeField] private RuleTile wallTileBottomMiddle;
    [SerializeField] private RuleTile wallTileBottomEast;
    [SerializeField] private RuleTile wallTileBottomWest;
    private HashSet<Vector2Int> WallPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> PositionsToRemove = new HashSet<Vector2Int>(); //Error Position List
    private HashSet<Vector2Int> EmptyWallPositions = new HashSet<Vector2Int>();
    
    private HashSet<Vector2Int> BottomWallPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> EastWallTopPositions = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> WestWallTopPositons = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> SouthWallTopPositions = new HashSet<Vector2Int>();

    [Header("Narrow Walls")]
    [SerializeField] private RuleTile wallTileIsolatedBot;
    [SerializeField] private RuleTile wallTileNarrowT;
    [SerializeField] private RuleTile wallTileNarrowB;
    [SerializeField] private RuleTile wallTileNarrowTB;
    [SerializeField] private RuleTile wallTileNarrowL;
    [SerializeField] private RuleTile wallTileNarrowR;
    [SerializeField] private RuleTile wallTileNarrowLR;


    [Header("Wall Corners")]
    [SerializeField] private RuleTile wallTileTopRightCorner;
    [SerializeField] private RuleTile wallTileTopLeftCorner;
    [SerializeField] private RuleTile wallTileTopBottomRightCorner;
    [SerializeField] private RuleTile wallTileTopBottomLeftCorner;
    private HashSet<Vector2Int> BottomLeftWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> BottomRightWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> TopRightWallCorners = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> TopLeftWallCorners = new HashSet<Vector2Int>();

    [Header("Wall Inner Corners")]
    [SerializeField] private RuleTile wallTileInnerCornerNW;
    [SerializeField] private RuleTile wallTileInnerCornerNE;
    [SerializeField] private RuleTile wallTileInnerCornerSW;
    [SerializeField] private RuleTile wallTileInnerCornerSE;

    private HashSet<Vector2Int> SideWallsForReplacement = new HashSet<Vector2Int>();
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
            FloorTM.SetTile((Vector3Int)Position, floorTileDefault);
        }
        //Place Decorated Floor Tiles
        PlaceDecorativePatches(FloorTM, FloorPositionSet, ExtraFloorTiles, FloorTileDecorChance);


        //WALLS
        //Get Walls
        int DirMultipleMax = DungeonCreationV2.Instance.GetRoomBuffer;
        foreach (Vector2Int Position in FloorPositionSet)
        {
            foreach (Vector2Int Dir in AllDirections)
            {
                for (int i = 1; i <= DirMultipleMax; i++)
                {
                    Vector2Int NewPos = Position + (Dir * i);
                    if (!FloorPositionSet.Contains(NewPos) && !WallPositions.Contains(NewPos))
                    {
                        //Check to See if Wall is Valid
                        if (FloorPositionSet.Contains(NewPos + Vector2Int.up) && FloorPositionSet.Contains(NewPos + Vector2Int.down))
                        {
                            PositionsToRemove.Add(NewPos);
                        }
                        else
                        {
                            WallPositions.Add(NewPos);
                        }
                    }
                }
            }
        }
        

        //Remove Error Walls
        foreach (Vector2Int Pos in PositionsToRemove)
        {
            WallTM.SetTile((Vector3Int)Pos, null);
            WallPositions.Remove(Pos);
            FloorPositionSet.Add(Pos);
            FloorTM.SetTile((Vector3Int)Pos, floorTileDefault);
        }
        //Sort Walls
        foreach (Vector2Int Pos in WallPositions)
        {
            SortWallTile(Pos);
        }

        //Place Walls
        PlaceAllWallTilesInList(EastWallTopPositions, wallTileTopMiddleEast);
        PlaceAllWallTilesInList(WestWallTopPositons, wallTileTopMiddleWest);
        PlaceAllWallTilesInList(SouthWallTopPositions, wallTileTopMiddle);
        PlaceAllWallTilesInList(BottomWallPositions, wallTileBottomMiddle);

        //Place Corners
        PlaceAllWallTilesInList(TopRightWallCorners, wallTileTopRightCorner);
        PlaceAllWallTilesInList(TopLeftWallCorners, wallTileTopLeftCorner);
        PlaceAllWallTilesInList(BottomLeftWallCorners, wallTileBottomWest);
        PlaceAllWallTilesInList(BottomRightWallCorners, wallTileBottomEast);

        //Move Inner Corners and Place New Walls
        PlaceAllWallTilesInList(InnerNWCorners, wallTileInnerCornerNW);
        PlaceAllWallTilesInList(InnerNECorners, wallTileInnerCornerNE);
        PlaceAllWallTilesInList(InnerSWCorners, wallTileInnerCornerSW);
        PlaceAllWallTilesInList(InnerSECorners, wallTileInnerCornerSE);

        //Place Tops of Walls on Corners
        PlaceWallTop(wallTileTopBottomLeftCorner, BottomLeftWallCorners, SideWallsForReplacement);
        PlaceWallTop(wallTileTopBottomRightCorner, BottomRightWallCorners, SideWallsForReplacement);

        //Place Normal Wall Tops
        PlaceWallTop(wallTileTopMiddleSouth, BottomWallPositions, EmptyWallPositions);

        //Check for Isolated Walls/Corners
        foreach (Vector2Int position in WallPositions)
        {
            RuleTile Tile = PlaceIsolatedWalls(position);
            if (Tile != wallTileEmpty)
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
        foreach (Vector2Int FloorPosition in FloorPositions)
        {
            foreach (var direction in AllDirections)
            {
                Vector2Int WallPos = FloorPosition + direction;
                if (!FloorPositions.Contains(WallPos))
                {
                    walls.Add(WallPos);
                }
            }
        }
        return walls;
    }
    
    //Gather all positions for floor from given room
    private HashSet<Vector2Int> GetRoomFloorPositions(SingleDungeonRoom room)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int x = room.Area.xMin; x <= room.Area.xMax; x++)
        {
            for (int y = room.Area.yMin;  y <= room.Area.yMax; y++)
            {
                floor.Add(new Vector2Int(x, y));
            }
        }
        return floor;
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

        //Check Room Corners
        if (!up && !left && !down && !right)//Must be Room Corner
        {
            //South East Corner
            if (upLeft) { InnerSECorners.Add(wallPos); return; }
            if (upRight) { InnerSWCorners.Add(wallPos); return; }
            if (downLeft) { InnerNECorners.Add(wallPos + Vector2Int.up); SideWallsForReplacement.Add(wallPos); EastWallTopPositions.Add(wallPos); return; }  //Adds Position above for corner, returns side
            if (downRight) { InnerNWCorners.Add(wallPos + Vector2Int.up); SideWallsForReplacement.Add(wallPos); WestWallTopPositons.Add(wallPos); return; } //Adds Position above for corner, returns side
            EmptyWallPositions.Add(wallPos); return;
        }

        //Check Corridor Corners
        if (up && left) { TopLeftWallCorners.Add(wallPos); return; }
        if (up && right) { TopRightWallCorners.Add(wallPos); return; }
        if (down && left) { BottomLeftWallCorners.Add(wallPos); return; }    //Requires additional logic so stores position
        if (down && right) { BottomRightWallCorners.Add(wallPos); return; } //Requires additional logic so stores position

        //Check Side Walls
        if (down) { BottomWallPositions.Add(wallPos); return; }
        if (up) { SouthWallTopPositions.Add(wallPos); return; }
        if (left) { EastWallTopPositions.Add(wallPos); SideWallsForReplacement.Add(wallPos); return; }   //Stores position for later safety check
        if (right) { WestWallTopPositons.Add(wallPos); SideWallsForReplacement.Add(wallPos); return; }  //Stores position for later safety check

    }
    private void PlaceAllWallTilesInList(HashSet<Vector2Int> List, RuleTile tile)
    {
        foreach (Vector2Int wallPos in List)
        {
            WallTM.SetTile((Vector3Int)wallPos, tile);
        }
    }


    //Additional Tile Placement Functions
    private void PlaceWallTop(RuleTile Tile, HashSet<Vector2Int> PossiblePositions, HashSet<Vector2Int> SafeTiles)
    {
        //Loop Through all Given Positions
        foreach (Vector2Int position in PossiblePositions)
        {
            //Check Adjacent Tile to See if it Can be replaced with given tile
            Vector3Int NewPosition = (Vector3Int)position + Vector3Int.up;
            if (SafeTiles.Contains((Vector2Int)NewPosition))
            {
                //Replace with given tile
                WallTM.SetTile(NewPosition, Tile);
            }
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
        
        //Horizontal Thin Wall
        if (aboveWallBottom && FloorAbove)
        {
            //Left Corner
            if (FloorLeft && !FloorRight) { return wallTileNarrowL; }
            if (!FloorLeft && FloorRight) { return wallTileNarrowR; }
            if (!FloorLeft && !FloorRight) { return wallTileNarrowLR; }
        }

        //Vertical Thin Wall
        if (!aboveWallBottom && !FloorAbove && FloorLeft && FloorRight && FloorBelow) { return  wallTileIsolatedBot; }
        if (aboveWallBottom && FloorLeft && FloorRight && !FloorAbove && !FloorBelow) { return wallTileNarrowB; }
        if (!aboveWallBottom && FloorLeft && FloorRight && FloorAbove && !FloorBelow) { return wallTileNarrowT; }
        if (!aboveWallBottom && FloorLeft && FloorRight && !FloorAbove && !FloorBelow) { return wallTileNarrowTB; }

        return wallTileEmpty;
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
                    WallTM.SetTile(Position, wallTileEmpty);
                }
            }
        }
    }
}
