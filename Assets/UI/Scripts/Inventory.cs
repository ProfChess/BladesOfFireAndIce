using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private RunDataManager runDataManager;

    [Header("Currency UI")]
    [SerializeField] private TextMeshProUGUI GoldNumUI;
    [SerializeField] private TextMeshProUGUI EssenceNumUI;

    [Header("Ability UI")]
    [SerializeField] private InventoryItemBoxUI Slot1Fire;
    [SerializeField] private InventoryItemBoxUI Slot1Ice;
    [SerializeField] private InventoryItemBoxUI Slot2Fire;
    [SerializeField] private InventoryItemBoxUI Slot2Ice;

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

        //Update Currency Numbers
        if (runDataManager != null)
        {
            GoldNumUI.text = runDataManager.GoldCurrencyCollected.ToString();
            EssenceNumUI.text = runDataManager.EssenceCurrencyCollected.ToString();
        }

        //Update Stats
        foreach (InventoryStatBlessingBox box in StatBoxes)
        {
            box.AssignDisplayNumber(statManager.GetStatPointsFromStat(box.Stat));
        }
    }

    //Add Virtues and Relics
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

    //Add Abilities
    public void AddAbilityToUI(Ability ability, ElementType element, PlayerAbilitySlot slot)
    {
        GetSlotUI(slot, element).AssignItem(ability);
    }
    private InventoryItemBoxUI GetSlotUI(PlayerAbilitySlot slot, ElementType Element)
    {
        if (Element == ElementType.Fire)
            return slot == PlayerAbilitySlot.Slot1 ? Slot1Fire : Slot2Fire;
        else
            return slot == PlayerAbilitySlot.Slot1 ? Slot1Ice : Slot2Ice;
    }
}
