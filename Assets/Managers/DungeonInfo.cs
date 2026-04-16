using System.Collections.Generic;
using UnityEngine;

public class DungeonInfo : MonoBehaviour
{
    //Singleton
    public static DungeonInfo Instance;

    //List of Dungeon Info
    private List<SingleDungeonRoom> roomList = new List<SingleDungeonRoom>();
    //Get
    public List<SingleDungeonRoom> GetDungeonRoomList() { return roomList; }
    //Set
    public void SetInfo(List<SingleDungeonRoom> rooms)
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
    public SingleDungeonRoom GrabArea(Vector3 position)
    {
        Vector3Int RoundedPosition = Vector3Int.RoundToInt(position);
        for (int i = 0; i < roomList.Count; i++)
        {
            if (IsPositionInBounds(RoundedPosition, roomList[i].Area))
            {
                return roomList[i];
            }

        }
        Debug.Log("Could Not Find Position in Any Room");
        return roomList[0];
    }
    private bool IsPositionInBounds(Vector3 Position, RectInt bounds)
    {
        return bounds.xMin <= Position.x &&
                bounds.xMax >= Position.x &&
                bounds.yMin <= Position.y &&
                bounds.yMax >= Position.y;
    }


    //Save Edge Positions For Each Room
    //Dictionary to Keep Data
    Dictionary<SingleDungeonRoom, List<Vector2Int>> EdgePositions = new Dictionary<SingleDungeonRoom, List<Vector2Int>>();
    private void AssignEdgePositons()
    {
        foreach (SingleDungeonRoom room in roomList)
        {
            List<Vector2Int> EdgeList = new List<Vector2Int>();
            RectInt Bounds = room.Area;
            
            //Search Each Position of The Room
            for (int x = Bounds.xMin + 1; x < Bounds.xMax - 1; x++)
            {
                for (int y =  Bounds.yMin + 1; y < Bounds.yMax -1 ; y++)
                {
                    Vector2Int Position = new Vector2Int(x, y);
                    if (IsOutSideGivenBounds(Position, Bounds, 2))
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
    private bool IsOutSideGivenBounds(Vector2Int pos, RectInt bounds, int edgeThickness = 1)
    {
        return
        pos.x < bounds.xMin + edgeThickness ||
        pos.x >= bounds.xMax - edgeThickness ||
        pos.y < bounds.yMin + edgeThickness ||
        pos.y >= bounds.yMax - edgeThickness;
    }
    public List<Vector2Int> GetEdgePositionFromPosition(Vector2 Position)
    {
        SingleDungeonRoom room = GrabArea(Position);
        if (EdgePositions.TryGetValue(room, out List<Vector2Int> EdgeList))
        {
            return EdgeList;
        }
        else
        {
            return null;
        }
    }
}
