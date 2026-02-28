using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //ABILITIES
    //Types
    private Dictionary<(PlayerAbilitySlot AbilitySlot, ElementType BoundElement), AbilityEntry> ActiveAbilities = new();

    //Events for Boons
    public static event Action<PlayerEventContext> OnAbilityUse;
    private PlayerEventContext AbilityUseDetails = new();

    //Assign and Call abilities by Type (Possibilities for Endless Mode)
    public void AssignAbility(PlayerAbilitySlot SlotNum, ElementType Element, Action AbilityEffect, float AbilityCooldown)
    {
        if (ActiveAbilities.ContainsKey((SlotNum, Element)))
        {
            //REPLACE WITH REPLACING ABILITY LATER MAYBE
            Debug.Log("Error: Ability Spot Already Taken");
            return;
        }
        AbilityEntry Entry = new AbilityEntry { Effect = AbilityEffect, Cooldown = AbilityCooldown, CooldownRemaining = 0f };
        ActiveAbilities.Add((SlotNum, Element), Entry);
    }
    public bool IsAbilitySlotTaken(PlayerAbilitySlot abilitySlot)
    {
        //SINCE FIRE AND ICE ARE ALWAYS SELECTED TOGETHER, IF NO FIRE -> NO ABILITY HAS BEEN SELECTED
        if (ActiveAbilities.ContainsKey((abilitySlot, ElementType.Fire))) { Debug.Log("True"); }
        return ActiveAbilities.ContainsKey((abilitySlot, ElementType.Fire));
    }
    public void ActivateAbility(PlayerAbilitySlot abilitySlot, ElementType CurrentElement)
    {
        if (!ActiveAbilities.ContainsKey((abilitySlot, CurrentElement))) { return; }

        if (ActiveAbilities[(abilitySlot, CurrentElement)].CooldownRemaining <= 0f)
        {
            Debug.Log("Yup");
            //Ability Activated 
            AbilityUseDetails.Setup(CurrentElement, Vector2.right);
            OnAbilityUse?.Invoke(AbilityUseDetails);

            //Effect
            ActiveAbilities[(abilitySlot, CurrentElement)].Effect?.Invoke();

            //Start Cooldown
            ActiveAbilities[(abilitySlot, CurrentElement)].CooldownRemaining = ActiveAbilities[(abilitySlot, CurrentElement)].Cooldown;
        }
    }


    //Count Down Cooldowns
    private void Update()
    {
        foreach (AbilityEntry entry in ActiveAbilities.Values)
        {
            if (entry.CooldownRemaining > 0f)
            {
                entry.CooldownRemaining -= GameTimeManager.GameDeltaTime;
            }
        }
    }
}
public class AbilityEntry
{
    public Action Effect;
    public float Cooldown = 0f;
    public float CooldownRemaining = 0f;
    public void StartCooldown() { CooldownRemaining = Cooldown; }
    public void EndCooldown() { CooldownRemaining = 0f; }
}
public enum PlayerAbilityType
{
    None = 0,
    FireSmash = 1,
    IceSmash = 2,
}
public enum PlayerAbilitySlot
{
    None = 0,
    Slot1 = 1,
    Slot2 = 2
}