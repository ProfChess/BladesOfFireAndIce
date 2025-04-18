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
    public BoundsInt GrabArea(Vector3 position)
    {
        Vector3Int RoundedPosition = Vector3Int.RoundToInt(position);
        for (int i = 0; i < roomList.Count; i++)
        {
            if (IsPositionInBounds(RoundedPosition, roomList[i].space))
            {
                return roomList[i].space;
            }

        }
        Debug.Log("Could Not Find Position in Any Room");
        return new BoundsInt();
    }
    private bool IsPositionInBounds(Vector3 Position, BoundsInt bounds)
    {
        return bounds.xMin <= Position.x &&
                bounds.xMax >= Position.x &&
                bounds.yMin <= Position.y &&
                bounds.yMax >= Position.y;
    }

}
