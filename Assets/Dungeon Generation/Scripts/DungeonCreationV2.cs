using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DungeonCreationV2 : MonoBehaviour
{
    //Custom Data
    //Struct for holding room appearance chances (Might move to Scriptable object with placment details later)
    [System.Serializable]
    public struct SpecialRoomTypeChance
    {
        public SpecialRoomKind Type;
        [Range(0f, 1f)] public float Chance;
    }

    //Instance
    public static DungeonCreationV2 Instance;

    //Specific Stats
    [Header("Dungeon Size")]
    public RectInt StartingTotalDungeonSize = new RectInt(0, 0, 65, 65);
    public RectInt FinalTotalDungeonSize => GetMaxDungeonBounds();
    [SerializeField, Range(0f, 1f)] float BaseSpecialRoomSpawnChance = 0.25f;
    [SerializeField] private int SetMinRoomLength = 8;
    [SerializeField] private int SetMinRoomHeight = 8;
    private int MinRoomLength = 0;
    private int MinRoomHeight = 0;
    [SerializeField] private int RoomBuffer = 2;
    [SerializeField] private int MinOffset = 4;
    [SerializeField] private int MaxOffset = 10;

    [Header("Dungeon Changes")]
    [SerializeField] int MaxSpecialRooms = 0;
    private List<SpecialRoomTypeChance> SpecialRoomChances = new List<SpecialRoomTypeChance>
    {
        new SpecialRoomTypeChance{Type = SpecialRoomKind.Chest, Chance = 0.4f },
        new SpecialRoomTypeChance{Type = SpecialRoomKind.Rest, Chance = 0.15f },
        new SpecialRoomTypeChance{Type = SpecialRoomKind.Buff, Chance = 0.28f },
        new SpecialRoomTypeChance{Type = SpecialRoomKind.Puzzle, Chance = 0.35f },

    };
    private enum SplitAxis { Horizontal, Vertical, Invalid }

    //Rooms 
    private SingleDungeonRoom StartingRoom;
    private List<SingleDungeonRoom> BasicDungeonRooms = new List<SingleDungeonRoom>();
    private List<(RectInt, RectInt)> DungeonCorridors = new List<(RectInt, RectInt)>();

    //Positions
    private HashSet<Vector2Int> TilePlacePositions = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> GetTilePlaces => TilePlacePositions;
    public List<SingleDungeonRoom> GetDungeonRooms => BasicDungeonRooms;
    public int GetRoomBuffer => RoomBuffer;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }

        //RoomOffset = RoomBuffer / 2;
        MinRoomHeight = SetMinRoomHeight;
        MinRoomLength = SetMinRoomLength; 
        StartingRoom = new SingleDungeonRoom(StartingTotalDungeonSize);
        SplitSpace(StartingRoom);

        //Separate Rooms Based on Room Buffer
        SeparateAllDungeonRooms();

        //INSERT RANDOM REPLACING OF SPECIAL ROOMS
        InsertSpecialRooms();

        //CONNECT ROOMS WITH CORRIDORS
        CreateCorridorConnections();

        //ADD ALL SPACE/POSITIONS TO ADDITIONAL LIST FOR TILE PLACEMENT
        CollectPositions();

        //for (int i = 0; i < BasicDungeonRooms.Count; i++)
        //{
        //    Debug.Log("Room #" + i + " Width: " + BasicDungeonRooms[i].Area.width + " Height: " + BasicDungeonRooms[i].Area.height);
        //}
    }
    private void Start()
    {
        DungeonDataCreated?.Invoke();
    }
    private void SplitSpace(SingleDungeonRoom Room)
    {
        //Decide Split Based on Dimentions
        bool longEnough = false;
        bool tallEnough = false;
        if (Room.Area.width >= MinRoomLength * 2) { longEnough = true; }
        if (Room.Area.height >= MinRoomHeight * 2) { tallEnough = true; }

        //Choose Axis
        SplitAxis AxisToSplit = SplitAxis.Invalid;
        if (longEnough && tallEnough)
        {
            float AxisSelection = Random.Range(0.0f, 1.0f);
            AxisToSplit = AxisSelection <= 0.5f ? SplitAxis.Horizontal : SplitAxis.Vertical;
        }
        else if (longEnough && !tallEnough)
        {
            AxisToSplit = SplitAxis.Vertical;
        }
        else if (!longEnough && tallEnough)
        {
            AxisToSplit = SplitAxis.Horizontal;
        }
        else
        {
            AddRoomToList(Room);
            return;
        }

        //Split Accross Axis
        CutRoomInHalf(Room, AxisToSplit);
        SplitSpace(Room.FirstChildRoom);
        SplitSpace(Room.SecondChildRoom);
    }
    private void CutRoomInHalf(SingleDungeonRoom Room, SplitAxis Axis)
    {
        //Separate Behaviour from Axis
        switch (Axis)
        {
            case SplitAxis.Horizontal:
                int minYSplitPoint = Room.Area.yMin + MinRoomHeight;
                int maxYSplitPoint = Room.Area.yMax - MinRoomHeight;

                //Pick Random Split
                int YSplit = Room.Area.yMin + Room.Area.height / 2;
                if (Room.Area.height != MinRoomHeight * 2)
                {
                    YSplit = Random.Range(minYSplitPoint, maxYSplitPoint + 1);
                }


                //Create Top and Bottom Rooms
                RectInt TopRoomRect = new RectInt(Room.Area.xMin, Room.Area.yMin, Room.Area.width, YSplit - Room.Area.yMin);
                RectInt BotRoomRect = new RectInt(Room.Area.xMin, YSplit, Room.Area.width, Room.Area.yMax - YSplit);
                SingleDungeonRoom TopRoom = new SingleDungeonRoom(TopRoomRect);
                SingleDungeonRoom BottomRoom = new SingleDungeonRoom(BotRoomRect);
                
                //Assigning Children
                Room.AssignChildren(TopRoom, BottomRoom);
                break;
            
            case SplitAxis.Vertical:
                int minXSplitPoint = Room.Area.xMin + MinRoomLength;
                int maxXSplitPoint = Room.Area.xMax - MinRoomLength;

                //Pick Random Point
                int XSplit = Room.Area.xMin + Room.Area.width / 2;
                if (Room.Area.width != MinRoomLength * 2)
                {
                    XSplit = Random.Range(minXSplitPoint, maxXSplitPoint + 1);
                }

                //Create Left and Right
                RectInt LeftRoomRect = new RectInt(Room.Area.xMin, Room.Area.yMin, XSplit - Room.Area.xMin, Room.Area.height);
                RectInt RightRoomRect = new RectInt(XSplit, Room.Area.yMin, Room.Area.xMax - XSplit, Room.Area.height);
                SingleDungeonRoom LeftRoom = new SingleDungeonRoom(LeftRoomRect);
                SingleDungeonRoom RightRoom = new SingleDungeonRoom(RightRoomRect);
                
                //Assigning Children
                Room.AssignChildren(LeftRoom, RightRoom);
                break;
            
            default:
                Debug.Log("Undefined Axis Given");
                break;
        }
    }

    private void AddRoomToList(SingleDungeonRoom Room)
    {
        BasicDungeonRooms.Add(Room);
    }
    //Move Rooms Apart 
    private void SeparateAllDungeonRooms()
    {
        Vector2Int CenterOfDungeon = new Vector2Int(StartingTotalDungeonSize.xMin + (StartingTotalDungeonSize.width / 2), StartingTotalDungeonSize.yMin + (StartingTotalDungeonSize.height / 2));
        bool movedRoom = true;

        while (movedRoom)
        {
            movedRoom = false;
            for (int i = 0; i < BasicDungeonRooms.Count; i++)
            {
                for (int j =  i + 1; j < BasicDungeonRooms.Count; j++)
                {
                    if (MoveRoomsApart(ref BasicDungeonRooms[i].Area, ref BasicDungeonRooms[j].Area, RoomBuffer * 2))
                    {
                        movedRoom = true;
                    }
                }
            }
        }

    }
    private Vector2Int OverlapNum(RectInt a, RectInt b)
    {
        int overlapX = Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin);
        int overlapY = Mathf.Min(a.yMax, b.yMax) - Mathf.Max(a.yMin, b.yMin);
        return new Vector2Int(overlapX, overlapY);
    }
    private bool MoveRoomsApart(ref RectInt a, ref RectInt b, int buffer)
    {
        //Check if Rooms Overlap -> if They Don't, No Need to Move
        RectInt BuffedA = new RectInt(a.x + buffer, a.y + buffer, a.width + buffer * 2, a.height + buffer * 2);
        RectInt BuffedB = new RectInt(b.x + buffer, b.y + buffer, b.width + buffer * 2, b.height + buffer * 2);

        if (!BuffedA.Overlaps(BuffedB)) return false;

        //Get Amount They Overlap
        Vector2Int OverlapAmount = OverlapNum(BuffedA, BuffedB);

        //Determine How to Separate -> Move Apart Based on Which Axis Overlaps Less
        bool moveByX = OverlapAmount.x < OverlapAmount.y;

        //Move Along X Plane
        if (moveByX)
        {
            //Move Room With Larger X 
            if (a.x < b.x) { b.x += OverlapAmount.x + RoomBuffer; OffsetRoom(ref b, false); }
            else { a.x += OverlapAmount.x + RoomBuffer; OffsetRoom(ref a, false); }
        }
        //Move Along Y Plane
        else
        {
            if (a.y < b.y) { b.y += OverlapAmount.y + RoomBuffer; OffsetRoom(ref b, true); }
            else { a.y += OverlapAmount.y + RoomBuffer; OffsetRoom(ref a, true); }
        }
        return true;
    }
    private void OffsetRoom(ref RectInt RoomArea, bool horizontal)
    {
        //Random Movement Left/Rght
        float Choice = Random.Range(0.0f, 1.0f);
        int MovementAmount = Random.Range(MinOffset, MaxOffset + 1);
        if (horizontal)
        {
            //Move Right
            if (Choice <= 0.35f) { RoomArea.x += MovementAmount; }
            //Move Left
            else { RoomArea.x -= MovementAmount; }
        }
        //Random Movement Up/Down
        else
        {
            //Move Up
            if (Choice <= 0.35f) { RoomArea.y += MovementAmount; }
            //Move Down
            else { RoomArea.y -= MovementAmount; }
        }
    }
    //Marks some random rooms into special rooms
    private void InsertSpecialRooms()
    {
        int specialRoomCount = 0;

        //Iterate through each room
        for(int i = 0; i < BasicDungeonRooms.Count; i++)
        {
            //Stops too many rooms being marked as special
            if (specialRoomCount >= MaxSpecialRooms) { break; }

            //Get random chance to change into special room
            float SRChanceToSpawn = Random.Range(0f, 1f);
            if (SRChanceToSpawn <= BaseSpecialRoomSpawnChance)
            {
                SpecialRoomKind SelectedRoom = GetRandomSpecialRoom();
                //Reassign room in question to new Special room
                BasicDungeonRooms[i] = new SpecialDungeonRoom(BasicDungeonRooms[i].Area, SelectedRoom);
                specialRoomCount++;
            }
        }
    }
    //Get random choice of special room based on weighted chances
    private SpecialRoomKind GetRandomSpecialRoom()
    {
        //Add all Chances
        float TotalChance = 0f;
        foreach (var entry in SpecialRoomChances)
        {
            TotalChance += entry.Chance;
        }

        //Select Special Room To Spawn
        float SelectedSpecial = Random.Range(0f, TotalChance);
        float CumulativeChance = 0f;
        foreach (var entry in SpecialRoomChances)
        {
            CumulativeChance += entry.Chance;
            if (SelectedSpecial <= CumulativeChance)
            {
                return entry.Type;
            }
        }
        //Fallback
        return SpecialRoomKind.None;
    }
    
    private void CreateCorridorConnections()
    {
        List<Vector2Int> roomCenters = GetRoomCenters();
        List<(Vector2Int, Vector2Int)> corridorConnections = GetRoomConnections(roomCenters);

        //Corridor Creation
        foreach (var (a, b) in corridorConnections)
        {
            int xStart = Mathf.Min(a.x, b.x);
            int yStart = Mathf.Min(a.y, b.y);

            bool horFirst = Random.value < 0.5f; //Determine whether to start with horizontal or vertical corridor

            RectInt XCor;
            RectInt YCor;

            if (horFirst)
            {
                XCor = new RectInt(new Vector2Int(xStart, a.y), new Vector2Int(Mathf.Abs(b.x - a.x) + 2, 2));
                YCor = new RectInt(new Vector2Int(b.x, yStart), new Vector2Int(2, Mathf.Abs(b.y - a.y) + 2));
            }
            else
            {
                YCor = new RectInt(new Vector2Int(a.x, yStart), new Vector2Int(2, Mathf.Abs(b.y - a.y) + 2));
                XCor = new RectInt(new Vector2Int(xStart, b.y), new Vector2Int(Mathf.Abs(b.x - a.x) + 2, 2));
            }
            DungeonCorridors.Add((XCor, YCor));
        }
    }
    //Connects rooms using Prim's Algorithm
    private List<(Vector2Int, Vector2Int)> GetRoomConnections(List<Vector2Int> roomCenters)
    {
        //Rooms that have been connected to another
        List<Vector2Int> roomsConnected = new List<Vector2Int>();
        //Start and end points of connections 
        List<(Vector2Int, Vector2Int)> savedConnections = new List<(Vector2Int, Vector2Int)>();

        roomsConnected.Add(roomCenters[0]); //Start with first room

        //repeat until all rooms are connected
        while (roomsConnected.Count < roomCenters.Count)
        {
            float closestDistance = float.MaxValue;
            Vector2Int fromRoom = Vector2Int.zero; 
            Vector2Int toRoom = Vector2Int.zero;

            //each room already connected
            foreach (var connectedRoom in roomsConnected)
            {
                //against every room unconnected
                foreach (var room in roomCenters)
                {
                    if (roomsConnected.Contains(room)) { continue; } // skip if already in connected list
                    //Distance between connected room and room in decision list
                    float distance = Vector2Int.Distance(connectedRoom, room);

                    //Save shortest distance
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        fromRoom = connectedRoom;
                        toRoom = room;
                    }
                }
            }
            //Connect closest room
            savedConnections.Add((fromRoom, toRoom));
            roomsConnected.Add(toRoom);
        }
        return savedConnections;
    }
    //Get All Room Centers
    private List<Vector2Int> GetRoomCenters()
    {
        List<Vector2Int> centers = new List<Vector2Int>();

        //For all dungeon rooms, calculate centerpoint and add it to the list
        foreach (var room in BasicDungeonRooms)
        {
            RectInt Area = room.Area;
            Vector2Int Center = new Vector2Int(Area.x + Area.width/2, Area.y + Area.height/2);
            centers.Add(Center);
        }
        return centers;
    }
    

    //Collect All Tile Positions
    private void CollectPositions()
    {
        //Gather all positions from rooms
        for (int i = 0; i < BasicDungeonRooms.Count; i++)
        {
            for(int x = BasicDungeonRooms[i].Area.xMin; x <= BasicDungeonRooms[i].Area.xMax; x++)
            {
                for(int y = BasicDungeonRooms[i].Area.yMin; y <= BasicDungeonRooms[i].Area.yMax; y++)
                {
                    Vector2Int Position = new Vector2Int(x, y);
                    TilePlacePositions.Add(Position);
                }
            }
        }
        //Gather all positions from corridors
        for (int i = 0; i < DungeonCorridors.Count; ++i)
        {
            RectInt XArea = DungeonCorridors[i].Item1;
            RectInt YArea = DungeonCorridors[i].Item2;

            //Horizontal Corridor Position Gathering
            for (int x = XArea.xMin; x <= XArea.xMax; x++)
            {
                for (int y = XArea.yMin; y <= XArea.yMax; y++)
                {
                    Vector2Int Position = new Vector2Int(x, y);
                    TilePlacePositions.Add(Position);
                }
            }
            //Vertical Corridor Position Gathering
            for (int x = YArea.xMin; x <= YArea.xMax; x++)
            {
                for (int y = YArea.yMin; y <= YArea.yMax; y++)
                {
                    Vector2Int Position = new Vector2Int(x, y);
                    TilePlacePositions.Add(Position);
                }
            }
        }

    }

    //Gather Max Bounds Accounting for Room Movement
    private RectInt GetMaxDungeonBounds()
    {
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;

        foreach (var Room in BasicDungeonRooms)
        {
            RectInt RoomArea = Room.Area;
            if (RoomArea.xMin < minX) { minX = RoomArea.xMin; }
            if (RoomArea.yMin < minY) { minY = RoomArea.yMin; }
            if (RoomArea.xMax > maxX) { maxX = RoomArea.xMax; }
            if (RoomArea.yMax > maxY) { maxY = RoomArea.yMax; }
        }
        RectInt LargestBounds = new RectInt(minX - 2, minY - 2, (maxX - minX) + 4, (maxY - minY) + 4);
        return LargestBounds;
    }

    public static event System.Action DungeonDataCreated;




    //TEMP VISUALIZATION FOR DUNGEON CREATION - GREEN = NORMAL ROOMS - BLUE = SPECIAL ROOMS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (SingleDungeonRoom room in BasicDungeonRooms)
        {
            if (room.roomType == RoomType.Default)
            {
                Gizmos.color = Color.green;
                Vector2 size = new Vector2(room.Area.width, room.Area.height);
                Vector2 center = new Vector2(room.Area.x + room.Area.width * 0.5f, room.Area.y + room.Area.height * 0.5f);

                Gizmos.DrawWireCube(center, size);
            }
            else if (room.roomType == RoomType.Special)
            {
                Gizmos.color = Color.blue;
                Vector2 size = new Vector2(room.Area.width, room.Area.height);
                Vector2 center = new Vector2(room.Area.x + room.Area.width * 0.5f, room.Area.y + room.Area.height * 0.5f);

                Gizmos.DrawWireCube(center, size);
            }
        }
        Gizmos.color = Color.green;
        foreach (var room in DungeonCorridors)
        {
            Vector2 size1 = new Vector2(room.Item1.width, room.Item1.height);
            Vector2 center1 = new Vector2(room.Item1.x + room.Item1.width * 0.5f, room.Item1.y + room.Item1.height * 0.5f);
            Gizmos.DrawWireCube(center1, size1);

            Vector2 size2 = new Vector2(room.Item2.width, room.Item2.height);
            Vector2 center2 = new Vector2(room.Item2.x + room.Item2.width * 0.5f, room.Item2.y + room.Item2.height * 0.5f);
            Gizmos.DrawWireCube(center2, size2);
        }
        Gizmos.color = Color.red;
        Vector3 totalSize = new Vector3(StartingTotalDungeonSize.width, StartingTotalDungeonSize.height);
        Vector3 totalcenter = new Vector3(
            StartingTotalDungeonSize.x + StartingTotalDungeonSize.width * 0.5f,
            StartingTotalDungeonSize.y + StartingTotalDungeonSize.height * 0.5f, 0);
        Gizmos.DrawWireCube(totalcenter, totalSize);
    }
}

