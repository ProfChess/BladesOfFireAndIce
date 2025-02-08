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
    private List<Tuple<BoundsInt, BoundsInt, float>> roomDistances = new List<Tuple<BoundsInt, BoundsInt, float>>();

    //Room Specifics
    [SerializeField]
    private int roomBuffer = 1;

    void Start()
    {
        roomList.Clear();
        FirstRoom = new DungeonRoom(Space, minRoomSize, roomBuffer);
        FirstRoom.SplitRecursive();       //Create Tree
        CollectLeafRooms(FirstRoom);      //Collect Leaf Nodes
        SortRoomDistance();
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
    private void SortRoomDistance()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            for (int a = i + 1; a < roomList.Count; a++)
            {
                BoundsInt FirstRoom = roomList[i];
                BoundsInt SecondRoom = roomList[a];

                float d = Vector3Int.Distance(Vector3Int.RoundToInt(FirstRoom.center), Vector3Int.RoundToInt(SecondRoom.center));
                roomDistances.Add(new Tuple<BoundsInt, BoundsInt, float>(FirstRoom, SecondRoom, d));
            }
        }

        roomDistances.Sort((a, b) => a.Item3.CompareTo(b.Item3));
        GenerateMST();
    }
    private void GenerateMST()
    {
        //Set of rooms already checked
        HashSet<BoundsInt> checkedRooms = new HashSet<BoundsInt>();
        List<Tuple<BoundsInt, BoundsInt, float>> freeEdges = new List<Tuple<BoundsInt, BoundsInt, float>>();

        checkedRooms.Add(roomList[0]); //Pick First Room

        //Get all connected edges
        foreach (var edge in roomDistances)
        {
            if (edge.Item1 == roomList[0] || edge.Item2 == roomList[0])
            {
                freeEdges.Add(edge);
            }
        }
        while (checkedRooms.Count < roomList.Count)
        {
            freeEdges.Sort((a, b) => a.Item3.CompareTo(b.Item3));

            var shortestEdge = freeEdges[0];
            freeEdges.RemoveAt(0);

            BoundsInt newRoom = !checkedRooms.Contains(shortestEdge.Item1) ? shortestEdge.Item1 : shortestEdge.Item2;

            if (!checkedRooms.Contains(newRoom))
            {
                checkedRooms.Add(newRoom);

                CreateCorridor(shortestEdge.Item1.center, shortestEdge.Item2.center);

                foreach (var edge in roomDistances)
                {
                    if ((edge.Item1 == newRoom || edge.Item2 == newRoom) && (!checkedRooms.Contains(edge.Item1) || !checkedRooms.Contains(edge.Item2)))
                    {
                        freeEdges.Add(edge);
                    }
                }
            }
        }
    }
    private void CreateCorridor(Vector3 First, Vector3 Second)
    {
        Vector3Int xy1 = Vector3Int.RoundToInt(First);
        Vector3Int xy2 = Vector3Int.RoundToInt(Second);

        int xStart = Mathf.Min(xy1.x, xy2.x);
        int yStart = Mathf.Min(xy1.y, xy2.y);
        int xEnd = Mathf.Max(xy1.x, xy2.x);
        int yEnd = Mathf.Max(xy1.y, xy2.y);

        int xLength = Mathf.Max(1, xEnd - xStart);
        int yLength = Mathf.Max(1, yEnd - yStart);

        BoundsInt XCor = new BoundsInt(new Vector3Int(xStart, xy1.y, 0), new Vector3Int(xLength, 1, 0));

        BoundsInt YCor = new BoundsInt(new Vector3Int(xy2.x, yStart, 0), new Vector3Int(1, yLength, 0));

        corridorList.Add(new Tuple<BoundsInt, BoundsInt>(XCor, YCor));
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
    private int roomBuffer;

    //Constructor
    public DungeonRoom(BoundsInt roomSpace, Vector2Int minSize, int roomBuff)
    {
        space = roomSpace;
        minRoomSize = minSize;
        roomBuffer = roomBuff;
    }

    // Method to split the room
    public void Split()
    {
        var random = new System.Random();

        //Calculate Aspect Ratio of Room
        float aspectRatio = (float)space.size.y / space.size.x;

        //Check if splitting is only possible one way
        int randomAxis;
        if (space.size.x <= 2 * minRoomSize.x)
        {
            randomAxis = 0;
        }
        else if (space.size.y <= 2 * minRoomSize.y)
        {
            randomAxis = 1;
        }
        else
        {
            //Calculate Bias
            float hBias = aspectRatio;
            float vBias = 1.0f / aspectRatio;
            float totalBias = hBias + vBias;
            float weightedRandom = (float)random.NextDouble() * totalBias;
            if (weightedRandom < hBias)
            {
                randomAxis = 0;
            }
            else
            {
                randomAxis = 1;
            }
        }
        

        int randomPoint;

        if (randomAxis == 0)                // Horizontal split
        {
            randomPoint = random.Next(space.yMin + minRoomSize.y, space.yMax - minRoomSize.y); // avoid splitting too close to the edge

            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(space.size.x, randomPoint - space.yMin - roomBuffer, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(space.xMin, randomPoint + roomBuffer, 0), new Vector3Int(space.size.x, space.yMax - randomPoint - roomBuffer, 0));

            // Ensure both rooms are big enough
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom (leftSpace, minRoomSize, roomBuffer);
                rightChild = new DungeonRoom (rightSpace, minRoomSize, roomBuffer);
            }
        }
        else                                // Vertical split
        {
            randomPoint = random.Next(space.xMin + minRoomSize.x, space.xMax - minRoomSize.x); // avoid splitting too close to the edge

            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(randomPoint - space.xMin - roomBuffer, space.size.y, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(randomPoint + roomBuffer, space.yMin, 0), new Vector3Int(space.xMax - randomPoint - roomBuffer, space.size.y, 0));

            // Ensure both rooms are big enough
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom(leftSpace, minRoomSize, roomBuffer);
                rightChild = new DungeonRoom(rightSpace, minRoomSize, roomBuffer);
            }
        }
    }

    //Create BSP Tree
    public void SplitRecursive()
    {
        if (space.size.x > 2 * minRoomSize.x || space.size.y > 2 * minRoomSize.y)
        {
            Split(); //Split again
        }

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
