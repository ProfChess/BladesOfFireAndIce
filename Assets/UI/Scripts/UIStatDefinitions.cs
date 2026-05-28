using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIStatDefinitions
{
    //Returns Display Info For Each Stat
    public static ItemDisplayInfo GetInfo(StatType stat)
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
                return new ItemDisplayInfo
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
        return new ItemDisplayInfo
        {
            Name = "Unrecognized",
            Description = "No Data Available For This Stat"
        };
    }
    public static ItemDisplayInfo GetInfo(PlayerEffectStatType stat)
    {
        switch (stat)
        {
            case PlayerEffectStatType.Damage:
                return new ItemDisplayInfo
                {
                    Name = "Power",
                    Description = "Damage Dealt To Foes"
                };
            case PlayerEffectStatType.TriggerCount:
                return new ItemDisplayInfo
                {
                    Name = "Resonance",
                    Description = "Number of Activations When Triggered"
                };
            case PlayerEffectStatType.EffectSize:
                return new ItemDisplayInfo
                {
                    Name = "Reach",
                    Description = "Size of The Effect"
                };
            case PlayerEffectStatType.EffectDuration:
                return new ItemDisplayInfo
                {
                    Name = "Flow",
                    Description = "Duration of Damage Over Time Effects/Channeled Effects"
                };
            case PlayerEffectStatType.SpawnCount:
                return new ItemDisplayInfo
                {
                    Name = "Fragments",
                    Description = "Number of Effects for Each Activation"
                };
            case PlayerEffectStatType.Cooldown:
                return new ItemDisplayInfo
                {
                    Name = "Recovery",
                    Description = "Cooldown of The Effect"
                };
            case PlayerEffectStatType.ProjSpeed:
                return new ItemDisplayInfo
                {
                    Name = "Momentum",
                    Description = "Projectiles Movement Speed"
                };
            case PlayerEffectStatType.ProjLifetime:
                return new ItemDisplayInfo
                {
                    Name = "Persistance",
                    Description = "Time Before Projectile Dissapates"
                };
        }
        return new ItemDisplayInfo
        {
            Name = "Unrecognized",
            Description = "No Data Available For This Stat"
        };
    }
}
