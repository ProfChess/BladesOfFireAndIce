using NavMeshPlus.Components;
using System;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMesh;
    public DungeonVisuals visuals;

    //Events
    public event Action MeshCreated;


    //ENABLE/DISABLE/EVENTS
    private void OnEnable()
    {
        visuals.DungeonComplete += CreateMesh;
    }
    private void OnDisable()
    {
        visuals.DungeonComplete -= CreateMesh;
    }

    private void CreateMesh()
    {
        navMesh.BuildNavMesh();
        MeshCreated?.Invoke();
    }
}
