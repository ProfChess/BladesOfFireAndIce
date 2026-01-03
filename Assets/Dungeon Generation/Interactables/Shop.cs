using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : InteractableObject
{
    public override void Interact()
    {
        //Display Shop UI
    }

    //Items That Can be Sold
    [SerializeField] private List<ShopOption> shopOptions = new List<ShopOption>();
    private List<ShopOption> GetSetOfItems(int num)
    {
        List<ShopOption> ItemSet = new List<ShopOption>();
        for (int i = 0; i < num; i++)
        {
            ShopOption item = GetRandomItem();
            ItemSet.Add(item);
        }
        return ItemSet;
    }
    private ShopOption GetRandomItem()
    {
        //Get Total Chance
        float TotalChance = 0f;
        for (int i = 0; i < shopOptions.Count; i++)
        {
            TotalChance += shopOptions[i].ChanceToAppear;
        }
        if(TotalChance <= 0f) { Debug.Log("ShopOption Chances Have Not Been Set"); return null; }

        //Decide on Item
        float ChosenItem = Random.Range(0f, TotalChance);
        float cumulativeChance = 0f;
        foreach (ShopOption item in shopOptions)
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

public class ShopOption
{
    public string ItemName;
    public float ItemCost;
    //ICON
    public ShopItemType ItemType;
    public float ChanceToAppear;
}
public enum ShopItemType { Boon, Buff}