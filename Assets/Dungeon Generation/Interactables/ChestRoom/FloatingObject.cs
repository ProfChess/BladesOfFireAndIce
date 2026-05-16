using System.Collections;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Float Controls")]
    [SerializeField] private float yMaxLevel = 1f;          //How High the Visual Will go From Starting Position
    [SerializeField] private float yMinLevel = 1f;          //How Low the Visual Will go From Starting Position
    [SerializeField] private float MovementSpeed = 0.1f;    //Change in Height per Frame

    private Coroutine MovingCoroutine;

    private RectTransform rectTransform;
    private bool isUI;
    
    private Vector2 StartWorldPosition = Vector2.zero;
    private Vector2 StartAnchoredPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        isUI = rectTransform != null && GetComponentInParent<Canvas>() != null;
    }
    public void StartObject()
    {
        if (isUI) { StartAnchoredPosition = rectTransform.anchoredPosition; }
        else { StartWorldPosition = transform.position; }

        ReturnToStart();
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
        }
        MovingCoroutine = StartCoroutine(Move());
    }
    public void StopObject()
    {
        if (MovingCoroutine != null)
        {
            StopCoroutine(MovingCoroutine);
            MovingCoroutine = null;
        }
        ReturnToStart();
    }
    public void ReturnToStart()
    {
        if (isUI) { rectTransform.anchoredPosition = StartAnchoredPosition; }
        else { transform.position = StartWorldPosition; }
    }

    private float GetEdgeDeceleration(float value, float min, float max)
    {
        float mid = (min + max) * 0.5f;
        float distFromCenter = Mathf.Abs(value - mid);
        float range = (max - min) * 0.5f;

        float t = distFromCenter / range;

        //Slow down near edges (0 at edge, 1 at center)
        return Mathf.SmoothStep(0.2f, 1f, 1f - t);
    }
    private IEnumerator Move()
    {
        float Direction = 1f; //1 = Up, -1 = Down

        while (true)
        {
            yield return null;
            if (isUI)
            {
                Vector2 pos = rectTransform.anchoredPosition;
                float min = StartAnchoredPosition.y - yMinLevel;
                float max = StartAnchoredPosition.y + yMaxLevel;

                float speedMult = GetEdgeDeceleration(pos.y, min, max);

                pos.y += Direction * MovementSpeed * speedMult * GameTimeManager.GameDeltaTime;

                if (pos.y >= max) { pos.y = max; Direction = -1f; }
                else if(pos.y <= min) { pos.y = min; Direction = 1f; }

                rectTransform.anchoredPosition = pos;
            }
            else
            {
                Vector2 pos = transform.position;
                float min = StartWorldPosition.y - yMinLevel;
                float max = StartWorldPosition.y + yMaxLevel;

                float speedMult = GetEdgeDeceleration(pos.y, min, max);

                pos.y += Direction * MovementSpeed * speedMult * GameTimeManager.GameDeltaTime;

                if (pos.y >= StartWorldPosition.y + yMaxLevel) { Direction = -1f; }
                else if (pos.y <= StartWorldPosition.y - yMinLevel) { Direction = 1f; }

                transform.position = pos;
            }
        }

    }
    
}
