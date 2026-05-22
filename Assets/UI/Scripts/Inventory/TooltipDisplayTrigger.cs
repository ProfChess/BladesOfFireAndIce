using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipDisplayTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemStatAccess item;
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.uiManager.ShowTooltip(item.GetDisplayInfo(), GetComponentInParent<BaseStatUIDisplay>().GetTooltipLocation());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.uiManager.CloseTooltip();
    }
}