public enum RoomType { Default, Special }
public enum SpecialRoomKind { None, Chest, Rest, Buff, Puzzle}
public class SingleDungeonRoom
{
    //Room Specifics
    public RectInt Area;
    public SingleDungeonRoom FirstChildRoom;
    public SingleDungeonRoom SecondChildRoom;
    public RoomType roomType = RoomType.Default;

    //Constructor
    public SingleDungeonRoom(RectInt size)
    {
        Area = size;
    }

    //Children Assignment
    public void AssignChildren(SingleDungeonRoom First, SingleDungeonRoom Second)
    {
        FirstChildRoom = First;
        SecondChildRoom = Second;
    }
}
public class SpecialDungeonRoom : SingleDungeonRoom
{
    public Vector2Int CenterPoint;
    public SpecialRoomKind SpecialType = SpecialRoomKind.None; //Assigned Upon Creation
    public SpecialDungeonRoom(RectInt size, SpecialRoomKind type) : base(size)
    {
        roomType = RoomType.Special;
        SpecialType = type;
        CenterPoint = new Vector2Int(size.x + size.width/2, size.y + size.height / 2);
    }

    //Points for Spawning Objects
    public Vector2Int[] Corners => new[]
    {
        new Vector2Int(Area.xMin + 1, Area.yMin + 1),
        new Vector2Int(Area.xMin + 1, Area.yMax - 1),
        new Vector2Int(Area.xMax - 1, Area.yMin + 1),
        new Vector2Int(Area.xMax - 1, Area.yMax - 1)
    };
}
