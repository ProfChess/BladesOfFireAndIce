
using System;
using System.Collections.Generic;
using UnityEngine;
public static class RelicEffectLibrary
{
    private static RunDataManager rundata => GameManager.Instance.runData;
    public static void PlayRuneEffect(Rune relic, PlayerEventContext Details)
    {
        //Check if Boon can Trigger in Element
        bool canBoonTrigger = (relic.ElementRestriction == ElementType.None || relic.ElementRestriction == Details.Element);
        if (!canBoonTrigger) return;

        switch (relic.EffectType)
        {
            case RelicEffectType.FireShieldBuff: RuneEffect_FireShield(relic, Details); break;
            case RelicEffectType.IceShieldBuff: RuneEffect_IceShield(relic, Details); break;
            case RelicEffectType.DamageFromHealthBuff: RuneEffect_DamageFromHealthBuff(relic, Details); break;
            case RelicEffectType.StartingSwapBuff: RuneEffect_SwapElement(relic, Details); break;

        }
    }
    private static readonly HashSet<StatType> RestorativeStats = new() { StatType.RefreshVitality, StatType.RefreshEndurance };
    //Default Application and Deactivation Calls
    private static void ApplyAllStatBuffs(List<StatPercentageValuePair> StatPairs)
    {
        foreach (StatPercentageValuePair pair in StatPairs)
        {
            if (RestorativeStats.Contains(pair.Stat))
            {
                PlayerEffectSubscriptionManager.Instance.RestoreStat(pair.Stat, pair.PercentageIncrease);
            }
            else
            {
                PlayerEffectSubscriptionManager.Instance.AddBonus(pair.Stat, pair.PercentageIncrease);
            }
        }
    }
    private static void UnApplyAllStatBuffs(List<StatPercentageValuePair> StatPairs)
    {
        foreach (StatPercentageValuePair pair in StatPairs)
        {
            if (RestorativeStats.Contains(pair.Stat)) { continue; } //Does not Refund Restorative Stats
            PlayerEffectSubscriptionManager.Instance.RemoveBonus(pair.Stat, pair.PercentageIncrease);
        }
    }


    //Starting Relic --> Fire Shield --> Grants Ability to Give AOE/Damage Buff Upon Blocking Damage
    private static void RuneEffect_FireShield(Rune relic, PlayerEventContext ctx)
    {
        //Make Sure Context is Correct
        if (ctx is not BlockEventContext blockedCtx || blockedCtx.hitsBlocked == 0) { return; }

        //Get Timing Scaled off Blocked Hits
        float FinalDuration = relic.Duration * blockedCtx.hitsBlocked;

        //Increased percentage from hits absorbed and base increase
        float DamageScale = relic.GetIncreaseFromStat(StatType.StrengthFire) * blockedCtx.hitsBlocked;
        float AOEScale = relic.GetIncreaseFromStat(StatType.ReachFire) * blockedCtx.hitsBlocked;

        if (!rundata.isRuneApplied(relic) || rundata.GetRuneBuffTimeRemaining(relic) < FinalDuration)
        {
            if (rundata.isRuneApplied(relic)) { rundata.RuneDeactivated(relic); }

            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, DamageScale);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.ReachFire, AOEScale);
            rundata.RuneActivated(
                relic,
                () => {
                    PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, DamageScale);
                    PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.ReachFire, AOEScale);
                },
                FinalDuration);
        }

    }

    //Starting Relic --> Ice Shield --> Grants Invulnerability and Attack Speed Upon Successful Parry
    private static void RuneEffect_IceShield(Rune relic, PlayerEventContext ctx)
    {
        if (!rundata.isRuneApplied(relic))
        {
            //Apply Bonus
            ApplyAllStatBuffs(relic.StatIncreases);
            
            //Mark Relic as Activated and Submit Deactivation Logic with Timing
            rundata.RuneActivated(relic, () =>
            {
                UnApplyAllStatBuffs(relic.StatIncreases);
            }, relic.Duration);
        }
    }

    //Starting Relic --> Swapping Power --> Grants Buff Depending on Element + Restore Stamina
    private static void RuneEffect_SwapElement(Rune relic, PlayerEventContext ctx)
    {
        if (rundata.isRuneApplied(relic)) { rundata.RuneDeactivated(relic); }
        
        //Fire Buff
        ApplyAllStatBuffs(relic.StatIncreases);
        rundata.RuneActivated(relic, () =>
        {
            UnApplyAllStatBuffs(relic.StatIncreases);
        }, relic.Duration);
    }

    //Grants Bonus Damage When Above 50% HP
    private static void RuneEffect_DamageFromHealthBuff(Rune relic, PlayerEventContext ctx)
    {
        if (ctx is not StatChangeEventContext statctx) { return; }
        bool Above50 = (statctx.newValue / statctx.maxValue) >= 0.5f;
        Debug.Log(Above50);

        if (Above50 && !rundata.isRuneApplied(relic))
        {
            ApplyAllStatBuffs(relic.StatIncreases);
            
            rundata.RuneActivated(relic, () =>
            {
                UnApplyAllStatBuffs(relic.StatIncreases);
            });
        }
        else if (!Above50 && rundata.isRuneApplied(relic))
        {
            rundata.RuneDeactivated(relic);
        }
    }

}
