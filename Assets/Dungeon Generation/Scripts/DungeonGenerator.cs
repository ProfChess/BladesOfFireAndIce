using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{
    //Dungeon Specs
    [SerializeField] private BoundsInt Space;
    [SerializeField] private Vector2Int minRoomSize;

    private DungeonRoom FirstRoom;

    //List of rooms
    private List<BoundsInt> roomList = new List<BoundsInt>();
    private List<Tuple<BoundsInt, BoundsInt>> corridorList = new List<Tuple<BoundsInt, BoundsInt>>();
    private List<Vector3Int> roomCenterList = new List<Vector3Int>();

    void Start()
    {
        roomList.Clear();
        FirstRoom = new DungeonRoom(Space, minRoomSize);
        FirstRoom.SplitRecursive();       //Create Tree
        CollectLeafRooms(FirstRoom);      //Collect Leaf Nodes
        CollectCorridors();
    }

    private void CollectLeafRooms(DungeonRoom room) //Gathers rooms into list
    {
        if (room != null)
        {
            if (room.HasChild)
            {
                CollectLeafRooms(room.leftChild);
                CollectLeafRooms(room.rightChild);
            }
            else
            {
                roomList.Add(room.space);
            }
        }
    }

    private void CollectCorridors()
    {
        foreach (BoundsInt room in roomList)
        {
            roomCenterList.Add(Vector3Int.RoundToInt(room.center));
        }
        for (int i = 0; i < roomList.Count - 1; i++)
        {
            Vector3Int xy1 = new Vector3Int(roomCenterList[i].x, roomCenterList[i].y, 0);
            Vector3Int xy2 = new Vector3Int(roomCenterList[i+1].x, roomCenterList[i+1].y, 0);
            int xStart = Mathf.Min(xy1.x, xy2.x);
            int yStart = Mathf.Min(xy1.y, xy2.y);
            int xEnd = Mathf.Max(xy1.x, xy2.x);
            int yEnd = Mathf.Max(xy1.y, xy2.y);

            int xLength = Mathf.Max(1, xEnd -  xStart);
            int yLength = Mathf.Max(1, yEnd - yStart);

            BoundsInt XCor = new BoundsInt(new Vector3Int(xStart, xy1.y, 0), new Vector3Int(xLength, 1, 0));

            BoundsInt YCor = new BoundsInt(new Vector3Int(xy2.x, yStart, 0), new Vector3Int(1, yLength, 0));

            corridorList.Add(new Tuple<BoundsInt, BoundsInt>(XCor, YCor));
        }
    }
    private void OnDrawGizmos()
    {
        if (roomList == null) return;

        Gizmos.color = Color.green;

        foreach (BoundsInt room in roomList)
        {
            // Draw the room as a wireframe box
            Gizmos.DrawWireCube(room.center, room.size);
        }
        Gizmos.color = Color.red;
        foreach (Tuple<BoundsInt, BoundsInt> Cor in corridorList)
        {
            Gizmos.DrawWireCube(Cor.Item1.center, Cor.Item1.size);
            Gizmos.DrawWireCube(Cor.Item2.center, Cor.Item2.size);
        }
    }

}

public class DungeonRoom
{
    public BoundsInt space;
    public DungeonRoom leftChild;
    public DungeonRoom rightChild;

    //Check if room has children
    public bool HasChild => leftChild != null || rightChild != null;

    private Vector2Int minRoomSize;


    //Constructor
    public DungeonRoom(BoundsInt roomSpace, Vector2Int minSize)
    {
        space = roomSpace;
        minRoomSize = minSize;
    }

    // Method to split the room
    public void Split()
    {
        var random = new System.Random();

        //Calculate Aspect Ratio of Room
        float aspectRatio = (float)space.size.y / space.size.x;
        
        //Calculate Bias
        float hBias = aspectRatio;
        float vBias = 1.0f / aspectRatio;
        float totalBias = hBias + vBias;
        float weightedRandom = (float)random.NextDouble() * totalBias;
        int randomAxis;
        if (weightedRandom < hBias)
        {
            randomAxis = 0;
        }
        else
        {
            randomAxis = 1;
        }

        int randomPoint;
        int roomBuffer = 1;

        if (randomAxis == 0)                // Horizontal split
        {
            randomPoint = random.Next(space.yMin + minRoomSize.y, space.yMax - minRoomSize.y); // avoid splitting too close to the edge

            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(space.size.x, randomPoint - space.yMin - roomBuffer, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(space.xMin, randomPoint + roomBuffer, 0), new Vector3Int(space.size.x, space.yMax - randomPoint - roomBuffer, 0));

            // Ensure both rooms are big enough
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom (leftSpace, minRoomSize);
                rightChild = new DungeonRoom (rightSpace, minRoomSize);
            }
        }
        else // Vertical split
        {
            randomPoint = random.Next(space.xMin + minRoomSize.x, space.xMax - minRoomSize.x); // avoid splitting too close to the edge

            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(randomPoint - space.xMin - roomBuffer, space.size.y, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(randomPoint + roomBuffer, space.yMin, 0), new Vector3Int(space.xMax - randomPoint - roomBuffer, space.size.y, 0));

            // Ensure both rooms are big enough
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom(leftSpace, minRoomSize);
                rightChild = new DungeonRoom(rightSpace, minRoomSize);
            }
        }
    }

    //Create BSP Tree
    public void SplitRecursive()
    {
        if (space.size.x <= 2 * minRoomSize.x || space.size.y <= 2 * minRoomSize.y)
        {
            return;
        }

        Split(); //Split again

        if (leftChild == null && rightChild == null)
        {
            return;
        }

        //If successful split again
        if (leftChild != null)
        {
            leftChild.SplitRecursive();
        }
        if (rightChild != null)
        {
            rightChild.SplitRecursive();
        }
    }
}
