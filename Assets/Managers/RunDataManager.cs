using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDataManager : MonoBehaviour
{
    //TEMPORARY DATA FOR EACH RUN THROUGH THE DUNGEON
    [field: SerializeField] public int MaxBoonLevel {  get; private set; }
    //Relics
    private Dictionary<Relic, bool> AppliedRelics = new();
    public bool isRelicApplied(Relic relic) { return AppliedRelics[relic]; }
    public bool isRelicCollected(Relic relic) { return AppliedRelics.ContainsKey(relic); }
    public void AddRelic(Relic relic)
    {
        if (!isRelicCollected(relic))
        {
            AppliedRelics.Add(relic, false);
        }
    }
    public void RelicActivated(Relic relic)
    {
        if (isRelicCollected(relic) && !isRelicApplied(relic))
        {
            AppliedRelics[relic] = true;
            Debug.Log("Relic Bonus Activated");
        }
    }
    public void RelicDeactivated(Relic relic)
    {
        if (isRelicCollected(relic) && isRelicApplied(relic))
        {
            AppliedRelics[relic] = false;
            Debug.Log("Relic Bonus Deactivated");
        }
    }
    //BOONS
    private Dictionary<BaseBoon, int> BoonLevels = new();
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
            Debug.Log("Boon Level Increased To: " +  BoonLevels[Boon]);
        }
    }



    //CURRENCY
    [field: SerializeField] public float ShopCurrencyCollected { get; private set; } = 100f;
    [field: SerializeField] public float StatCurrencyCollected { get; private set; } = 0f;
    //Shop Functions
    public void AddShopCurrency(float num) {  ShopCurrencyCollected += num; }
    public bool CanPayItemCost(float num) { return ShopCurrencyCollected >= num; }

    //Stat Functions
    public void AddStatCurrency(float num) { StatCurrencyCollected += num; }

    public void ClearRunData()
    {
        BoonLevels.Clear();
        ShopCurrencyCollected = 0f;
        StatCurrencyCollected = 0f;
    }
}
