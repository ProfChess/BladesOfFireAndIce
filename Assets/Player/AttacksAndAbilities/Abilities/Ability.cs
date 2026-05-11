using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Abilities/Ability"))]
public class Ability : BaseAttainedBonus
{
    [SerializeField] private StatsToDisplay DisplayStats;

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

    public override void DisplayStatsOfBonusInInventory(InventoryDescriptionUI inventoryObj)
    {
        List<StatDisplayEntry> Results = new();
        foreach (PlayerEffectStatType type in Enum.GetValues(typeof(PlayerEffectStatType)))
        {
            float value = GetValueOfBaseStat(type);
            ActivePlayerEffectHelpers.AddStatIfRelevant(type, DisplayStats, Results, value);
        }
        inventoryObj.AssignStatsFromItem(Results);
    }
    //returns value of stat given the enum
    private float GetValueOfBaseStat(PlayerEffectStatType stat)
    {
        switch (stat)
        {
            case PlayerEffectStatType.Damage:           return BaseStats.Damage;
            case PlayerEffectStatType.TriggerCount:     return BaseStats.TriggerCount;
            case PlayerEffectStatType.EffectSize:       return BaseStats.EffectSize;
            case PlayerEffectStatType.EffectDuration:   return BaseStats.EffectDuration;
            case PlayerEffectStatType.SpawnCount:       return BaseStats.SpawnCount;
            case PlayerEffectStatType.Cooldown:         return BaseStats.Cooldown;
            case PlayerEffectStatType.ProjSpeed:        return BaseStats.ProjSpeed;
            case PlayerEffectStatType.ProjLifetime:     return BaseStats.ProjLifetime;
        }
        return 0f;
    }

}
