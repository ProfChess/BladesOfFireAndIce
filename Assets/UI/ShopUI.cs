using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform AllShopOptionsParentObject;
    [SerializeField] private TextMeshProUGUI PlayerCurrentCurrency;
    [SerializeField] private ShopUIEntryOption ShopEntryPrefab;
    [SerializeField] private List<ShopUIEntryOption> reserveShopUIOptions = new List<ShopUIEntryOption>();


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
        PlayerCurrentCurrency.text = GameManager.Instance.runData.GoldCurrencyCollected.ToString();
    }
}
