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
                    Description = "Increases Damage Dealt While In Fire Stance"
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
}
