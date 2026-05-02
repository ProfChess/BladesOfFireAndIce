using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemBoxUI : MonoBehaviour
{
    [SerializeField] protected Image IconImage;
    protected BaseAttainedBonus StoredObject;
    
    public void AssignItem(BaseAttainedBonus Item)
    {
        StoredObject = Item;
    }
    public void OnClick()
    {
        GameManager.Instance.uiManager.InventoryUIObject.DisplayItemDetails(StoredObject);
    }
}
