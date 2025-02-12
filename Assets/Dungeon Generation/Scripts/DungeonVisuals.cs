using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonVisuals : MonoBehaviour
{
    //Variables
    [Header("Dungeon")]
    [SerializeField] private DungeonGenerator DG;
    [Header("Tiles")]
    [SerializeField] private Tilemap FloorTM;
    [SerializeField] private Tilemap WallTM;
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTile;

    //Private Lists
    private List<BoundsInt> roomList = new List<BoundsInt>();
    private List<Tuple<BoundsInt, BoundsInt>> corridorList = new List<Tuple<BoundsInt, BoundsInt>>();

    private HashSet<Vector3Int> FloorPositions = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> WallPositions = new HashSet<Vector3Int>();
    Vector3Int[] TileAdjacentDirecitons = 
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(1, -1, 0),
        new Vector3Int(-1, -1, 0),
    };
    // Start is called before the first frame update
    void Start()
    {
        roomList.Clear();
        corridorList.Clear();
        roomList = DG.getRoomList();
        corridorList = DG.getCorridorList();
        gatherTiles();
        placeTiles();
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
            FloorTM.SetTile(tilePosition, floorTile);
            foreach (Vector3Int Direction in TileAdjacentDirecitons)
            {
                Vector3Int AdjacentPos = tilePosition + Direction;
                if (!FloorPositions.Contains(AdjacentPos))
                {
                    WallPositions.Add(AdjacentPos);
                }
            }
        }
        foreach (Vector3Int WallPos in WallPositions)
        {
            WallTM.SetTile(WallPos, wallTile);
        }
    }
}
