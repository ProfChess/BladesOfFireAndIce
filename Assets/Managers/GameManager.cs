using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action GameStart;

    public PlayerController Player;
    [SerializeField] private NavMeshBaker MeshBaker;

    //Managers
    public static GameManager Instance;
    [HideInInspector] public DifficultyManager difficultyManager;
    [HideInInspector] public EnemySpawnManager enemySpawnManager;
    [HideInInspector] public StatManager statManager;
    [HideInInspector] public XPManager xpManager;
    [HideInInspector] public HitStopManager hitStopManager;
    [HideInInspector] public AbilitySelection abilityOptions;
    [HideInInspector] public BoonSelection boonOptions;
    [HideInInspector] public RelicSelection relicOptions;
    [HideInInspector] public RunDataManager runData;
    [HideInInspector] public SaveManager saveManager;
    [HideInInspector] public UIManager uiManager;

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
            return;
        }
        ManagerFirstTimeSetup();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = FindObjectOfType<PlayerController>();

        //Apply Health/Stamina
        Player.ApplySaveStats();

        //Apply all Virtues, relics, etc
        runData.ReapplyAllBonuses();
    }
    public void LoadScene(string sceneName)
    {
        Player.SaveStats();
        SceneManager.LoadScene(sceneName);
    }
    private void ManagerFirstTimeSetup()
    {
        //Other Managers
        difficultyManager = GetComponentInChildren<DifficultyManager>();
        enemySpawnManager = GetComponentInChildren<EnemySpawnManager>();
        statManager = GetComponentInChildren<StatManager>();
        xpManager = GetComponentInChildren<XPManager>();
        hitStopManager = GetComponentInChildren<HitStopManager>();
        abilityOptions = GetComponentInChildren<AbilitySelection>();
        boonOptions = GetComponentInChildren<BoonSelection>();
        relicOptions = GetComponentInChildren<RelicSelection>();
        runData = GetComponentInChildren<RunDataManager>();
        saveManager = GetComponentInChildren<SaveManager>();
        uiManager = GetComponentInChildren<UIManager>();

        //Other Objects Start
        StartCoroutine(GameBeginningDelayedCall());
    }

    //Events
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (Player != null)
        {
            Player.GetComponentInChildren<PlayerHealth>().PlayerIsDead += PlayerDeathEvent;
        }
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

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
    private List<GameObject> MenusToClose = new();
    public void MenuOpened(GameObject Menu) { MenusToClose.Add(Menu); ChangePlayerToUIActions(); }
    public void CloseLatestMenu()
    {
        if (MenusToClose.Count <= 0) { Debug.Log("No Menu Open"); return; }

        GameObject LatestMenu = MenusToClose[MenusToClose.Count - 1];
        LatestMenu.SetActive(false);
        MenusToClose.RemoveAt(MenusToClose.Count - 1);

        //Check if All Menus are Closed
        if (MenusToClose.Count == 0) { ChangePlayerToPlayerActions(); }
    }


    //New Run
    public void ActivateUIPopup_NewRun() { NewRunPopup.SetActive(true); }
    public void DeactivateUIPopup_NewRun() { NewRunPopup.SetActive(false); }
    //Shop
    public void InputUIPopup_Shop(List<ShopOption> options) { ShopGetter.PopulateShopOptions(options); }
    public void ActivateUIPopup_Shop() { ShopSelectionUI.SetActive(true); MenuOpened(ShopSelectionUI); }
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


    public GameObject getPlayer() 
    { 
        if (Player == null) { Player = FindObjectOfType<PlayerController>(); }
        return Player.gameObject; 
    }
    public NavMeshBaker getNavMesh() {  return MeshBaker; }
}
