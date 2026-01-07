using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Transform AllShopOptionsObject;
    [SerializeField] private Text PlayerCurrentCurrency;
    private List<ShopUIEntryOption> shopUIOptions = new List<ShopUIEntryOption>();

    private void Awake()
    {
        shopUIOptions = new List<ShopUIEntryOption>(AllShopOptionsObject.GetComponentsInChildren<ShopUIEntryOption>(true));
    }
    public void PopulateShopOptions(List<ShopOption> Options)
    {
        //Fill Shop Options
        for (int i = 0; i < Options.Count; i++)
        {
            shopUIOptions[i].Assign(Options[i]);
        }
        //Show Player Current Currency Amount
        ReEvalutateShop();
    }

    public void ReEvalutateShop()
    {
        for (int i = 0; i < shopUIOptions.Count; i++)
        {
            shopUIOptions[i].CheckEntryStatus();
        }
        ApplyCurrencyNumChange();
    }

    public void ApplyCurrencyNumChange()
    {
        PlayerCurrentCurrency.text = GameManager.Instance.runData.ShopCurrencyCollected.ToString();
    }
}
