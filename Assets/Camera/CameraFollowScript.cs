using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private Vector2 xyMinLimit;
    private Vector2 xyMaxLimit;

    //References
    [SerializeField] private GameObject player;
    [SerializeField] private DungeonCreationV2 DunGen;

    //Camera
    private Camera cam;
    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        //Camera Set
        cam = Camera.main;

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

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;

        Vector2 PlayerPos = player.transform.position;
        float targetX = Mathf.Clamp(PlayerPos.x, xyMinLimit.x + camHalfWidth, xyMaxLimit.x - camHalfWidth);
        float targetY = Mathf.Clamp(PlayerPos.y, xyMinLimit.y + camHalfHeight, xyMaxLimit.y - camHalfHeight);

        Vector3 Goal = new Vector3(targetX, targetY, gameObject.transform.position.z);

        // Snapping position to nearest pixel grid based on PPU
        float pixelSize = 1f / 32f;
        Goal.x = Mathf.Round(Goal.x / pixelSize) * pixelSize;
        Goal.y = Mathf.Round(Goal.y / pixelSize) * pixelSize;

        // Directly move camera to the snapped position
        transform.position = Goal;

    }


}
