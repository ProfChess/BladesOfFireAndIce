using System.Collections.Generic;
using UnityEngine;

public class DungeonInfo : MonoBehaviour
{
    //Singleton
    public static DungeonInfo Instance;

    //List of Dungeon Info
    private List<DungeonRoom> roomList = new List<DungeonRoom>();
    //Get
    public List<DungeonRoom> GetDungeonRoomList() { return roomList; }
    //Set
    public void SetInfo(List<DungeonRoom> rooms)
    {
        roomList = rooms;
        AssignEdgePositons();
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Clear Dungeon Info
    private void ClearInfo()
    {
        roomList.Clear();
    }



    //Dungeon Room Info For Enemy AI
    //Gets Set bounds based on enemy starting point
    public DungeonRoom GrabArea(Vector3 position)
    {
        Vector3Int RoundedPosition = Vector3Int.RoundToInt(position);
        for (int i = 0; i < roomList.Count; i++)
        {
            if (IsPositionInBounds(RoundedPosition, roomList[i].space))
            {
                return roomList[i];
            }

        }
        Debug.Log("Could Not Find Position in Any Room");
        return roomList[0];
    }
    private bool IsPositionInBounds(Vector3 Position, BoundsInt bounds)
    {
        return bounds.xMin <= Position.x &&
                bounds.xMax >= Position.x &&
                bounds.yMin <= Position.y &&
                bounds.yMax >= Position.y;
    }


    //Save Edge Positions For Each Room
    //Dictionary to Keep Data
    Dictionary<DungeonRoom, List<Vector3Int>> EdgePositions = new Dictionary<DungeonRoom, List<Vector3Int>>();
    private void AssignEdgePositons()
    {
        foreach (DungeonRoom room in roomList)
        {
            List<Vector3Int> EdgeList = new List<Vector3Int>();
            BoundsInt Bounds = room.space;
            
            //Search Each Position of The Room
            for (int x = Bounds.xMin; x < Bounds.xMax; x++)
            {
                for (int y =  Bounds.yMin; y < Bounds.yMax; y++)
                {
                    Vector3Int Position = new Vector3Int(x, y, 0);
                    if (IsOutSideGivenBounds(Position, Bounds, 1))
                    {
                        //Position is Near Wall
                        EdgeList.Add(Position);
                    }
                }
            }
            //Add List to Dictionary under that room
            EdgePositions[room] = EdgeList;
        }

    }
    //Returns true if given value is outside bounds (ex. Finds Adjacent Wall Positions)
    private bool IsOutSideGivenBounds(Vector3Int pos, BoundsInt bounds, int edgeThickness = 1)
    {
        return
        pos.x < bounds.xMin + edgeThickness ||
        pos.x >= bounds.xMax - edgeThickness ||
        pos.y < bounds.yMin + edgeThickness ||
        pos.y >= bounds.yMax - edgeThickness;
    }
    public List<Vector3Int> GetEdgePositionFromPosition(Vector3 Position)
    {
        DungeonRoom room = GrabArea(Position);
        if (EdgePositions.TryGetValue(room, out List<Vector3Int> EdgeList))
        {
            return EdgeList;
        }
        else
        {
            return null;
        }
    }
}
