using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private Vector2 xyMinLimit;
    private Vector2 xyMaxLimit;

    //References
    [SerializeField] private GameObject player;
    [SerializeField] private DungeonGenerator DunGen;

    private void Start()
    {
        xyMinLimit = new Vector2(DunGen.Space.xMin, DunGen.Space.yMin);
        xyMaxLimit = new Vector2(DunGen.Space.xMax, DunGen.Space.yMax);
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            followPlayer();
        }
    }

    private void followPlayer()
    {
        if (isWithinLimits())
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, 
                new Vector3(player.transform.position.x, player.transform.position.y, gameObject.transform.position.z), 1);

        }
    }


    private bool isWithinLimits()
    {
        //Check if camera is within map
        Vector2 p = gameObject.transform.position;
        if (p.x >= xyMinLimit.x && p.x <= xyMaxLimit.x)     //Checks X position
        {
            if (p.y >= xyMinLimit.y && p.y <= xyMaxLimit.y) //Checks Y position
            {
                return true;
            }
        }
        return false;
    }

}
