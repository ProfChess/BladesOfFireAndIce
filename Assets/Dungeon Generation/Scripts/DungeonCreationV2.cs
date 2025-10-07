using System.Collections.Generic;
using UnityEngine;

public class DungeonCreationV2 : MonoBehaviour
{
    //Specific Stats
    public RectInt TotalDungeonSize = new RectInt(0, 0, 65, 65);
    private int MinRoomLength = 8;
    private int MinRoomHeight = 8;
    private int RoomBuffer = 2;
    private enum SplitAxis { Horizontal, Vertical, Invalid }

    //Rooms 
    private SingleDungeonRoom StartingRoom;
    private List<SingleDungeonRoom> DungeonRooms = new List<SingleDungeonRoom>();

    private void Awake()
    {
        MinRoomHeight = MinRoomHeight + RoomBuffer * 2;
        MinRoomLength = MinRoomLength + RoomBuffer * 2; 
        StartingRoom = new SingleDungeonRoom(TotalDungeonSize);
        SplitSpace(StartingRoom);

        //INSERT RANDOM REPLACING OF SPECIAL ROOMS

        //CONNECT ROOMS WITH CORRIDORS

        //ADD ALL SPACE/POSITIONS TO ADDITIONAL LIST FOR TILE PLACEMENT

        for (int i = 0; i < DungeonRooms.Count; i++)
        {
            Debug.Log("Room #" + i + " Width: " +  DungeonRooms[i].Area.width + " Height: " + DungeonRooms[i].Area.height);
        }
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

        RectInt NewSpace = new RectInt(
            Room.Area.xMin + RoomBuffer,
            Room.Area.yMin + RoomBuffer,
            Room.Area.width - RoomBuffer*2,
            Room.Area.height - RoomBuffer*2);

        Room.Area = NewSpace;
        DungeonRooms.Add(Room);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (SingleDungeonRoom room in DungeonRooms)
        {
            Vector3 size = new Vector3(room.Area.width, room.Area.height, 0);
            Vector3 center = new Vector3(room.Area.x + room.Area.width * 0.5f, room.Area.y + room.Area.height * 0.5f, 0);

            Gizmos.DrawWireCube(center, size);
        }
        Gizmos.color = Color.red;
        Vector3 totalSize = new Vector3(TotalDungeonSize.width, TotalDungeonSize.height);
        Vector3 totalcenter = new Vector3(
            TotalDungeonSize.x + TotalDungeonSize.width * 0.5f,
            TotalDungeonSize.y + TotalDungeonSize.height * 0.5f, 0);
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
public class SpecialDungeonRoom
{
    public RectInt Area;
    public Vector2Int CenterPoint;
    public RoomType roomType = RoomType.Special;
    public SpecialRoomKind SpecialType = SpecialRoomKind.None; //Assigned Upon Creation
    public SpecialDungeonRoom(RectInt size)
    {
        Area = size;
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
