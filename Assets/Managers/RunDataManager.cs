using System;
using System.Collections.Generic;
using UnityEngine;

public class RunDataManager : MonoBehaviour
{
    //TEMPORARY DATA FOR EACH RUN THROUGH THE DUNGEON
    [field: SerializeField] public int MaxBoonLevel {  get; private set; }
    [field: SerializeField] public float GoldCurrencyCollected { get; private set; } = 100f;
    [field: SerializeField] public float EssenceCurrencyCollected { get; private set; } = 0f;
    [SerializeField] private Inventory InventoryUI;

    //Relics
    private Dictionary<Rune, RuneInstance> CurrentRunes = new();
    public bool isRuneApplied(Rune relic) { return CurrentRunes[relic].isActive; }
    public bool isRuneCollected(Rune relic) { return CurrentRunes.ContainsKey(relic); }
    public void AddRune(Rune relic)
    {
        if (!isRuneCollected(relic))
        {
            RuneInstance instance = new RuneInstance
            {
                RelicRef = relic,
                isActive = false,
                isTimed = relic.Duration > 0
            };
            CurrentRunes.Add(relic, instance);
            relic.BoonSelected();

            //UI
            InventoryUI.AddRuneToInventory(relic);
        }
    }
    public void RuneActivated(Rune relic, Action DisableEffect, float Duration = 0f)
    {
        if (isRuneCollected(relic) && !isRuneApplied(relic))
        {
            CurrentRunes[relic].isActive = true;
            CurrentRunes[relic].DurationOfBuff = Duration;
            CurrentRunes[relic].DisableEffect = DisableEffect;

            Debug.Log("Rune Bonus Activated");
        }
    }

    public void RuneDeactivated(Rune relic)
    {
        if (isRuneCollected(relic) && isRuneApplied(relic))
        {
            CurrentRunes[relic].isActive = false;
            CurrentRunes[relic].DisableEffect?.Invoke();
            
            Debug.Log("Relic Bonus Deactivated");
        }
    }
    public bool CanRuneTrigger(Rune relic)
    {
        if (!isRuneCollected(relic)) { return false; }
        return CurrentRunes[relic].CanTrigger;
    }
    public void BeginRuneCooldown(Rune relic)
    {
        if (!isRuneCollected(relic)) { return; }
        CurrentRunes[relic].StartCooldown();
    }
    public float GetRuneBuffTimeRemaining(Rune relic)
    {
        if (!isRuneCollected(relic) && CurrentRunes[relic].isTimed)
        {
            return CurrentRunes[relic].DurationOfBuff;
        }
        else
        {
            return 0f;
        }
    }
    private void ReapplyAllRunesOnSceneChange()
    {
        foreach (Rune relic in CurrentRunes.Keys)
        {
            CurrentRunes[relic].isActive = false;
            relic.BoonSelected();
            relic.BonusCollected();
        }
    }
    
    //VIRTUES
    private Dictionary<Virtue, VirtueInstance> CurrentVirtues = new();
    public int GetVirtueLevel(Virtue virtue) 
    {
        if (CurrentVirtues.ContainsKey(virtue))
        {
            return CurrentVirtues[virtue].Level;
        }
        return 0;
    }
    //Does The Player Already Have This Virtue
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

            //UI
            InventoryUI.AddVirtueToInventory(virtue);
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
    private void ReapplyAllVirtuesOnSceneChange()
    {
        foreach (Virtue virtue in CurrentVirtues.Keys)
        {
            virtue.BoonSelected();
        }
    }



