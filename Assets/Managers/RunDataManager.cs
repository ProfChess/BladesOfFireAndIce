using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDataManager : MonoBehaviour
{
    //TEMPORARY DATA FOR EACH RUN THROUGH THE DUNGEON
    [field: SerializeField] public int MaxBoonLevel {  get; private set; }
    //Relics
    private Dictionary<Relic, RelicInstance> CurrentRelics = new();
    public bool isRelicApplied(Relic relic) { return CurrentRelics[relic].isActive; }
    public bool isRelicCollected(Relic relic) { return CurrentRelics.ContainsKey(relic); }
    public void AddRelic(Relic relic)
    {
        if (!isRelicCollected(relic))
        {
            RelicInstance instance = new RelicInstance
            {
                RelicRef = relic,
                isActive = false
            };
            CurrentRelics.Add(relic, instance);
            relic.BoonSelected();
        }
    }
    public void RelicActivated(Relic relic)
    {
        if (isRelicCollected(relic) && !isRelicApplied(relic))
        {
            CurrentRelics[relic].isActive = true;
            Debug.Log("Relic Bonus Activated");
        }
    }
    public void RelicDeactivated(Relic relic)
    {
        if (isRelicCollected(relic) && isRelicApplied(relic))
        {
            CurrentRelics[relic].isActive = false;
            Debug.Log("Relic Bonus Deactivated");
        }
    }
    
    
    //BOONS
    private Dictionary<Virtue, VirtueInstance> CurrentVirtues = new();
    public int GetVirtueLevel(Virtue virtue) {  return CurrentVirtues[virtue].Level; }
    //Does The Player Already Have This Boon
    public bool IsVirtueCollected(Virtue virtue) { return CurrentVirtues.ContainsKey(virtue); }
    public void AddVirtue(Virtue virtue) 
    {
        //If We Have Not Collected the Boon So Far, Add it And Attach the Effect to the Event
        if (!IsVirtueCollected(virtue))
        {
            VirtueInstance instance = new VirtueInstance
            {
                VirtueRef = virtue,
                Level = 1,
            };
            CurrentVirtues.Add(virtue, instance);
            virtue.BoonSelected();
        }
        else if (CurrentVirtues[virtue].Level < MaxBoonLevel)
        {
            CurrentVirtues[virtue].Level = 2;
        }
    }
    public void BeginVirtueCooldown(Virtue virtue)
    {
        if (!IsVirtueCollected(virtue)) return;
        CurrentVirtues[virtue].Start(virtue.BaseStats.Cooldown);
    }
    public bool CanVirtueTrigger(Virtue virtue)
    {
        if (!IsVirtueCollected(virtue)) return false;

        return CurrentVirtues[virtue].CanTrigger;
    }

    private void Update()
    {
        //COOLDOWNS
        //Virtues
        CountDownList(CurrentVirtues.Values);
        //Relics
        CountDownList(CurrentRelics.Values);
    }
    private void CountDownList(IEnumerable<CooldownState> cooldowns)
    {
        foreach (CooldownState cooldown in cooldowns)
        {
            cooldown.CountDown(GameTimeManager.GameTime);
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
        CurrentVirtues.Clear();
        CurrentRelics.Clear();
        ShopCurrencyCollected = 0f;
        StatCurrencyCollected = 0f;
    }
}

//Classes for Boon/Reic/Ability Instances
public class CooldownState
{
    public float RemainingTime = 0f;
    public bool CanTrigger => RemainingTime <= 0f;
    public void Start(float Duration)
    {
        RemainingTime = Duration;
    }

    public void CountDown(float dt)
    {
        if (RemainingTime > 0f)
        {
            RemainingTime -= dt;
        }
    }
}
public class VirtueInstance : CooldownState
{
    public Virtue VirtueRef;
    public int Level = 0;

    public void StartCooldown()
    {
        Start(VirtueRef.BaseStats.Cooldown);
    }
}
public class RelicInstance : CooldownState
{
    public Relic RelicRef;
    public bool isActive = false;

    public void StartCooldown()
    {
        Start(RelicRef.BaseStats.Cooldown);
    }
}
