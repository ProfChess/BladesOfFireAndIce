using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIStatDefinitions
{
    //Returns Display Info For Each Stat
    public static StatDisplayInfo GetInfo(StatType stat)
    {
        switch (stat)
        {
            case StatType.MaxVitality:
                break;
            case StatType.RefreshVitality:
                break;
            case StatType.MaxEndurance:
                break;
            case StatType.RefreshEndurance:
                break;
            case StatType.RollEndurance:
                break;
            case StatType.StrengthFire:
                return new StatDisplayInfo
                {
                    Name = "Fire Damage",
                    Description = "Increases Damage of Normal Attacks While In Fire Stance"
                };
            case StatType.StrengthIce:
                break;
            case StatType.ReachFire:
                break;
            case StatType.ReachIce:
                break;
            case StatType.DexterityFire:
                break;
            case StatType.DexterityIce:
                break;
            case StatType.Luck:
                break;
            case StatType.Armor:
                break;
            default:
                break;
        }
        return new StatDisplayInfo
        {
            Name = "Unrecognized",
            Description = "No Data Available For This Stat"
        };
    }
    public static StatDisplayInfo GetInfo(PlayerEffectStatType stat)
    {
        switch (stat)
        {
            case PlayerEffectStatType.Damage:
                return new StatDisplayInfo
                {
                    Name = "Power",
                    Description = "Damage Dealt To Foes"
                };
            case PlayerEffectStatType.TriggerCount:
                return new StatDisplayInfo
                {
                    Name = "Resonance",
                    Description = "Number of Activations When Triggered"
                };
            case PlayerEffectStatType.EffectSize:
                return new StatDisplayInfo
                {
                    Name = "Reach",
                    Description = "Size of The Effect"
                };
            case PlayerEffectStatType.EffectDuration:
                return new StatDisplayInfo
                {
                    Name = "Flow",
                    Description = "Duration of Damage Over Time Effects/Channeled Effects"
                };
            case PlayerEffectStatType.SpawnCount:
                return new StatDisplayInfo
                {
                    Name = "Fragments",
                    Description = "Number of Effects for Each Activation"
                };
            case PlayerEffectStatType.Cooldown:
                return new StatDisplayInfo
                {
                    Name = "Recovery",
                    Description = "Cooldown of The Effect"
                };
            case PlayerEffectStatType.ProjSpeed:
                return new StatDisplayInfo
                {
                    Name = "Momentum",
                    Description = "Projectiles Movement Speed"
                };
            case PlayerEffectStatType.ProjLifetime:
                return new StatDisplayInfo
                {
                    Name = "Persistance",
                    Description = "Time Before Projectile Dissapates"
                };
        }
        return new StatDisplayInfo
        {
            Name = "Unrecognized",
            Description = "No Data Available For This Stat"
        };
    }
}
