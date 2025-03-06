
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject Player;
    [SerializeField] private DungeonGenerator DunGen;
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
    }

    private void Start()
    {
        PlacePlayerAtStart();
    }

    //Placing + Respawning Player
    public void PlacePlayerAtStart()
    {
        Player.transform.position = DunGen.GetStartingRoomCenter();
    }
}
