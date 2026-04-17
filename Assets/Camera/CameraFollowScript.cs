using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraFollowScript : MonoBehaviour
{
    private Vector2 xyMinLimit;
    private Vector2 xyMaxLimit;

    //References
    [SerializeField] private GameObject player;
    [SerializeField] private DungeonCreationV2 DunGen;
    [SerializeField] private float SmoothTime = 0.08f;

    //Camera
    private Camera cam;

    private void Start()
    {
        //Camera Set
        cam = Camera.main;
        cam.orthographicSize = 4f;

        player = GameManager.Instance.getPlayer();

        //Bounds
        xyMinLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMin - 1, DunGen.FinalTotalDungeonSize.yMin - 1);
        xyMaxLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMax + 1, DunGen.FinalTotalDungeonSize.yMax + 1);
    }

    private void LateUpdate()
    {
        followPlayer();
    }

    private void followPlayer()
    {
        if (player == null) { return;}

        // Directly move camera to the snapped position
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 TargetPositon = player.transform.position;

        TargetPositon.x = Mathf.Floor(TargetPositon.x * 32f + 0.5f) / 32f;
        TargetPositon.y = Mathf.Floor(TargetPositon.y * 32f + 0.5f) / 32f;

        transform.position = TargetPositon + offset;
    }


}
