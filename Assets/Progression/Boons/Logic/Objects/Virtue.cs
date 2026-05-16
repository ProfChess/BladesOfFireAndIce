using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Virtues/Virtue"))]
public class Virtue : BaseBoon
{
    //Access
    protected int Level => runData.GetVirtueLevel(this);
    [SerializeField] private StatsToDisplay DisplayStats;

    [Tooltip("Effect of This Boon")]
    public DamageBoonEffectType EffectType;

    [Tooltip("Effect will Use This value for Spawning so Long as It is Available")]
    public EffectOriginType EffectOrigin;

    //Damage of Effect
    [Header("Boon Attributes")]
    public EffectBaseStats BaseStats;
    [Header("")]
    public EffectLevelScalers LevelScalers;

    public override void Effect(PlayerEventContext AttackDetails)
    {
        if (runData.CanVirtueTrigger(this))
        {
            BoonEffectLibrary.PlayBoonEffect(this, AttackDetails);
            runData.BeginVirtueCooldown(this);
        }
    }
    public override void BoonCollected()
    {
        runData.AddVirtue(this);
    }

    public BoonLeveledStats GetLeveledStats(int Level)
    {
        return new BoonLeveledStats
        {
            FinalDamage = BaseStats.Damage * Mathf.Pow(LevelScalers.DamageScale, Level - 1),
            FinalTriggerCount = (int)(BaseStats.TriggerCount * Mathf.Pow(LevelScalers.TriggerCountScale, Level - 1)),
            FinalEffectSize = BaseStats.EffectSize * Mathf.Pow(LevelScalers.EffectSizeScale, Level - 1),
            FinalEffectDuration = BaseStats.EffectDuration * Mathf.Pow(LevelScalers.EffectDurationScale, Level - 1),
            FinalSpawnCount = (int)(BaseStats.SpawnCount * Mathf.Pow(LevelScalers.SpawnCountScale, Level - 1)),
            FinalProjSpeed = BaseStats.ProjSpeed * Mathf.Pow(LevelScalers.ProjSpeedScale, Level - 1),
            FinalProjLifetime = BaseStats.ProjLifetime * Mathf.Pow(LevelScalers.ProjSpeedScale, Level - 1),
        };
    }

    //UI Display
    public override void DisplayDetailsOfBonusInInventory(InventoryDescriptionUI inventoryDesc)
    {
        int level = GameManager.Instance.runData.GetVirtueLevel(this);
        inventoryDesc.AssignTextFromItem(BonusName, type.ToString(), BonusDescription, level);
    }
    public override List<StatDisplayEntry> GetListOfStatsForDisplay()
    {
        int level = 1;
        if (GameManager.Instance.runData.IsVirtueCollected(this))
        {
            level = GameManager.Instance.runData.GetVirtueLevel(this);
        }
        BoonLeveledStats stats = GetLeveledStats(level);

        List<StatDisplayEntry> Results = new();
        foreach (PlayerEffectStatType type in Enum.GetValues(typeof(PlayerEffectStatType)))
        {
            float value = GetLeveledStatFromStatName(type, stats);
            ActivePlayerEffectHelpers.AddStatIfRelevant(type, DisplayStats, Results, value);
        }
        return Results;
    }
    //returns value of stat given the enum
    private float GetLeveledStatFromStatName(PlayerEffectStatType stat, BoonLeveledStats statCollection)
    {
        switch (stat)
        {
            case PlayerEffectStatType.Damage:         return statCollection.FinalDamage;
            case PlayerEffectStatType.TriggerCount:   return statCollection.FinalTriggerCount;
            case PlayerEffectStatType.EffectSize:     return statCollection.FinalEffectSize;
            case PlayerEffectStatType.EffectDuration: return statCollection.FinalEffectDuration;
            case PlayerEffectStatType.SpawnCount:     return statCollection.FinalSpawnCount;
            case PlayerEffectStatType.Cooldown:       return BaseStats.Cooldown;
            case PlayerEffectStatType.ProjSpeed:      return statCollection.FinalProjSpeed;
            case PlayerEffectStatType.ProjLifetime:   return statCollection.FinalProjLifetime;
        }
        return 0f;
    }
}
//Boon Type Specific Enum
public enum DamageBoonEffectType { FireBoom, FireBurst, IceBoom}
public enum EffectOriginType { Player, Target, Attack}
public enum PlayerEffectStatType { Damage, TriggerCount, EffectSize, EffectDuration, SpawnCount, Cooldown, ProjSpeed, ProjLifetime }

//Base Stats
[System.Serializable]
public class EffectBaseStats
{
    //Base Values Each Boon Might Use
    [Header("All Boons")]
    [Tooltip("Base Damage of Boon Effect")]
    public float Damage = 0f;
    [Tooltip("Size of Effect")]
    public float EffectSize = 1f;
    [Tooltip("Number of Times Effect is Triggered After Event")]
    public int TriggerCount = 1;
    [Tooltip("Duration of Timed Effects")]
    public float EffectDuration = 1f;
    [Tooltip("Number of Effect Objects For Each Effect Triggered")]
    public int SpawnCount = 1;
    [Tooltip("Time Before Effect Can be Triggered Again")]
    public float Cooldown = 0f;

    [Header("Projectile Specific")]
    [Tooltip("Speed the Projectile Travels")]
    public float ProjSpeed = 1f;
    [Tooltip("Time the Proj Lasts")]
    public float ProjLifetime = 1f;
}
//Leveling Values
[System.Serializable]
public class EffectLevelScalers
{
    //Multipliers on Base Values
    public float DamageScale = 1f;
    public float EffectSizeScale = 1f;
    public float TriggerCountScale = 1f;
    public float EffectDurationScale = 1f;
    public float SpawnCountScale = 1;
    public float ProjSpeedScale = 1f;
    public float ProjLifetimeScale = 1f;
}
public struct BoonLeveledStats
{
    //Multipliers on Base Values
    public float FinalDamage;
    public float FinalEffectSize;
    public int FinalTriggerCount;
    public float FinalEffectDuration;
    public int FinalSpawnCount;
    public float FinalProjSpeed;
    public float FinalProjLifetime;
}
[System.Serializable]
public class StatsToDisplay
{
    public bool Damage;
    public bool EffectSize;
    public bool TriggerCount;
    public bool EffectDuration;
    public bool SpawnCount;
    public bool Cooldown;
    public bool ProjSpeed;
    public bool ProjLifetime;
}