    //Blessings
    private Dictionary<StatBlessing, BlessingInstance> CurrentBlessings = new();
    public bool IsBlessingSelected(StatBlessing Blessing) { if (Blessing == null) { return false; } return CurrentBlessings.ContainsKey(Blessing); }
    public void SelectBlessing(StatBlessing Blessing)
    {
        if (CurrentBlessings.ContainsKey(Blessing)) { Debug.Log("Blessing Already Selected"); return; }

        //Get List of Effects and Times
        BlessingInstance BlessingInst = new BlessingInstance
        {
            BlessingRef = Blessing,
            TimedEffects = Blessing.GetTimedEffectList()
        };

        CurrentBlessings.Add(Blessing, BlessingInst);
        Blessing.AttachBlessingTempEffects();
        Blessing.ApplyEffects();

        //Add to Inventory
        InventoryUI.AddBlessingToInventory(Blessing);
    }
    public void DeSelectBlessing(StatBlessing Blessing)
    {
        if (!CurrentBlessings.ContainsKey(Blessing)) { Debug.Log("This Blessing is Not Equipped"); return; }

        //Remove From Dictionary
        CurrentBlessings.Remove(Blessing);
        //Unsub Events
        Blessing.DeAttachBlessingTempEffects();
        //Remove Flat Bonuses
        Blessing.RemoveEffects();
    }
    public void ToggleBlessing(StatBlessing Blessing)
    {
        if (IsBlessingSelected(Blessing))
        {
            DeSelectBlessing(Blessing);
            Debug.Log("Blessing Deselected: " + Blessing.BonusName);
        }
        else
        {
            SelectBlessing(Blessing);
            Debug.Log("Blessing Selected: " + Blessing.BonusName);
        }
    }
    public void DisableAllBlessingsOfType(StatBlessing[] Blessings)
    {
        foreach (var Blessing in Blessings)
        {
            if (IsBlessingSelected(Blessing))
            {
                DeSelectBlessing(Blessing);
                Debug.Log("Blessing Deselected: " + Blessing.BonusName);
            }
        }
    }
    private void ReapplyAllBlessingsOnSceneChange()
    {
        foreach(StatBlessing blessing in CurrentBlessings.Keys)
        {
            blessing.ApplyEffects();
        }
    }
    public void BeginBlessingCooldown(StatBlessing Blessing)
    {
        if(!IsBlessingSelected(Blessing)) { return; }
        CurrentBlessings[Blessing].StartCooldown();
    }
    public bool CanBlessingTrigger(StatBlessing Blessing)
    {
        if (!IsBlessingSelected(Blessing)) { return false; }
        return CurrentBlessings[Blessing].CanTrigger;
    }
    public void BlessingEffectActive(StatBlessing Blessing, float Duration)
    {
        BlessingInstance inst = CurrentBlessings[Blessing];
        foreach (var effect in inst.TimedEffects)
        {
            effect.Duration = Duration;
            effect.IsActive = true;
        }
    }

    //Abilities
    private Dictionary<Ability, AbilityInstance> AbilityMap = new();
    private Dictionary<(PlayerAbilitySlot, ElementType), AbilityInstance> CurrentAbilitySlotMap = new();
    public bool IsAbilityCollected(Ability ability) { return AbilityMap.ContainsKey(ability); }
    public bool IsAbilitySlotAvailable() { return CurrentAbilitySlotMap.Count != Enum.GetValues(typeof(PlayerAbilitySlot)).Length; }
    public bool IsAbilitySlotOffCooldown(PlayerAbilitySlot slot, ElementType element) { return CurrentAbilitySlotMap[(slot,element)].CanTrigger; }
    public void AddAbilityPair(Ability fireAbility, Ability iceAbility)
    {
        if (IsAbilityCollected(fireAbility) || IsAbilityCollected(iceAbility)) 
        { Debug.Log("Error One or Both of These Abilities are Already Equipped"); return; }
        if (!IsAbilitySlotAvailable()) { Debug.Log("Error No Ability Slots Available"); return; }


        if (!CurrentAbilitySlotMap.ContainsKey((PlayerAbilitySlot.Slot1, ElementType.Fire)))
        {
            //First Ability Slot is Free -> Place Ability
            //Place Fire Ability
            AssignAbilityToSlot(PlayerAbilitySlot.Slot1, ElementType.Fire, fireAbility);
            //Place Ice Ability
            AssignAbilityToSlot(PlayerAbilitySlot.Slot1, ElementType.Ice, iceAbility);
        }
        else
        {
            //Second Ability Slot is Free -> Place Ability
            //Place Fire Ability
            AssignAbilityToSlot(PlayerAbilitySlot.Slot2, ElementType.Fire, fireAbility);
            //Place Ice Ability
            AssignAbilityToSlot(PlayerAbilitySlot.Slot2, ElementType.Ice, iceAbility);
        }
    }
    private void AssignAbilityToSlot(PlayerAbilitySlot slot, ElementType Element, Ability ability)
    {
        AbilityInstance abilityInstance = new AbilityInstance
        {
            AbilityRef = ability,
            AbilitySlot = slot,
            BoundElement = Element
        };
        AbilityMap.Add(ability, abilityInstance);
        CurrentAbilitySlotMap.Add((slot, Element), abilityInstance);

        //UI 
        InventoryUI.AddAbilityToUI(ability, Element, slot);
    }
    public void FindAndActivateAbility(PlayerAbilitySlot slot, ElementType element)
    {
        CurrentAbilitySlotMap[(slot, element)].AbilityRef.GetEffect().Invoke();
    }

