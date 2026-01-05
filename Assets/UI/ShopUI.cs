using UnityEngine;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{

    [SerializeField] private Transform AllShopOptionsObject;
    [SerializeField] private List<ShopUIEntryOption> shopUIOptions = new List<ShopUIEntryOption>();

    private void Awake()
    {
        shopUIOptions = new List<ShopUIEntryOption>(AllShopOptionsObject.GetComponentsInChildren<ShopUIEntryOption>(true));
    }
    public void PopulateShopOptions(List<ShopOption> Options)
    {
        for (int i = 0; i < Options.Count; i++)
        {
            shopUIOptions[i].Assign(Options[i]);
        }
    }
}
