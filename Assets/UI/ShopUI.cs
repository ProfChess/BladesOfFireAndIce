using UnityEngine;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [SerializeField] List<ShopUIEntryOption> shopUIOptions = new List<ShopUIEntryOption>();
    public void PopulateShopOptions(List<ShopOption> Options)
    {
        for (int i = 0; i < Options.Count; i++)
        {
            shopUIOptions[i].Assign(Options[i]);
        }
    }
}
