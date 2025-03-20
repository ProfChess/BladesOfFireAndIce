using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonVisuals : MonoBehaviour
{
    //Stats
    [Header("Chance For Each Decoration")]
    [SerializeField] private float FloorDecorChance;
    [SerializeField] private float FloorGrateChance;
    [SerializeField] private float WallHangingChance;
    //Decoration Heirarchy Objects
    [SerializeField] private GameObject GrateObject;
    [SerializeField] private GameObject WallObject;

    //Variables
    [Header("Dungeon")]
    [SerializeField] private DungeonGenerator DG;
    [Header("Tiles")]
    [SerializeField] private Tilemap FloorTM;
    [SerializeField] private Tilemap WallTM;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTile;

    //Decor
    [Header("Decorations")]
    [SerializeField] private Tilemap DecorTM;
    [SerializeField] private List<RuleTile> DecorationTiles = new List<RuleTile>();
    [SerializeField] private List<RuleTile> CandleList = new List<RuleTile>();
    [SerializeField] private List<RuleTile> PotList = new List<RuleTile>();

    //Decoration Struct
    [Serializable]
    public struct DecorationObject { public GameObject obj; public float spawnChance; }
    //Grates
    [SerializeField] private List<DecorationObject> GrateList = new List<DecorationObject>();
    //Hanging Objects
    [SerializeField] private List<DecorationObject> WallObjectList = new List<DecorationObject>();

    //Private Lists
    private List<BoundsInt> roomList = new List<BoundsInt>();
    private List<Tuple<BoundsInt, BoundsInt>> corridorList = new List<Tuple<BoundsInt, BoundsInt>>();

    private HashSet<Vector3Int> FloorPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> WallPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> DecorationFloorPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> DecorationRoomCenters = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> WallHangingPositions = new HashSet<Vector3Int>();

    Vector3Int[] TileAdjacentDirecitons =
    {
        //Regular Spots
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1, 0),

        //Additional Spaces
        new Vector3Int(2, 0, 0),
        new Vector3Int(-2, 0, 0),
        new Vector3Int(0, 2, 0),
        new Vector3Int(0, -2, 0),
        new Vector3Int(2, 2, 0),
        new Vector3Int(-2, 2, 0),
        new Vector3Int(2, -2, 0),
        new Vector3Int(-2, -2, 0),

        new Vector3Int(3, 0, 0),
        new Vector3Int(-3, 0, 0),
        new Vector3Int(0, 3, 0),
        new Vector3Int(0, -3, 0),
        new Vector3Int(3, 3, 0),
        new Vector3Int(-3, 3, 0),
        new Vector3Int(3, -3, 0),
        new Vector3Int(-3, -3, 0),
    };
    Vector3Int[] WallAdjacentDirections =
    {
        //Regular Spots
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1, 0),
    };

    //Events
    public event Action DungeonComplete;

    // Start is called before the first frame update
    void Start()
    {
        //Clear Rooms
        roomList.Clear();
        corridorList.Clear();

        //Get Rooms from Dungeon Gen
        roomList = DG.getRoomList();
        corridorList = DG.getCorridorList();

        getRoomCenters();
        gatherTiles();
        placeTiles();

        DungeonComplete?.Invoke();
    }

    private void getRoomCenters()
    {
        foreach (BoundsInt room in roomList)
        {
            DecorationRoomCenters.Add(Vector3Int.RoundToInt(room.center));
        }
    }
    private void gatherTiles()
    {
        //Adds Positions for Tiles for rooms in roomlist
        foreach (BoundsInt room in roomList)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    FloorPositions.Add(new Vector3Int(x, y, 0));
                    DecorationFloorPositions.Add(new Vector3Int(x, y, 0));
                }
            }
        }

        //Adds Corridors to same Hashset 
        for (int i = 0; i < corridorList.Count; i++)
        {
            BoundsInt xCor = corridorList[i].Item1;
            BoundsInt yCor = corridorList[i].Item2;

            //Collects Corridor for horizontal component
            if (xCor.size.x > 0 && xCor.size.y > 0) //Checks the corridor is not 0
            {
                for (int x = xCor.xMin; x < xCor.xMax; x++) //Repeats for each x in the corridor
                {
                    for (int y = xCor.yMin; y < xCor.yMax; y++) //Repeats for each y in the corridor
                    {
                        FloorPositions.Add(new Vector3Int(x, y, 0));
                    }
                }
            }
            //Collects Corridor for vertical component
            if (yCor.size.x > 0 && yCor.size.y > 0)
            {
                for (int x = yCor.xMin; x < yCor.xMax; x++)
                {
                    for (int y = yCor.yMin; y < yCor.yMax; y++)
                    {
                        FloorPositions.Add(new Vector3Int(x, y, 0));
                    }
                }
            }

        }

    }

    private void placeTiles()
    {
        foreach (Vector3Int tilePosition in FloorPositions)
        {
            //Place Floor Tiles
            FloorTM.SetTile(tilePosition, floorTile);

            //Place Random Decor on Floor
            if (PlaceTileChance(FloorDecorChance)) { SelectRandomTile(tilePosition); }

            foreach (Vector3Int Direction in TileAdjacentDirecitons)
            {
                Vector3Int AdjacentPos = tilePosition + Direction;
                if (!FloorPositions.Contains(AdjacentPos))
                {
                    WallPositions.Add(AdjacentPos);
                }
            }
        }
        foreach (Vector3Int x in WallPositions) //Checks each wall tiles adjacent position adds to wall decor list if no surrounding tiles are floor
        {
            int count = 0;
            foreach (Vector3Int y in WallAdjacentDirections)
            {
                Vector3Int Pos = x + y;
                if (FloorPositions.Contains(Pos)) { count += 1; }
            }
            if (count == 0)
            {
                if (x.y >= DG.Space.yMin - 1 && x.y <= DG.Space.yMax + 1 
                    && x.x >= DG.Space.xMin && x.x <= DG.Space.xMax + 1)
                {
                    bool nearDecor = false;
                    foreach (Vector3Int Dir in WallAdjacentDirections)
                    {
                        Vector3Int AdjacentDir = x + Dir;
                        if (WallHangingPositions.Contains(AdjacentDir))
                        {
                            nearDecor = true;
                        }
                    }
                    if (!nearDecor)
                    {
                        WallHangingPositions.Add(x);
                    }
                }
            }
        }
        foreach (Vector3Int WallPos in WallPositions){ WallTM.SetTile(WallPos, wallTile); }

        //Decorations
        //Place Grate in Center of room
        foreach (Vector3Int FloorCenter in DecorationRoomCenters) { if (PlaceTileChance(FloorGrateChance)) { PlaceDecorList(FloorCenter, GrateList, GrateObject); }}

        //Place Hanging Decor on Walls
        foreach (Vector3Int wallPos in WallHangingPositions) { if (PlaceTileChance(WallHangingChance)) { PlaceDecorList(wallPos, WallObjectList, WallObject); }}
    }

    private bool PlaceTileChance(float chance)
    {
        float x = UnityEngine.Random.Range(0f, 1f);
        if (chance <= x) { return false; }
        else { return true; }
    }
    private void PlaceDecorList(Vector3Int Center, List<DecorationObject> List, GameObject Parent)
    {
        //Total Chance
        float totalChance = 0;
        foreach (DecorationObject Obj in List) {totalChance += Obj.spawnChance;}

        float rand = UnityEngine.Random.Range(0f, totalChance);
        float curSum = 0f;

        foreach (DecorationObject Obj in List)
        {
            curSum += Obj.spawnChance;
            if (rand <= curSum)
            {
                GameObject x = Instantiate(Obj.obj, Center, Quaternion.identity);
                x.transform.parent = Parent.transform;
                return;
            }
        }
    }
    private void SelectRandomTile(Vector3Int spot)
    {
        int x = UnityEngine.Random.Range(0, DecorationTiles.Count);
        RuleTile newTile = null;
        switch (x)
        {
            //Candle
            case 0:
                int CandleNum = UnityEngine.Random.Range(0, CandleList.Count);
                newTile = CandleList[CandleNum];
                break;

            //Pot
            case 1:
                int PotNum = UnityEngine.Random.Range(0, PotList.Count);
                newTile = PotList[PotNum];
                break;

            default:
                break;
        }
        DecorTM.SetTile(spot, newTile);
    }
}
