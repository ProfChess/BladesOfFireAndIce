using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventoryDescriptionUI inventoryDesc;
    [Header("Virtue and Relic Box Parents")]
    [SerializeField] private Transform VirtueContentParent;
    [SerializeField] private Transform RelicContentParent;
    [SerializeField] private GameObject InventoryItemBoxPrefab;

    [Header("Other Needed Objects")]
    [SerializeField] private StatManager statManager;

    [Header("Stat Display")]
    [SerializeField] private List<InventoryStatBlessingBox> StatBoxes;
    public void DisplayItemDetails(BaseAttainedBonus Item)
    {
        Item.DisplayInformationInInventory(inventoryDesc);
    }
    private void OnEnable()
    {
        //Add Self to Game Manager's Current Open Menu
        GameManager.Instance.MenuOpened(gameObject);

        //Update Stats
        foreach (InventoryStatBlessingBox box in StatBoxes)
        {
            box.AssignDisplayNumber(statManager.GetStatPointsFromStat(box.Stat));
        }
    }

    public void AddVirtueToInventory(Virtue virtue)
    {
        GameObject newBox = Instantiate(InventoryItemBoxPrefab, VirtueContentParent);
        newBox.GetComponent<InventoryItemBoxUI>().AssignItem(virtue);
    }
    public void AddRelicToInventory(Relic relic)
    {
        GameObject newBox = Instantiate(InventoryItemBoxPrefab, RelicContentParent);
        newBox.GetComponent<InventoryItemBoxUI>().AssignItem(relic);
    }


    //Add Blessing
    public void AddBlessingToInventory(StatBlessing blessing)
    {
        MainStatType statType = statManager.GetStatTypeOfBlessing(blessing);
        foreach (var Statbox in StatBoxes)
        {
            if (Statbox.Stat == statType)
            {
                //Correct Stat Found for Blessing
                Statbox.AssignItem(blessing);
                break;
            }
        }
    }
}
