using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action GameStart;

    private GameObject Player;
    [SerializeField] private NavMeshBaker MeshBaker;

    //Managers
    public static GameManager Instance;
    [HideInInspector] public DifficultyManager difficultyManager;
    [HideInInspector] public EnemySpawnManager enemySpawnManager;
    [HideInInspector] public StatManager statManager;

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

        //Other Managers
        difficultyManager = GetComponentInChildren<DifficultyManager>();
        enemySpawnManager = GetComponentInChildren<EnemySpawnManager>();
        statManager = GetComponentInChildren<StatManager>();
    }

    private void Start()
    {
        GameStart?.Invoke();
    }
    public GameObject getPlayer() { return  Player; }
    public NavMeshBaker getNavMesh() {  return MeshBaker; }
}
