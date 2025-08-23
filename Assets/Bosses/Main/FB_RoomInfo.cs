using System.Collections.Generic;
using UnityEngine;

public class FB_RoomInfo : MonoBehaviour
{
    public static FB_RoomInfo Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Teleporting")]
    [Header("Corners")]
    [SerializeField] private List<Transform> CornerPoints = new List<Transform>();

    [Header("N/S/E/W")]
    [SerializeField] private List<Transform> CardinalPoints = new List<Transform>();

    [Header("Middle")]
    [SerializeField] private Transform MiddlePoint;


    [Header("Room Area")]
    [SerializeField] private BoxCollider2D RoomArea;
    

    //Teleporting
    public Vector3 GetRandomCornerPoint(Vector2 CurrentPosition)
    {
        return FilterList(CornerPoints, CurrentPosition);
    }
    public Vector3 GetRandomCardinalPoint(Vector2 CurrentPosition)
    {
        return FilterList(CardinalPoints, CurrentPosition);
    }
    private Vector3 FilterList(List<Transform> GivenList, Vector3 CurrentPosition)
    {
        List<Transform> Options = new List<Transform>();

        foreach (Transform point in GivenList)
        {
            if(point.position != CurrentPosition)
            {
                Options.Add(point);
            }
        }
        if (GivenList.Count == 0) { return CurrentPosition; }

        int randNum = Random.Range(0, Options.Count);
        return Options[randNum].position;
    }
    public Vector3 GetMiddlePoint()
    {
        return MiddlePoint.position;
    }


    //Room Area
    public Vector2[] GetRoomCellPositions(int columns, int rows)
    {
        Bounds roombounds = RoomArea.bounds;
        float cellWidth = roombounds.size.x / columns;
        float cellHeight = roombounds.size.y / rows;

        List<Vector2> SpawnPositions = new List<Vector2>();
        Vector2 startPos = roombounds.min;
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Vector2 CellPos = new Vector2();
                CellPos.x = startPos.x + (c * cellWidth) + (cellWidth / 2f);
                CellPos.y = startPos.y + (r * cellHeight) + (cellHeight / 2f);

                SpawnPositions.Add(CellPos);
            }
        }
        return SpawnPositions.ToArray();
    }
}
