using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDataManager : MonoBehaviour
{
    //TEMPORARY DATA FOR EACH RUN THROUGH THE DUNGEON
    [field: SerializeField] public int MaxBoonLevel {  get; private set; }
    //Relics
    //BOONS
    private Dictionary<BaseBoon, int> BoonLevels = new Dictionary<BaseBoon, int>();
    public int GetBoonLevel(BaseBoon Boon) {  return BoonLevels[Boon]; }
    //Does The Player Already Have This Boon
    public bool IsBoonCollected(BaseBoon Boon) { return BoonLevels.ContainsKey(Boon); }
    public void AddBoon(BaseBoon Boon) 
    {
        //If We Have Not Collected the Boon So Far, Add it And Attach the Effect to the Event
        if (!IsBoonCollected(Boon))
        {
            BoonLevels.Add(Boon, 1);
            Boon.BoonSelected();
        }
        else if (BoonLevels[Boon] < MaxBoonLevel)
        {
            BoonLevels[Boon] = 2;
            //Increases Bonus Gained From Stat-Increasing Boons
            if (Boon is NumberBoostBoon StatBoon) { StatBoon.IncreaseBonus(); }
            Debug.Log("Boon Level Increased To: " +  BoonLevels[Boon]);
        }
    }

    //CURRENCY
    [field: SerializeField] public float ShopCurrencyCollected { get; private set; } = 100f;
    //private float StatCurrencyCollected = 0f;
    public void AddShopCurrency(float num) {  ShopCurrencyCollected += num; }
    public void SubShopCurrency(float num) { ShopCurrencyCollected -= num; }
    public bool CanPayItemCost(float num) { return ShopCurrencyCollected >= num; }


    public void ClearRunData()
    {
        BoonLevels.Clear();
        ShopCurrencyCollected = 0f;
    }
}
