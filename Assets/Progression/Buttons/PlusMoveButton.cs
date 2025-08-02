using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlusMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform ChangingTransform;
    private Vector2 pressedOffset = new Vector2(0f, -1.5f);
    private Vector2 OriginalPosition = new Vector2(1f, 2.5f);

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition + pressedOffset;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition;
        }
    }


}
