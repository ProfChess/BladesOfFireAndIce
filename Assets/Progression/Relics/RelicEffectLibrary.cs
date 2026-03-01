
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
            case RelicEffectType.IceShieldBuff: RelicEffect_IceShield(relic, Details); break;
            case RelicEffectType.HealthBuff: RelicEffect_HealthBuff(relic, Details); break;

        }
    }

    //Starting Relic --> Fire Shield --> Grants Ability to Give AOE/Damage Buff Upon Blocking Damage
    private static void RelicEffect_FireShield(Relic relic, PlayerEventContext ctx)
    {
        //Make Sure Context is Correct
        if (ctx is not BlockEventContext blockedCtx || blockedCtx.hitsBlocked == 0) { return; }

        //Get Timing Scaled off Blocked Hits
        float FinalDuration = relic.Duration * blockedCtx.hitsBlocked;

        //Increased percentage from hits absorbed and base increase
        float DamageScale = relic.GetIncreaseFromStat(StatType.StrengthFire) * blockedCtx.hitsBlocked;
        float AOEScale = relic.GetIncreaseFromStat(StatType.ReachFire) * blockedCtx.hitsBlocked;

        if (!rundata.isRelicApplied(relic) || rundata.GetRelicBuffTimeRemaining(relic) < FinalDuration)
        {
            if (rundata.isRelicApplied(relic)) { rundata.RelicDeactivated(relic); }

            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, DamageScale);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.ReachFire, AOEScale);
            rundata.RelicActivated(
                relic,
                () => {
                    PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, DamageScale);
                    PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.ReachFire, AOEScale);
                },
                FinalDuration);
        }

    }

    //Starting Relic --> Ice Shield --> Grants Invulnerability and Attack Speed Upon Successful Parry
    private static void RelicEffect_IceShield(Relic relic, PlayerEventContext ctx)
    {
        if (!rundata.isRelicApplied(relic))
        {
            //Apply Bonus
            float AttackSpeedIncrease = relic.GetIncreaseFromStat(StatType.DexterityIce);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.DexterityIce, AttackSpeedIncrease);
            
            //Mark Relic as Activated and Submit Deactivation Logic with Timing
            rundata.RelicActivated(relic, () =>
            {
                PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.DexterityIce, AttackSpeedIncrease);
            }, relic.Duration);
        }
    }

    //Grants Bonus Damage When Above 50% HP
    private static void RelicEffect_HealthBuff(Relic relic, PlayerEventContext ctx)
    {
        if (ctx is not StatChangeEventContext statctx) { return; }
        bool Above50 = (statctx.newValue / statctx.maxValue) >= 0.5f;

        if (Above50 && !rundata.isRelicApplied(relic))
        {
            float FireDamageIncrease = relic.GetIncreaseFromStat(StatType.StrengthFire);
            float IceDamageIncrease = relic.GetIncreaseFromStat(StatType.StrengthIce);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, FireDamageIncrease);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthIce, IceDamageIncrease);
            
            rundata.RelicActivated(relic, () =>
            {
                PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, FireDamageIncrease);
                PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthIce, IceDamageIncrease);
            });
        }
        else if (!Above50 && rundata.isRelicApplied(relic))
        {
            rundata.RelicDeactivated(relic);
        }
    }

}
