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
        }

        //Check if The Player Has Items/Boons Already
        foreach (var option in cachedShopSelection)
        {
            if (!option.CanBeSold()) { option.isSold = true; }
        }


        //Create Shop
        GameManager.Instance.InputUIPopup_Shop(cachedShopSelection);
        GameManager.Instance.ActivateUIPopup_Shop();
    }

    private List<ShopOption> GetSetOfItems(int num, IReadOnlyList<ShopOption> options)
    {
        if (options.Count < num) { Debug.Log("Insufficient Number of Items in Lists"); }

        List<ShopOption> ItemSet = new List<ShopOption>();
        List<ShopOption> TempOptions = new List<ShopOption>(options);

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
}


//SHOP CLASSES
[System.Serializable]
public abstract class ShopOption
{
    protected RunDataManager GM_Rundata => GameManager.Instance.runData;
    public ShopDescription Description;
    public float ChanceToAppear;
    [HideInInspector] public bool isSold = false;
    //Application Function
    public abstract void ApplyChoice();
    public abstract bool CanBeSold();
}
[System.Serializable]
public class ShopDescription
{
    public string ItemName;
    public float ItemCost;
    public UnityEngine.UI.Image Icon;
}
[System.Serializable]
public class ShopOptionBoon : ShopOption
{
    public BaseBoon BoonRef;
    public override void ApplyChoice()
    {
        if (GameManager.Instance == null) { return; }
        GameManager.Instance.runData.AddBoon(BoonRef);
    }
    public override bool CanBeSold()
    {
        if (GM_Rundata == null || BoonRef == null) { return false; }
        if (GM_Rundata.IsBoonCollected(BoonRef))
        {
            //Boon is Collected
            if (GM_Rundata.GetBoonLevel(BoonRef) == GM_Rundata.MaxBoonLevel) { return false; }
        }
        return true;
    }
}
[System.Serializable]
public class ShopOptionItem : ShopOption
{
    public GameObject Relic;
    public override void ApplyChoice()
    {
        if (GameManager.Instance == null) { return; }
    }
    public override bool CanBeSold()
    {
        return true;
    }
}
