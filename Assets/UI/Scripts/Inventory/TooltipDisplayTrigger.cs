using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipDisplayTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemStatAccess item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        BaseStatUIDisplay parentUIDisplay = GetComponentInParent<BaseStatUIDisplay>();
        GameManager.Instance.uiManager.ShowTooltip(item.GetDisplayInfo(), parentUIDisplay.GetTooltipLocation(), parentUIDisplay.IsToolTipRight);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.uiManager.CloseTooltip();
    }
}
