using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    private int baseShopSize = 2;
    public override void Interact()
    {
        //Decide Size of Shop
        int ShopSize = 0;
        ShopSize = GameManager.Instance.difficultyManager.ShopSize;
        if (ShopSize == 0) { ShopSize = baseShopSize; }

        //Get Set of Items for The Shop
        List<ShopOption> itemsForSale = new List<ShopOption>();
        itemsForSale = GetSetOfItems(ShopSize, shopOptions);

        //Create Shop
        GameManager.Instance.InputUIPopup_Shop(itemsForSale);
        GameManager.Instance.ActivateUIPopup_Shop();
    }
    private void Awake()
    {
        shopOptions = CreateShopOptionList();
    }

    //Items That Can be Sold
    private List<ShopOption> shopOptions;
    [SerializeField] private List<ShopOptionBoon> boonShopOptions = new List<ShopOptionBoon>();
    [SerializeField] private List<ShopOptionItem> relicShopOptions = new List<ShopOptionItem>();

    private List<ShopOption> GetSetOfItems(int num, List<ShopOption> options)
    {
        if (options.Count < num) { Debug.Log("Insufficient Number of Items in Lists"); }

        List<ShopOption> ItemSet = new List<ShopOption>();
        List<ShopOption> TempOptions = options;

        for (int i = 0; i < num; i++)
        {
            ShopOption item = GetRandomItem(TempOptions);
            TempOptions.Remove(item);
            ItemSet.Add(item);
        }
        return ItemSet;
    }
    private List<ShopOption> CreateShopOptionList()
    {
        List<ShopOption> FullOptions = new List<ShopOption>();
        foreach (ShopOptionBoon item in boonShopOptions)
        {
            FullOptions.Add(item);
        }
        foreach (ShopOptionItem item in relicShopOptions)
        {
            FullOptions.Add(item);
        }
        return FullOptions;
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
        return shopOptions[0];
    }
}


//SHOP CLASSES
[System.Serializable]
public class ShopOptionBoon : ShopOption
{
    public BaseBoon BoonRef;
}
[System.Serializable]
public class ShopOptionItem : ShopOption
{
    public GameObject Relic;
}
[System.Serializable]
public class ShopOption
{
    public ShopDescription Description;
    public float ChanceToAppear;
}
[System.Serializable]
public class ShopDescription
{
    public string ItemName;
    public float ItemCost;
    public UnityEngine.UI.Image Icon;
}