    //Cooldowns
    private void Update()
    {
        //COOLDOWNS
        //Virtues
        CountDownList(CurrentVirtues.Values);
        //Relics
        CountDownList(CurrentRunes.Values);
        CountDownDuration(CurrentRunes.Keys);
        //Abilities
        CountDownList(AbilityMap.Values);
        //Blessings
        CountDownList(CurrentBlessings.Values);
        CountDownBlessingDuration(CurrentBlessings.Keys);
    }
    //Counts Down Cooldown of List of Equipment w/ Cooldown States
    private void CountDownList(IEnumerable<CooldownState> cooldowns)
    {
        foreach (CooldownState cooldown in cooldowns)
        {
            cooldown.CountDown(GameTimeManager.GameDeltaTime);
        }
    }
    //Counts Down Lasting Effects of Relics
    private void CountDownDuration(IEnumerable<Rune> runeList)
    {
        foreach (Rune rune in runeList)
        {
            RuneInstance relInst = CurrentRunes[rune];
            if (!relInst.isActive || !relInst.isTimed) { continue; }

            relInst.DurationOfBuff -= GameTimeManager.GameDeltaTime;

            if (relInst.DurationOfBuff <= 0f)
            {
                RuneDeactivated(rune);
            }
        }
    }
    private void CountDownBlessingDuration(IEnumerable<StatBlessing> blessingList)
    {
        foreach (StatBlessing Blessing in blessingList)
        {
            if (!CurrentBlessings.TryGetValue(Blessing, out var blessInst)) { continue; }

            blessInst = CurrentBlessings[Blessing];
            if(blessInst.TimedEffects.Count <= 0) { continue; }

            foreach (var Entry in blessInst.TimedEffects)
            {
                if (!Entry.IsActive) { continue; }

                Entry.Duration -= GameTimeManager.GameDeltaTime;
                if (Entry.Duration <= 0f)
                {
                    Entry.DisableEffect();
                    Entry.IsActive = false;
                }
            }
        }
    }



    //CURRENCY
    //Shop Functions
    public void AddShopCurrency(float num) {  GoldCurrencyCollected += num; }



    //Stat Functions
    public void AddStatCurrency(float num) { EssenceCurrencyCollected += num; }

    public void ClearRunData()
    {
        CurrentVirtues.Clear();
        CurrentRunes.Clear();
        CurrentBlessings.Clear();
        GoldCurrencyCollected = 0f;
        EssenceCurrencyCollected = 0f;
    }

    //Stat Carry Over
    public float CurrentHealth { get; private set; } = 0f;
    public float CurrentStamina { get; private set; } = 0f;
    public void StoreHealth(float hp) { CurrentHealth = hp; }
    public void StoreStamina(float stamina) { CurrentStamina = stamina; }
    public void ReapplyAllBonuses()
    {
        //Apply Virtues, Relics, Blessings
        ReapplyAllVirtuesOnSceneChange();
        ReapplyAllRunesOnSceneChange();
        ReapplyAllBlessingsOnSceneChange();
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
public class RuneInstance : CooldownState
{
    public Rune RelicRef;
    public bool isActive = false;
    public float DurationOfBuff = 0f;
    public bool isTimed = false;
    public Action DisableEffect;
    public void StartCooldown()
    {
        Start(RelicRef.Cooldown);
    }   
}
public class BlessingInstance : CooldownState
{
    public StatBlessing BlessingRef;
    public List<TimedBlessingEffect> TimedEffects;
    public void StartCooldown()
    {
        Start(BlessingRef.Cooldown);
    }
}
public class AbilityInstance : CooldownState
{
    public Ability AbilityRef;
    public PlayerAbilitySlot AbilitySlot;
    public ElementType BoundElement;

    public void StartCooldown()
    {
        Start(AbilityRef.BaseStats.Cooldown);
    }
}