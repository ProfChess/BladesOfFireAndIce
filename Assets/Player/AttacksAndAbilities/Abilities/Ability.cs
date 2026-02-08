using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(menuName =("PlayerAbilities/Ability"))]
public class Ability : ScriptableObject
{
    [Header("Ability Info")]
    public string AbilityName;
    [TextArea] public string Description;

    [Tooltip("Effect This Ability Will Use")]
    public PlayerAbilityType EffectToTrigger;

    [Tooltip("Element Stance the Player Must be in to Use This Ability")]
    public ElementType StanceForEffect;

    public Action GetEffect()
    {
        return Effect;
    }
    private void Effect()
    {
        AbilityEffectLibrary.PlayAbilityEffect(EffectToTrigger);
    }

}
