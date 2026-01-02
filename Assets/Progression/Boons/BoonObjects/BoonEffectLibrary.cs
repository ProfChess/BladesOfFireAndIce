
using UnityEngine;

public static class BoonEffectLibrary
{
    public static void PlayBoonEffect(EventBoon Boon, DamageBoonEffectType effectType, AttackEventDetails Details)
    {
        switch (effectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Boon, Details.Element, Details.AttackOrigin); break;
            case DamageBoonEffectType.FireBurst: ElementEffect_FireBurst(Boon, Details.Element, Details.AttackOrigin); break;
            case DamageBoonEffectType.IceBoom: Projectile_IceCircle(Boon, Details); break;
        }
    }

    //Effect List
    private static void ElementEffect_FireBurst(EventBoon Boon, ElementType Element, Vector2 Position)
    {
        //Get Level
        int Level = GameManager.Instance.runData.GetBoonLevel(Boon);

        //Effect
        if (PlayerEffectPoolManager.Instance == null) { return; }
        GameObject Explosion = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.FlameExplosion);
        
        if (Explosion != null)
        {
            BoonLeveledStats Stats = Boon.GetLeveledStats(Boon, Level);
            Explosion.GetComponent<BaseEffectSpawn>().Spawn(Position, 
                Stats.FinalArea, 
                Stats.FinalDamage,
                Stats.FinalFrequency, 
                Stats.FinalDuration);
        }
    }

    private static void DamageEffect_FireBoom(EventBoon Boon, ElementType Element, Vector2 Position)
    {

    }


    //Projectile
    private static void Projectile_IceCircle(EventBoon Boon, AttackEventDetails AttackDetails)
    {
        //Get Level and Stats
        int Level = GameManager.Instance.runData.GetBoonLevel(Boon);
        BoonLeveledStats Stats = Boon.GetLeveledStats(Boon, Level);

        //Effect
        if (PlayerEffectPoolManager.Instance == null) { return; }

        for (int i = 0; i < Stats.FinalFrequency; i++)
        {
            for (int a = 0; a < Stats.FinalEffectNumber; a++)
            {
                GameObject Proj = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.IceProj);
                Vector2 RandDirection = GetRandomDirectionAroundObject(AttackDetails.Direction, 180);
                BaseProjectileEffectSpawn ProfRef = Proj.GetComponent<BaseProjectileEffectSpawn>();
                Vector2 SpawnLocation = AttackDetails.AttackOrigin;
                if (AttackDetails.Target != null)
                {
                    ProfRef.ignoredEnemy = AttackDetails.Target.gameObject;
                    SpawnLocation = AttackDetails.Target.transform.position;
                }
                ProfRef.Spawn(SpawnLocation, RandDirection, Stats.FinalArea,
                    Stats.FinalDamage, Stats.FinalDuration, Stats.FinalProjTravelDuration, Stats.FinalProjSpeed);
            }
        }
    }
    private static Vector2 GetRandomDirectionAroundObject(Vector2 StartingDir, float maxAngleDegrees)
    {
        float angle = Random.Range(-maxAngleDegrees, maxAngleDegrees);
        return Quaternion.Euler(0, 0, angle) * StartingDir.normalized;
    }

}
