
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
            case RelicEffectType.HealthBuff: RelicEffect_HealthBuff(relic, Details); break;

        }
    }

    private static void RelicEffect_HealthBuff(Relic relic, PlayerEventContext ctx)
    {
        if (ctx is not StatChangeEventContext statctx) { return; }
        bool Above50 = (statctx.newValue / statctx.maxValue) >= 0.5f;

        if (Above50 && !rundata.isRelicApplied(relic))
        {
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthFire, relic.BaseStats.PercentageIncrease);
            PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.StrengthIce, relic.BaseStats.PercentageIncrease);
            rundata.RelicActivated(relic);
        }
        else if (!Above50 && rundata.isRelicApplied(relic))
        {
            PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthFire, relic.BaseStats.PercentageIncrease);
            PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.StrengthIce, relic.BaseStats.PercentageIncrease);
            rundata.RelicDeactivated(relic);
        }
    }

}
