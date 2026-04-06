using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TextMoveToggle : MonoBehaviour
{
    [SerializeField] private Toggle thisToggle;
    [SerializeField] private Image ToggleOffImage;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private Vector2 ToggledOffset = Vector2.zero;
    private Vector2 OriginalPosition;
    private void Start()
    {
        thisToggle.onValueChanged.AddListener(MoveText);
    }
    private void MoveText(bool isOn)
    {
        if (OriginalPosition == Vector2.zero)
        {
            OriginalPosition = textTransform.anchoredPosition;
        }

        if (isOn)
        {
            textTransform.anchoredPosition = ToggledOffset;
            ToggleOffImage.enabled = false;
        }
        else
        {
            textTransform.anchoredPosition = OriginalPosition;
            ToggleOffImage.enabled = true;
        }
    }

    public void Refresh()
    {
        MoveText(thisToggle.isOn);
    }

}
