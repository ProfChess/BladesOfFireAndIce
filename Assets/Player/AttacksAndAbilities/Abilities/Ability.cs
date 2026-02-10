using System;
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

    //Stats
    [Header("Stats")]
    public EffectBaseStats BaseStats;

    public Action GetEffect()
    {
        return Effect;
    }
    private void Effect()
    {
        AbilityEffectLibrary.PlayAbilityEffect(this);
    }

}
