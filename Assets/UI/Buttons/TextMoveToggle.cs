using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TextMoveToggle : MonoBehaviour
{
    [SerializeField] private Image ToggleOffImage;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private Vector2 ToggledOffset = Vector2.zero;
    private Vector2 OriginalPosition = Vector2.zero;
    private Toggle thisToggle;
    private void Start()
    {
        thisToggle = GetComponent<Toggle>();
        OriginalPosition = textTransform.anchoredPosition;
        thisToggle.onValueChanged.AddListener(MoveText);
    }
    private void MoveText(bool isOn)
    {
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

}
