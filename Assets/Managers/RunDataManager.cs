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
                isActive = false,
                isTimed = relic.Duration > 0
            };
            CurrentRelics.Add(relic, instance);
            relic.BoonSelected();
        }
    }
    public void RelicActivated(Relic relic, Action DisableEffect, float Duration = 0f)
    {
        if (isRelicCollected(relic) && !isRelicApplied(relic))
        {
            CurrentRelics[relic].isActive = true;
            CurrentRelics[relic].DurationOfBuff = Duration;
            CurrentRelics[relic].DisableEffect = DisableEffect;

            Debug.Log("Relic Bonus Activated");
        }
    }

    public void RelicDeactivated(Relic relic)
    {
        if (isRelicCollected(relic) && isRelicApplied(relic))
        {
            CurrentRelics[relic].isActive = false;
            CurrentRelics[relic].DisableEffect?.Invoke();
            
            Debug.Log("Relic Bonus Deactivated");
        }
    }
    public bool CanRelicTrigger(Relic relic)
    {
        if (!isRelicCollected(relic)) { return false; }
        return CurrentRelics[relic].CanTrigger;
    }
    public void BeginRelicCooldown(Relic relic)
    {
        if (!isRelicCollected(relic)) { return; }
        CurrentRelics[relic].StartCooldown();
    }
    public float GetRelicBuffTimeRemaining(Relic relic)
    {
        if (!isRelicCollected(relic) && CurrentRelics[relic].isTimed)
        {
            return CurrentRelics[relic].DurationOfBuff;
        }
        else
        {
            return 0f;
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
        CurrentVirtues[virtue].StartCooldown();
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
        CountDownDuration(CurrentRelics.Keys);
    }
    private void CountDownList(IEnumerable<CooldownState> cooldowns)
    {
        foreach (CooldownState cooldown in cooldowns)
        {
            cooldown.CountDown(GameTimeManager.GameDeltaTime);
        }
    }
    private void CountDownDuration(IEnumerable<Relic> relicList)
    {
        foreach (Relic relic in relicList)
        {
            RelicInstance relInst = CurrentRelics[relic];
            if (!relInst.isActive || !relInst.isTimed) { continue; }

            CurrentRelics[relic].DurationOfBuff -= GameTimeManager.GameDeltaTime;

            if (relInst.DurationOfBuff <= 0f)
            {
                RelicDeactivated(relic);
            }
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
    public float RemainingCooldown = 0f;
    public bool CanTrigger => RemainingCooldown <= 0f;
    public void Start(float Duration)
    {
        RemainingCooldown = Duration;
    }

    public void CountDown(float dt)
    {
        if (RemainingCooldown > 0f)
        {
            RemainingCooldown -= dt;
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
    public float DurationOfBuff = 0f;
    public bool isTimed = false;
    public Action DisableEffect;
    public void StartCooldown()
    {
        Start(RelicRef.Cooldown);
    }
    
}
