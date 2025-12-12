
public static class BoonEffectLibrary
{
    public static void PlayElementEffectBoon(ElementBoonEffectType effectType, ElementType Element)
    {
        switch (effectType)
        {
            case ElementBoonEffectType.FireBurst: ElementEffect_FireBurst(Element); break;
        }
    }
    public static void PlayDamageEffectBoon(DamageBoonEffectType effectType, ElementType Element, BaseHealth EnemyHealth)
    {
        switch (effectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Element, EnemyHealth); break;
        }
    }


    //ELEMENT EFFECTS
    private static void ElementEffect_FireBurst(ElementType Element)
    {

    }
    

    //DAMAGE EFFECTS
    private static void DamageEffect_FireBoom(ElementType Element, BaseHealth EnemyHealth)
    {

    }
}
