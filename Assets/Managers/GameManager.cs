using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [HideInInspector] public HitStopManager hitStopManager;
    [HideInInspector] public AbilitySelection abilityOptions;
    [HideInInspector] public BoonSelection boonOptions;
    [HideInInspector] public RelicSelection relicOptions;
    [HideInInspector] public RunDataManager runData;

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
    [SerializeField] private ShopUI ShopGetter;
    [Tooltip("Main Shop Menu Object to Turn On/Off")]
    [SerializeField] private GameObject ShopSelectionUI;
    public event Action MenuClosed;

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
        hitStopManager = GetComponentInChildren<HitStopManager>();
        abilityOptions = GetComponentInChildren<AbilitySelection>();
        boonOptions = GetComponentInChildren<BoonSelection>();
        relicOptions = GetComponentInChildren<RelicSelection>();
        runData = GetComponentInChildren<RunDataManager>();

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
    public void ChangePlayerToUIActions()
    {
        getPlayer().GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
    }
    public void ChangePlayerToPlayerActions()
    {
        getPlayer().GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerButtons");
    }

    //Close All Menus
    public void CloseMenus() 
    {
        //Close UI 
        NewRunPopup.SetActive(false);
        ShopSelectionUI.SetActive(false);
        boonOptions.SelectionPopup.SetActive(false);
        MenuClosed?.Invoke();

        //Change Action Map
        ChangePlayerToPlayerActions();
    }
    //New Run
    public void ActivateUIPopup_NewRun() { NewRunPopup.SetActive(true); }
    public void DeactivateUIPopup_NewRun() { NewRunPopup.SetActive(false); }
    //Shop
    public void InputUIPopup_Shop(List<ShopOption> options) { ShopGetter.PopulateShopOptions(options); }
    public void ActivateUIPopup_Shop() { ShopSelectionUI.SetActive(true); ChangePlayerToUIActions(); }
    public void DeactivateUIPopup_Shop() { ShopSelectionUI.SetActive(false); }
    public void MakeShopDecision(ShopOption ChosenShopItem)
    {
        runData.AddShopCurrency(-ChosenShopItem.Description.ItemCost);
        ShopGetter.ReEvalutateShop();
        if (ChosenShopItem != null)
        {
            ChosenShopItem.ApplyChoice();
        }
    }

    public void DungeonFinished()
    {
        MeshBaker.CreateMesh();
    }


    public GameObject getPlayer() { return  Player; }
    public NavMeshBaker getNavMesh() {  return MeshBaker; }
}
