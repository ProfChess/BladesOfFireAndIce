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

        player = GameManager.Instance.getPlayer();

        //Bounds
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        xyMinLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMin + camHalfWidth, DunGen.FinalTotalDungeonSize.yMin + camHalfHeight);
        xyMaxLimit = new Vector2(DunGen.FinalTotalDungeonSize.xMax - camHalfWidth, DunGen.FinalTotalDungeonSize.yMax - camHalfHeight);
    }

    private void LateUpdate()
    {
        followPlayer();
        AlignProjection();
    }

    private void followPlayer()
    {
        if (player == null) { return;}

        // Directly move camera to the snapped position
        Vector3 offset = new Vector3(0, 0, -10);
        Vector3 TargetPositon = player.transform.position + offset;

        TargetPositon.x = Mathf.Clamp(TargetPositon.x, xyMinLimit.x, xyMaxLimit.x);
        TargetPositon.y = Mathf.Clamp(TargetPositon.y, xyMinLimit.y, xyMaxLimit.y);

        transform.position = TargetPositon;
    }
    void AlignProjection()
    {
        float unitsPerPixel = 2f * cam.orthographicSize / Screen.height;

        Vector3 pos = cam.transform.position;

        pos.x = Mathf.Round(pos.x / unitsPerPixel) * unitsPerPixel;
        pos.y = Mathf.Round(pos.y / unitsPerPixel) * unitsPerPixel;

        cam.transform.position = pos;
    }

}
