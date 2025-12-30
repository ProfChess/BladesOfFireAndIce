
using UnityEngine;

public static class BoonEffectLibrary
{
    public static void PlayBoonEffect(EventBoon Boon, DamageBoonEffectType effectType, AttackEventDetails Details)
    {
        switch (effectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Boon, Details.Element, Details.AttackOrigin); break;
            case DamageBoonEffectType.FireBurst: ElementEffect_FireBurst(Boon, Details.Element, Details.AttackOrigin); break;
        }
    }

    //ELEMENT EFFECTS
    private static void ElementEffect_FireBurst(EventBoon Boon, ElementType Element, Vector2 Position)
    {
        //Get Level
        int Level = GameManager.Instance.runData.GetBoonLevel(Boon);


        //Effect
        if (PlayerEffectPoolManager.Instance == null) { return; }
        GameObject Explosion = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.FlameExplosion);
        
        if (Explosion != null)
        {
            Debug.Log($"BaseDamage={Boon.BaseStats.Damage}, DamageScale={Boon.LevelScalers.DamageScale}");
            BoonLeveledStats Stats = Boon.GetLeveledStats(Boon, Level);
            Explosion.GetComponent<BaseEffectSpawn>().Spawn(Position, 
                Stats.FinalArea, 
                Stats.FinalDamage,
                Stats.FinalFrequency, 
                Stats.FinalDuration);

            Debug.Log($"Spawn Damage={Stats.FinalDamage}");
        }
    }

    private static void DamageEffect_FireBoom(EventBoon Boon, ElementType Element, Vector2 Position)
    {

    }

}
