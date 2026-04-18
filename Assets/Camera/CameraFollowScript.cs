using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraFollowScript : MonoBehaviour
{
    public Vector2 xyMinLimit;
    public Vector2 xyMaxLimit;

    //References
    [SerializeField] private GameObject player;
    [SerializeField] private DungeonCreationV2 DunGen;

    //Camera
    private Camera cam;

    private void Start()
    {
        //Camera Set
        cam = Camera.main;
        cam.orthographicSize = 4f;

        player = GameManager.Instance.getPlayer();

        //Bounds
        xyMinLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMin + 6f, DunGen.FinalTotalDungeonSize.yMin + 3.5f);
        xyMaxLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMax - 6f, DunGen.FinalTotalDungeonSize.yMax - 3.5f);
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

        TargetPositon.x = Mathf.Clamp(TargetPositon.x, xyMinLimit.x, xyMaxLimit.x);
        TargetPositon.y = Mathf.Clamp(TargetPositon.y, xyMinLimit.y, xyMaxLimit.y);

        TargetPositon.x = Mathf.Round(TargetPositon.x * 32f) / 32f;
        TargetPositon.y = Mathf.Round(TargetPositon.y * 32f) / 32f;


        transform.position = TargetPositon + offset;
    }


}
