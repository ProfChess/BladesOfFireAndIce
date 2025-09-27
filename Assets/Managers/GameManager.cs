using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //Dungeon Specifics
    [HideInInspector] public Vector3 DungeonStartingRoomCenter;
    [HideInInspector] public Vector3 DungeonEndingRoomCenter;
    [SerializeField] private GameObject DungeonEndPrefab;

    //Room Progression
    [Header("Floor Progression")]
    [SerializeField] private float RegularFloorNum = 3f;
    private float currentFloorNum = 0;
    private bool inBossRoom = false;

    //UI
    [Header("UI")]
    [SerializeField] private GameObject NewRunPopup;

    //Very Start Loading 
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Other Managers
        difficultyManager = GetComponentInChildren<DifficultyManager>();
        enemySpawnManager = GetComponentInChildren<EnemySpawnManager>();
        statManager = GetComponentInChildren<StatManager>();

        //Other Objects Start
        StartCoroutine(GameBeginningDelayedCall());
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Events
    private void OnEnable()
    {
        if (Player != null)
        {
            Player.GetComponentInChildren<PlayerHealth>().PlayerIsDead += PlayerDeathEvent;
        }
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

    private IEnumerator GameBeginningDelayedCall()
    {
        yield return null;
        GameStart?.Invoke();
        Instantiate(DungeonEndPrefab, DungeonEndingRoomCenter, Quaternion.identity);
    }

    //New Level
    public void BeginNewLevel()
    {
        if (currentFloorNum < RegularFloorNum)
        {
            currentFloorNum++;
            SceneManager.LoadScene("MainTestLevel");
        }
        else if (currentFloorNum >= RegularFloorNum)
        {
            SceneManager.LoadScene("BossTestLevel");
            currentFloorNum = 0;
            inBossRoom = true;
        }
    }
    
    //UI
    public void ActivateUIPopup()
    {
        NewRunPopup.SetActive(true);
    }
    public void DeactivateUIPopup()
    {
        NewRunPopup.SetActive(false);
    }
    public void DungeonFinished()
    {
        MeshBaker.CreateMesh();
    }





    public GameObject getPlayer() { return  Player; }
    public NavMeshBaker getNavMesh() {  return MeshBaker; }
}
