using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("PlayerAbilities/Ability"))]
public class Ability : ScriptableObject
{
    public PlayerAbilityType EffectToTrigger;

    public Action GetEffect()
    {
        return Effect;
    }
    private void Effect()
    {
        AbilityEffectLibrary.PlayAbilityEffect(EffectToTrigger);
    }

}
