
using System;
using UnityEngine;
public static class RelicEffectLibrary
{
    private static RunDataManager rundata => GameManager.Instance.runData;
    public static void PlayRelicEffect(Relic relic, PlayerEventContext Details)
    {
        //Check if Boon can Trigger in Element
        bool canBoonTrigger = (relic.ElementRestriction == ElementType.None || relic.ElementRestriction == Details.Element);
        if (!canBoonTrigger) return;

        switch (relic.EffectType)
        {
            case RelicEffectType.FireShieldBuff: RelicEffect_FireShield(relic, Details); break;
            case RelicEffectType.HealthBuff: RelicEffect_HealthBuff(relic, Details); break;

        }
    }

    //Starting Relic --> Fire Shield --> Grants Ability to Give AOE/Damage Buff Upon Blocking Damage
    private static void RelicEffect_FireShield(Relic relic, PlayerEventContext ctx)
    {
        //Make Sure Context is Correct
        if (ctx is not BlockEventContext blockedCtx) { return; }
        if (blockedCtx.hitsBlocked == 0) return; //Return if no hits were blocked

        //Get Timing Scaled off Blocked Hits
        float FinalDuration = relic.BaseStats.Duration * blockedCtx.hitsBlocked;

        //Increased percentage from hits absorbed and base increase
        float DamageAndAOEScale = relic.BaseStats.PercentageIncrease * blockedCtx.hitsBlocked;

        if (!rundata.isRelicApplied(relic))
        {
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, DamageAndAOEScale);
            rundata.RelicActivated(
                relic, 
                () => { PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, DamageAndAOEScale); }, 
                FinalDuration);
        }
        else if (rundata.isRelicApplied(relic))
        {
            //Refresh Buff to New Duration if Remaining Duration is Less Than New Duration
            if (rundata.GetRelicBuffTimeRemaining(relic) < FinalDuration)
            {
                //End Previous Duration
                rundata.RelicDeactivated(relic);
                PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, DamageAndAOEScale);
                rundata.RelicActivated(
                    relic,
                    () => { PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, DamageAndAOEScale); },
                    FinalDuration);
            }
        }

    }


    //Grants Bonus Damage When Above 50% HP
    private static void RelicEffect_HealthBuff(Relic relic, PlayerEventContext ctx)
    {
        if (ctx is not StatChangeEventContext statctx) { return; }
        bool Above50 = (statctx.newValue / statctx.maxValue) >= 0.5f;

        if (Above50 && !rundata.isRelicApplied(relic))
        {
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, relic.BaseStats.PercentageIncrease);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthIce, relic.BaseStats.PercentageIncrease);
            
            rundata.RelicActivated(relic, () =>
            {
                PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, relic.BaseStats.PercentageIncrease);
                PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthIce, relic.BaseStats.PercentageIncrease);
            });
        }
        else if (!Above50 && rundata.isRelicApplied(relic))
        {
            rundata.RelicDeactivated(relic);
        }
    }

}
