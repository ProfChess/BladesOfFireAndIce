using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityEffectLibrary
{
    public static void PlayAbilityEffect(PlayerAbilityType AbilityEffect)
    {
        switch (AbilityEffect)
        {
            case PlayerAbilityType.None: return;
            case PlayerAbilityType.FireSmash: Ability_FireSmash(); return;
        }
    }
    private static void Ability_FireSmash()
    {

    }
  
}
