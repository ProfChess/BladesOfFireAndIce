
using UnityEngine;

public static class BoonEffectLibrary
{
    public static void PlayElementEffectBoon(ElementEffectBoon Boon, ElementBoonEffectType effectType, ElementType Element)
    {
        float Damage = Boon.Damage;
        switch (effectType)
        {
            case ElementBoonEffectType.FireBurst: ElementEffect_FireBurst(Element, Damage); break;
        }
    }
    public static void PlayDamageEffectBoon(DamageEffectBoon Boon, DamageBoonEffectType effectType, ElementType Element, BaseHealth EnemyHealth)
    {
        float Damage = Boon.Damage;
        switch (effectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Element, EnemyHealth, Damage); break;
        }
    }


    //ELEMENT EFFECTS
    private static void ElementEffect_FireBurst(ElementType Element, float DamageOfEffect = 0)
    {
        if (PlayerEffectPoolManager.Instance == null) { return; }
        GameObject Explosion = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.FlameExplosion);
        Explosion.GetComponent<BaseEffectSpawn>().Spawn(GameManager.Instance.getPlayer().transform.position, DamageOfEffect);
    }
    

    //DAMAGE EFFECTS
    private static void DamageEffect_FireBoom(ElementType Element, BaseHealth EnemyHealth, float DamageOfEffect)
    {

    }
}
