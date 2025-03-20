using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    public NavMeshSurface navMesh;
    public DungeonVisuals visuals;

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
    }
}
