using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action GameStart;

    private GameObject Player;
    [SerializeField] private NavMeshBaker MeshBaker;

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

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        GameStart?.Invoke();

    }
    public GameObject getPlayer() { return  Player; }
    public NavMeshBaker getNavMesh() {  return MeshBaker; }
}
