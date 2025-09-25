using System;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour 
{
    //Dungeon Specs
    [HideInInspector]
    public BoundsInt Space;
    private Vector2Int minRoomSize;

    private DungeonRoom FirstRoom;

    //List of rooms
    private List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();
    private List<BoundsInt> roomList = new List<BoundsInt>(); //List of all rooms
    private List<Tuple<BoundsInt, BoundsInt>> corridorList = new List<Tuple<BoundsInt, BoundsInt>>();
    private List<Tuple<BoundsInt, BoundsInt, float>> roomDistances = new List<Tuple<BoundsInt, BoundsInt, float>>();

    //Room Specifics
    [SerializeField] private int roomBuffer = 1;

    private BoundsInt StartingRoom;
    private BoundsInt EndingRoom;
    private int roomIDGiver = 0;
    private DifficultyManager DM => GameManager.Instance.difficultyManager;
    void Awake()
    {
        Space = new BoundsInt(new Vector3Int(0, 0, 0), DM.GetMapSize());
        minRoomSize = DM.GetRoomSize();

        roomIDGiver = 0; //Resets Room IDs

        roomList.Clear();
        if (!(Space.size.x > 2 * minRoomSize.x + roomBuffer && Space.size.y > 2 * minRoomSize.y + roomBuffer))
        {
            if (!DM.isTesting)
            {
                Debug.Log("Returning To Default Dungeon Size");
                Space.size = new Vector3Int(40, 40, 0);
                roomBuffer = 1;
                minRoomSize = new Vector2Int(5, 5);
            }
        }

        FirstRoom = new DungeonRoom(Space, minRoomSize, roomBuffer);
        FirstRoom.SplitRecursive();       //Create Tree
        CollectLeafRooms(FirstRoom);      //Collect Leaf Nodes
        DungeonInfo.Instance.SetInfo(dungeonRooms);
        SortRoomDistance();               //Sorts out an efficient way to connect rooms
    }
    private void Start()
    {
        AssignStartingEndingRooms();
    }

    private void CollectLeafRooms(DungeonRoom room) //Gathers rooms into list
    {
        if (room != null)
        {
            if (!room.HasChild)
            {
                roomList.Add(room.space);
                room.SetRoomID(roomIDGiver++);
                dungeonRooms.Add(room);
            }
            else
            {
                if (room.leftChild != null)
                {
                    CollectLeafRooms(room.leftChild);
                }
                if (room.rightChild != null)
                {
                    CollectLeafRooms(room.rightChild);
                }
            }
        }
    }
    private void SortRoomDistance() //Sorts out efficient way to connect rooms
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
    private void GenerateMST() //Connects rooms and Creates corridors 
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
    private void CreateCorridor(Vector3 First, Vector3 Second) //Creates corridor between two positions
    {
        Vector3Int xy1 = Vector3Int.RoundToInt(First);
        Vector3Int xy2 = Vector3Int.RoundToInt(Second);

        int xStart = Mathf.Min(xy1.x, xy2.x);
        int yStart = Mathf.Min(xy1.y, xy2.y);
        int xEnd = Mathf.Max(xy1.x, xy2.x);
        int yEnd = Mathf.Max(xy1.y, xy2.y);

        int xLength = Mathf.Max(1, xEnd - xStart);
        int yLength = Mathf.Max(1, yEnd - yStart);

        BoundsInt XCor = new BoundsInt(new Vector3Int(xStart, xy1.y, 0), new Vector3Int(xLength, 2, 0));

        BoundsInt YCor = new BoundsInt(new Vector3Int(xy2.x, yStart, 0), new Vector3Int(2, yLength, 0));

        corridorList.Add(new Tuple<BoundsInt, BoundsInt>(XCor, YCor));
    }


    //Get Room and corridor lists for decorations
    public List<BoundsInt> getRoomList()
    {
        return roomList;
    }
    public List<Tuple<BoundsInt, BoundsInt>> getCorridorList()
    {
        return corridorList;
    }


    //Getting and Setting Player Spawn Room and Ending Room
    private void AssignStartingEndingRooms()
    {
        StartingRoom = FirstRoom.space;
        EndingRoom = FirstRoom.space;

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].center.x < StartingRoom.center.x || roomList[i].center.y < StartingRoom.center.y)
            {
                StartingRoom = roomList[i];
            }
            if (roomList[i].center.x > EndingRoom.center.x || roomList[i].center.y > EndingRoom.center.y)
            {
                EndingRoom = roomList[i];
            }
        }
        GameManager.Instance.DungeonStartingRoomCenter = StartingRoom.center;
        GameManager.Instance.DungeonEndingRoomCenter = EndingRoom.center;
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

    public int roomID;
    public int GetRoomID() { return roomID; }
    public void SetRoomID(int ID) {  roomID = ID; }

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
            float hBias = Mathf.Clamp(aspectRatio, 0.5f, 2.0f);
            float vBias = 1.0f / Mathf.Clamp(aspectRatio, 0.5f, 2.0f);
            float totalBias = hBias + vBias;
            float weightedRandom = (float)random.NextDouble() * totalBias;
            randomAxis = (weightedRandom < hBias) ? 0 : 1;
        }
        

        int randomPoint;

        if (randomAxis == 0)                // Horizontal split -> Y AXIS
        {
            if (space.size.y - 2 * minRoomSize.y - roomBuffer <= 0) { return; }
            if (space.yMin + minRoomSize.y + roomBuffer >= space.yMax - minRoomSize.y - roomBuffer)
            {
                randomPoint = (space.yMin + space.yMax) / 2;
            }
            else
            {
                randomPoint = random.Next(space.yMin + minRoomSize.y + roomBuffer, space.yMax - minRoomSize.y - roomBuffer);
            }

            SplitAttempt(randomAxis, randomPoint);
        }
        else                                // Vertical split -> X AXIS
        {
            if (space.size.x - 2 * minRoomSize.x - roomBuffer <= 0) { return; }
            if (space.xMin + minRoomSize.x + roomBuffer >= space.xMax - minRoomSize.x - roomBuffer)
            {
                randomPoint = (space.xMin + space.xMax) / 2;
            }
            else
            {
                randomPoint = random.Next(space.xMin + minRoomSize.x + roomBuffer, space.xMax - minRoomSize.x - roomBuffer);
            }

            SplitAttempt(randomAxis, randomPoint);
        }
    }
    private bool SplitAttempt(int randomAxis, int splitPoint)
    {
        if (randomAxis == 0)
        {
            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(space.size.x, splitPoint - space.yMin - roomBuffer, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(space.xMin, splitPoint + roomBuffer, 0), 
                new Vector3Int(space.size.x, space.yMax - splitPoint - roomBuffer, 0));
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom(leftSpace, minRoomSize, roomBuffer);
                rightChild = new DungeonRoom(rightSpace, minRoomSize, roomBuffer);
                return true;
            }
        }
        else
        {
            BoundsInt leftSpace = new BoundsInt(space.position, new Vector3Int(splitPoint - space.xMin - roomBuffer, space.size.y, 0));
            BoundsInt rightSpace = new BoundsInt(new Vector3Int(splitPoint + roomBuffer, space.yMin, 0), 
                new Vector3Int(space.xMax - splitPoint - roomBuffer, space.size.y, 0));

            // Ensure both rooms are big enough
            if (leftSpace.size.x >= minRoomSize.x && leftSpace.size.y >= minRoomSize.y &&
                rightSpace.size.x >= minRoomSize.x && rightSpace.size.y >= minRoomSize.y)
            {
                leftChild = new DungeonRoom(leftSpace, minRoomSize, roomBuffer);
                rightChild = new DungeonRoom(rightSpace, minRoomSize, roomBuffer);
                return true;
            }
        }
        return false;
        
    }

    //Create BSP Tree
    public void SplitRecursive()
    {
        if (space.size.x > 2 * minRoomSize.x || space.size.y > 2 * minRoomSize.y)
        {
            Split();
        }
        else if (space.size.x <= 2 * minRoomSize.x && space.size.y <= 2 * minRoomSize.y)
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
