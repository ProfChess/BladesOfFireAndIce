using System.Collections;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float yMaxLevel = 1f;          //How High the Visual Will go From Starting Position
    [SerializeField] private float yMinLevel = 1f;          //How Low the Visual Will go From Starting Position
    [SerializeField] private float MovementInterval = 0.1f; //Change in Height per Frame

    private Vector2 StartPosition = Vector2.zero;
    private Coroutine MovingCoroutine;

    public void StartObject()
    {
        StartPosition = transform.parent.position;
        if (MovingCoroutine == null)
        {
            MovingCoroutine = StartCoroutine(Move());
        }
    }
    public void StopObject()
    {
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
        }
    }

    private IEnumerator Move()
    {
        float Direction = 1f; //1 = Up, -1 = Down

        float upperLimit = StartPosition.y + yMaxLevel;
        float lowerLimit = StartPosition.y - yMinLevel;

        while (true)
        {
            yield return null;

            //Move
            transform.position += new Vector3(0f, Direction * MovementInterval, 0f);

            //Change Direction if Hits Limit
            if (transform.position.y >= upperLimit)
            {
                Direction = -1f;
            }
            else if (transform.position.y <= lowerLimit)
            {
                Direction = 1f;
            }
        }

    }
    
}
