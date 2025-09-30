using System.Collections.Generic;
using UnityEngine;

public class DungeonCreationV2 : MonoBehaviour
{
    //Specific Stats
    public RectInt TotalDungeonSize = new RectInt(0, 0, 100, 100);
    private int MinRoomLength = 10;
    private int MinRoomHeight = 10;
    private enum SplitAxis { Horizontal, Vertical, Invalid }

    private SingleDungeonRoom StartingRoom;
    private List<SingleDungeonRoom> DungeonRooms = new List<SingleDungeonRoom>();

    private void Awake()
    {
        StartingRoom = new SingleDungeonRoom(TotalDungeonSize);
        SplitSpace(StartingRoom);
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
            AxisToSplit = SplitAxis.Horizontal;
        }
        else if (!longEnough && tallEnough)
        {
            AxisToSplit = SplitAxis.Vertical;
        }
        else if (!longEnough && !tallEnough)
        {
            DungeonRooms.Add(Room);
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
                int YSplit = Room.Area.height / 2;
                if (Room.Area.height != MinRoomHeight * 2)
                {
                    YSplit = Random.Range(Room.Area.yMin + MinRoomHeight, Room.Area.yMax - MinRoomHeight);
                }

                //Create Top and Bottom Rooms
                SingleDungeonRoom TopRoom = new SingleDungeonRoom(new RectInt(Room.Area.xMin, Room.Area.yMin, Room.Area.width, YSplit - Room.Area.yMin));
                SingleDungeonRoom BottomRoom = new SingleDungeonRoom(new RectInt(Room.Area.xMin, YSplit, Room.Area.width, Room.Area.yMax - YSplit));
                
                //Assigning Children
                Room.AssignChildren(TopRoom, BottomRoom);
                break;
            
            case SplitAxis.Vertical:
                int XSplit = Room.Area.width / 2;
                if (Room.Area.width != MinRoomLength * 2)
                {
                    XSplit = Random.Range(Room.Area.xMin + MinRoomLength, Room.Area.xMax - MinRoomLength);
                }

                //Create Left and Right
                SingleDungeonRoom LeftRoom = new SingleDungeonRoom(new RectInt(Room.Area.xMin, Room.Area.yMin, XSplit - Room.Area.xMin, Room.Area.height));
                SingleDungeonRoom RightRoom = new SingleDungeonRoom(new RectInt(XSplit, Room.Area.yMin, Room.Area.xMax - XSplit, Room.Area.height));
                
                //Assigning Children
                Room.AssignChildren(LeftRoom, RightRoom);
                break;
            
            default:
                Debug.Log("Undefined Axis Given");
                break;
        }
    }
}

public class SingleDungeonRoom
{
    //Room Specifics
    public RectInt Area;
    public SingleDungeonRoom FirstChildRoom;
    public SingleDungeonRoom SecondChildRoom;

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
