
using UnityEngine;

public static class BoonEffectLibrary
{
    public static void PlayBoonEffect(DamageEffectBoon Boon, DamageBoonEffectType effectType, AttackEventDetails Details)
    {
        float Damage = Boon.Damage;
        switch (effectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Details.Element, Details.Target, Damage); break;
            case DamageBoonEffectType.FireBurst: ElementEffect_FireBurst(Details.Element, Damage); break;
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
