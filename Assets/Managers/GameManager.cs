using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action GameStart;

    private GameObject Player;
    [SerializeField] private NavMeshBaker MeshBaker;

    //Events
    private void OnEnable()
    {
        Player.GetComponentInChildren<PlayerHealth>().PlayerIsDead += PlayerDeathEvent;
    }
    private void OnDisable()
    {
        if (Player != null)
        {
            Player.GetComponentInChildren<PlayerHealth>().PlayerIsDead -= PlayerDeathEvent;
        }
    }


    //Event on Death of Player
    private void PlayerDeathEvent()
    {
        Time.timeScale = 0;
        Debug.Log("Game Over");
    }



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
