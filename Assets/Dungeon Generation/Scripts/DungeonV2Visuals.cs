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
    [SerializeField] private RuleTile wallTileBottomMiddle;
    [SerializeField] private RuleTile wallTileBottomEast;
    [SerializeField] private RuleTile wallTileBottomWest;

    [Header("Wall Corners")]
    [SerializeField] private RuleTile wallTileTopRightCorner;
    [SerializeField] private RuleTile wallTileTopLeftCorner;
    [SerializeField] private RuleTile wallTileTopBottomRightCorner;
    [SerializeField] private RuleTile wallTileTopBottomLeftCorner;
    [Header("Wall Inner Corners")]
    [SerializeField] private RuleTile wallTileInnerCornerNW;
    [SerializeField] private RuleTile wallTileInnerCornerNE;
    [SerializeField] private RuleTile wallTileInnerCornerSW;
    [SerializeField] private RuleTile wallTileInnerCornerSE;


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

        //Place Walls
        HashSet<Vector2Int> WallPositions = new HashSet<Vector2Int>();
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
                        WallPositions.Add(NewPos);
                    }
                }
            }
        }
        foreach (Vector2Int Pos in WallPositions)
        {
            WallTM.SetTile((Vector3Int)Pos, wallTileDefault);
        }
        //Place Decorated Wall Tiles
        PlaceDecorativePatches(WallTM, WallPositions, ExtraWallTiles, WallTileDecorChance);

        HashSet<Vector2Int> AllWallPositions = GetAllRoomWalls(FloorPositionSet);
        foreach (Vector2Int Pos in WallPositions)
        {
            RoomWallSide WallSide = DetermineWallType(Pos);
            WallTM.SetTile((Vector3Int)Pos, GetWallTile(WallSide));
        }

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
    public enum RoomWallSide 
    { 
        North, South, East, West, SEBot, SWBot, 
        CornerNW, CornerNE, CornerSW, CornerSE, 
        WallDefault, WallDefaultEmpty, InnerCornerNW, InnerCornerNE, 
        InnerCornerSW, InnerCornerSE
    }
    public RoomWallSide DetermineWallType(Vector2Int wallPos)
    {
        bool up    = FloorPositionSet.Contains(wallPos + Vector2Int.up);
        bool down  = FloorPositionSet.Contains(wallPos + Vector2Int.down);
        bool left  = FloorPositionSet.Contains(wallPos + Vector2Int.left);
        bool right = FloorPositionSet.Contains(wallPos + Vector2Int.right);

        bool upLeft = FloorPositionSet.Contains(wallPos + new Vector2Int(-1, 1));
        bool upRight = FloorPositionSet.Contains(wallPos + new Vector2Int(1, 1));
        bool downLeft = FloorPositionSet.Contains(wallPos + new Vector2Int(-1, -1));
        bool downRight = FloorPositionSet.Contains(wallPos + new Vector2Int(1, -1));

        //Check Room Corners
        if (!up && !left && !down && !right)//Must be Room Corner
        {
            //South East Corner
            if (upLeft) { return RoomWallSide.InnerCornerSE; }
            if (upRight) { return RoomWallSide.InnerCornerSW; }
            if (downLeft) { return RoomWallSide.InnerCornerNE; }
            if (downRight) { return RoomWallSide.InnerCornerNW; }
            return RoomWallSide.WallDefaultEmpty;
        }

        //Check Corridor Corners
        if (up && left && upLeft) { return RoomWallSide.CornerNW; }
        if (up && right && upRight) {  return RoomWallSide.CornerNE; }
        if (down && left && downLeft) { return RoomWallSide.SWBot; } 
        if (down && right && downRight) { return RoomWallSide.SEBot; }

        //Check Side Walls
        if (down) { return RoomWallSide.North; }
        if (up) { return RoomWallSide.South; }
        if (left) { return RoomWallSide.East; }
        if (right) { return RoomWallSide.West; }

        //Fallback
        return RoomWallSide.WallDefault;
    }
    private RuleTile GetWallTile(RoomWallSide WallType)
    {
        switch(WallType)
        {
            default: return wallTileDefault;
            case RoomWallSide.WallDefaultEmpty:
                return wallTileEmpty;
            case RoomWallSide.North:
                return wallTileBottomMiddle;
            case RoomWallSide.East:
                return wallTileTopMiddleEast;
            case RoomWallSide.West:
                return wallTileTopMiddleWest;
            case RoomWallSide.South:
                return wallTileTopMiddle;
            case RoomWallSide.SEBot:
                return wallTileBottomEast;
            case RoomWallSide.SWBot:
                return wallTileBottomWest;

            case RoomWallSide.CornerNW:
                return wallTileTopLeftCorner;
            case RoomWallSide.CornerNE:
                return wallTileTopRightCorner;
            case RoomWallSide.CornerSW:
                return wallTileTopBottomLeftCorner;
            case RoomWallSide.CornerSE:
                return wallTileTopBottomRightCorner;

            case RoomWallSide.InnerCornerNW:
                return wallTileInnerCornerNW;
            case RoomWallSide.InnerCornerNE:
                return wallTileInnerCornerNE;
            case RoomWallSide.InnerCornerSW:
                return wallTileInnerCornerSW;
            case RoomWallSide.InnerCornerSE:
                return wallTileInnerCornerSE;
        }
    }

}
