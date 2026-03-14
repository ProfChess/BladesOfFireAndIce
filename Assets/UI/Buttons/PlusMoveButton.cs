using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlusMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform ChangingTransform;
    [SerializeField] private Vector2 pressedOffset = new Vector2(0f, -1.5f);
    [SerializeField] private Vector2 OriginalPosition = new Vector2(1f, 2.5f);
    private Button thisButton;
    private void Start()
    {
        thisButton = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!thisButton.interactable) { return; }
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition + pressedOffset;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!thisButton.interactable) { return; }
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!thisButton.interactable) { return; }
        if (ChangingTransform != null)
        {
            ChangingTransform.anchoredPosition = OriginalPosition;
        }
    }

}
