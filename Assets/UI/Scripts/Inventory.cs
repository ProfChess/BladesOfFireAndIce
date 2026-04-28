using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryDescriptionUI inventoryDesc;
    public void DisplayItemDetails(BaseBoon Item)
    {
        Item.DisplayInformationInInventory(inventoryDesc);
    }
}
