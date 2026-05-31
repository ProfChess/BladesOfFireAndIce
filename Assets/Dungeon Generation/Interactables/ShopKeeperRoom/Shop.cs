using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    private int baseShopSize = 2;
    private bool shopCreated = false;
    private List<ShopOption> cachedShopSelection;
    [SerializeField] ShopDataCreation ShopData;
    public override void Interact()
    {
        //Create Shop if First Interaction
        if (!shopCreated)
        {
            //Decide Size of Shop
            int ShopSize = 0;
            ShopSize = GameManager.Instance.difficultyManager.ShopSize;
            if (ShopSize == 0) { ShopSize = baseShopSize; }

            //Create Selection
            cachedShopSelection = GetSetOfItems(ShopSize, ShopData.ShopOptions);

            shopCreated = true;
            Debug.Log("Shop Created");
        }

        //Create Shop
        GameManager.Instance.uiManager.InputUIPopup_Shop(cachedShopSelection);
        GameManager.Instance.uiManager.ActivateUIPopup_Shop();
    }

    private List<ShopOption> GetSetOfItems(int num, IReadOnlyList<ShopOption> options)
    {
        if (options.Count < num) { Debug.Log("Insufficient Number of Items in Lists"); }

        List<ShopOption> ItemSet = new List<ShopOption>();
        List<ShopOption> TempOptions = FilterFirstTimeList(options);

        for (int i = 0; i < num; i++)
        {
            ShopOption item = GetRandomItem(TempOptions);
            TempOptions.Remove(item);
            ItemSet.Add(item);
        }
        return ItemSet;
    }
    private ShopOption GetRandomItem(List<ShopOption> Options)
    {
        //Get Total Chance
        float TotalChance = 0f;
        for (int i = 0; i < Options.Count; i++)
        {
            TotalChance += Options[i].ChanceToAppear;
        }
        if(TotalChance <= 0f) { Debug.Log("ShopOption Chances Have Not Been Set"); return null; }

        //Decide on Item
        float ChosenItem = Random.Range(0f, TotalChance);
        float cumulativeChance = 0f;
        foreach (ShopOption item in Options)
        {
            cumulativeChance += item.ChanceToAppear;
            if (cumulativeChance > ChosenItem)
            {
                return item;
            }
        }

        //Default
        Debug.Log("Failed to Select Shop Option");
        return Options.Count > 0 ? Options[0] : null;
    }
    private List<ShopOption> FilterFirstTimeList(IReadOnlyList<ShopOption> options)
    {
        List<ShopOption> filteredOptions = new();
        foreach (var option in options)
        {
            if (option.ChanceToAppear > 0 && option.item.isItemAvailable()) { filteredOptions.Add(option); }
        }
        return filteredOptions;
    }
}
[System.Serializable]
public class ShopOption
{
    public ShopEligibleBonus item;
    public float itemCost;
    public float ChanceToAppear;
    public bool CanAfford(float amount) { return amount >= itemCost; }
}
