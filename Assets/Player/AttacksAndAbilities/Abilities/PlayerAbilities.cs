using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //ABILITIES
    //Types
    //Events for Boons
    public static event Action<PlayerEventContext> OnAbilityUse;
    private PlayerEventContext AbilityUseDetails = new();

    public void ActivateAbility(PlayerAbilitySlot abilitySlot, ElementType CurrentElement)
    {
        if (GameManager.Instance.runData.IsAbilitySlotOffCooldown(abilitySlot, CurrentElement))
        {
            GameManager.Instance.runData.FindAndActivateAbility(abilitySlot, CurrentElement);

            //Ability Activated 
            AbilityUseDetails.Setup(CurrentElement, Vector2.right);
            OnAbilityUse?.Invoke(AbilityUseDetails);
        }
    }
}
public enum PlayerAbilityType
{
    None = 0,
    FireSmash = 1,
    IceSmash = 2,
}
public enum PlayerAbilitySlot
{
    Slot1 = 1,
    Slot2 = 2
}