using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowTextScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;    
    [SerializeField] private GameObject UpArrow;
    [SerializeField] private GameObject DownArrow;

    private void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScroll);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        OnScroll(scrollRect.normalizedPosition);
    }
    private void OnScroll(Vector2 Position)
    {
        float contentHeight = scrollRect.content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;
        if (contentHeight <= viewportHeight)
        {
            UpArrow.SetActive(false);
            DownArrow.SetActive(false);
            return;
        }

        float y = Position.y;
        UpArrow.SetActive(y < 0.95f);
        DownArrow.SetActive(y > 0.05f);
    }
    public void UpdateArrowScroll()
    {
        OnScroll(scrollRect.normalizedPosition);
    }
}
