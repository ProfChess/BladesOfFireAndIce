using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform AllShopOptionsParentObject;
    [SerializeField] private TextMeshProUGUI PlayerCurrentCurrency;
    [SerializeField] private ShopUIEntryOption ShopEntryPrefab;
    [SerializeField] private List<ShopUIEntryOption> reserveShopUIOptions = new List<ShopUIEntryOption>();
    public ShopItemDetailsUI shopItemDetails;
    private RunDataManager RDM => GameManager.Instance.runData;

    //Display Given Options no Matter the Size, and Update the Players Gold Amount Display
    public void PopulateShopOptions(List<ShopOption> Options)
    {
        for (int i = 0; i < Options.Count; i++)
        {
            if (i >= reserveShopUIOptions.Count)
            {
                ShopUIEntryOption newUIOption = Instantiate(ShopEntryPrefab, AllShopOptionsParentObject);
                reserveShopUIOptions.Add(newUIOption);
            }
            reserveShopUIOptions[i].gameObject.SetActive(true);
            reserveShopUIOptions[i].Assign(Options[i]);
        }
        ReEvalutateShop();
    }

    public void ReEvalutateShop()
    {
        for (int i = 0; i < reserveShopUIOptions.Count; i++)
        {
            reserveShopUIOptions[i].CheckEntryStatus();
        }
        ApplyCurrencyNumChange();
    }

    public void ApplyCurrencyNumChange()
    {
        PlayerCurrentCurrency.text = RDM.GoldCurrencyCollected.ToString();
    }
    public void AssignItemDetailsFromShopOption(ShopOption shopItem)
    {
        foreach (var itemEntry in reserveShopUIOptions) { itemEntry.DeselectItem(); }

        if (shopItem.item is Virtue shopVirtue)
        {
            int level = RDM.GetVirtueLevel(shopVirtue) + 1;
            List<StatDisplayEntry> statDisplay = level < 2 ?
                shopItem.item.GetListOfStatsForDisplay() : shopItem.item.GetLeveledStatsPreview();

            shopItemDetails.AssignItemDetails(
            shopItem.item.BonusName,
            shopItem.item.BonusDescription,
            shopItem.item.type,
            statDisplay,
            level
            );
        }
        else
        {
            shopItemDetails.AssignItemDetails(
            shopItem.item.BonusName,
            shopItem.item.BonusDescription,
            shopItem.item.type,
            shopItem.item.GetListOfStatsForDisplay());
        }

    }
    private void OnEnable()
    {
        shopItemDetails.ClearSelection();
        EventSystem.current.SetSelectedGameObject(null);
    }
}
