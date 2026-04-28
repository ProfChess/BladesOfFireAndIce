using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBoxUI : MonoBehaviour
{
    [SerializeField] private Image IconImage;
    private BaseBoon StoredObject;
    
    public void AssignItem(BaseBoon Item)
    {
        StoredObject = Item;
    }
    public void OnClick()
    {
        GameManager.Instance.uiManager.InventoryUIObject.DisplayItemDetails(StoredObject);
    }
}
