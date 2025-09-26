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
    public void CreateMesh()
    {
        navMesh.BuildNavMesh();
        MeshCreated?.Invoke();
    }
}
