
public static class RelicEffectLibrary
{
    public static void PlayBoonEffect(Relic relic, RelicEffectType effectType, PlayerEventContext Details)
    {
        //Check if Boon can Trigger in Element
        bool canBoonTrigger = (relic.ElementRestriction == ElementType.None || relic.ElementRestriction == Details.Element);
        if (!canBoonTrigger) return;

        switch (effectType)
        {
            case RelicEffectType.HealthBuff: RelicEffect_HealthBuff(relic, Details); break;

        }
    }

    private static void RelicEffect_HealthBuff(Relic relic, PlayerEventContext ctx)
    {
        if (ctx is not StatChangeEventContext statctx) { return; }
    }
}
