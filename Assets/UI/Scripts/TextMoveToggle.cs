using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class TextMoveToggle : MonoBehaviour
{
    [SerializeField] private Toggle thisToggle;
    [SerializeField] private Image ToggleOffImage;
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private Vector2 OnTextPosition = Vector2.zero;
    [SerializeField] private Vector2 OffTextPosition = Vector2.zero;
    private void Start()
    {
        thisToggle.onValueChanged.AddListener(MoveText);
    }
    private void MoveText(bool isOn)
    {
        if (OffTextPosition == Vector2.zero)
        {
            OffTextPosition = textTransform.anchoredPosition;
        }

        if (isOn)
        {
            textTransform.anchoredPosition = OnTextPosition;
            ToggleOffImage.enabled = false;
        }
        else
        {
            textTransform.anchoredPosition = OffTextPosition;
            ToggleOffImage.enabled = true;
        }
    }

    public void Refresh()
    {
        MoveText(thisToggle.isOn);
    }

}
