using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //ABILITIES
    //Types
    private Dictionary<(PlayerAbilitySlot AbilitySlot, ElementType BoundElement), Action> ActiveAbilities = new();

    //Assign and Call abilities by Type (Possibilities for Endless Mode)
    public void AssignAbility(PlayerAbilitySlot SlotNum, ElementType Element, Action Effect)
    {
        if (ActiveAbilities.ContainsKey((SlotNum, Element)))
        {
            //REPLACE WITH REPLACING ABILITY LATER MAYBE
            Debug.Log("Error: Ability Spot Already Taken");
            return;
        }
        ActiveAbilities.Add((SlotNum, Element), Effect);
    }
    public bool IsAbilitySlotTaken(PlayerAbilitySlot abilitySlot)
    {
        //SINCE FIRE AND ICE ARE ALWAYS SELECTED TOGETHER, IF NO FIRE -> NO ABILITY HAS BEEN SELECTED
        return ActiveAbilities.ContainsKey((abilitySlot, ElementType.Fire));
    }
    public void ActivateAbility(PlayerAbilitySlot abilitySlot, ElementType CurretElement)
    {
        ActiveAbilities[(abilitySlot, CurretElement)]?.Invoke();
    }
}
public enum PlayerAbilityType
{
    None = 0,
    FireSmash = 1,
}
public enum PlayerAbilitySlot
{
    None = 0,
    Slot1 = 1,
    Slot2 = 2
}