using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActivePlayerEffectHelpers
{
    //Adds given stat to display if determined to be relevant
    public static void AddStatIfRelevant(PlayerEffectStatType stat, StatsToDisplay displayStats, List<StatDisplayEntry> list, float value, bool isPercent = false)
    {
        if (!ShouldDisplayStat(stat, displayStats)) { return; }
        list.Add(new StatDisplayEntry
        {
            DisplayInfo = UIStatDefinitions.GetInfo(stat),
            Value = value,
            IsPercentage = isPercent,
        });
    }
    //returns if a stat has been marked as important
    public static bool ShouldDisplayStat(PlayerEffectStatType stat, StatsToDisplay displayStats)
    {
        switch (stat)
        {
            case PlayerEffectStatType.Damage: return displayStats.Damage;
            case PlayerEffectStatType.TriggerCount: return displayStats.TriggerCount;
            case PlayerEffectStatType.EffectSize: return displayStats.EffectSize;
            case PlayerEffectStatType.EffectDuration: return displayStats.EffectDuration;
            case PlayerEffectStatType.SpawnCount: return displayStats.SpawnCount;
            case PlayerEffectStatType.Cooldown: return displayStats.Cooldown;
            case PlayerEffectStatType.ProjSpeed: return displayStats.ProjSpeed;
            case PlayerEffectStatType.ProjLifetime: return displayStats.ProjLifetime;
        }
        return false;
    }
}
