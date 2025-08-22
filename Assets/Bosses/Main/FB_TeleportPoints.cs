using System.Collections.Generic;
using UnityEngine;

public class FB_TeleportPoints : MonoBehaviour
{
    public static FB_TeleportPoints Instance;
    private void Awake()
    {
        Instance = this;
    }

    [Header("Corners")]
    [SerializeField] private List<Transform> CornerPoints = new List<Transform>();

    [Header("N/S/E/W")]
    [SerializeField] private List<Transform> CardinalPoints = new List<Transform>();

    [Header("Middle")]
    [SerializeField] private Transform MiddlePoint;

    public Vector3 GetRandomCornerPoint(Vector3 CurrentPosition)
    {
        return FilterList(CornerPoints, CurrentPosition);
    }
    public Vector3 GetRandomCardinalPoint(Vector3 CurrentPosition)
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
}
