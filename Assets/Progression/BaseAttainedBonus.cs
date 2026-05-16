using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseAttainedBonus : ScriptableObject
{
    [Header("Boon Stats")]
    public string BonusName;
    public BonusType type = BonusType.Virtue;
    [TextArea] public string BonusDescription;

    public virtual void DisplayDetailsOfBonusInInventory(InventoryDescriptionUI inventoryObj)
    {
        inventoryObj.AssignTextFromItem(BonusName, type.ToString(), BonusDescription);
    }
    public virtual void DisplayStatsOfBonusInInventory(InventoryDescriptionUI inventoryObj)
    {
        inventoryObj.AssignStatsFromItem(GetListOfStatsForDisplay());
    }
    public virtual List<StatDisplayEntry> GetListOfStatsForDisplay()
    {
        return new List<StatDisplayEntry>();
    }
}
