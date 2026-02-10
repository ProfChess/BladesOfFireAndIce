using UnityEngine;

public static class BoonEffectLibrary
{
    public static void PlayBoonEffect(Virtue Boon, PlayerEventContext Details)
    {
        //Clarify Location
        Vector2 Location = GetEffectLocation(Boon, Details);
        bool canBoonTrigger = (Boon.ElementRestriction == ElementType.None || Boon.ElementRestriction == Details.Element);
        if (!canBoonTrigger) return;

        switch (Boon.EffectType)
        {
            case DamageBoonEffectType.FireBoom: DamageEffect_FireBoom(Boon, Location); break;
            case DamageBoonEffectType.FireBurst: ElementEffect_FireBurst(Boon, Location); break;
            case DamageBoonEffectType.IceBoom: Projectile_IceCircle(Boon, Details, Location); break;
        }
    }
    private static Vector2 GetEffectLocation(Virtue Boon, PlayerEventContext ctx)
    {
        switch (Boon.EffectOrigin)
        {
            case EffectOriginType.Player: 
                return ctx.player.transform.position;
            
            case EffectOriginType.Target:   
                if (ctx is AttackEventContext atkCxt && atkCxt.Target != null)
                { return atkCxt.Target.transform.position; }
                
                //Target Was not Given or Context is Incorrect
                Debug.Log("Target Not Found"); 
                return ctx.player.transform.position;

            case EffectOriginType.Attack:
                if (ctx is AttackEventContext atk)
                { return atk.AttackBoxOrigin; }
                
                //Context is Incorrect
                Debug.Log("Incorrect Event Context"); 
                return ctx.player.transform.position;
        }
        //Fallback
        return ctx.player.transform.position;
    }

    //Effect List
    private static void ElementEffect_FireBurst(Virtue virtue, Vector2 Position)
    {
        //Get Level
        int Level = GameManager.Instance.runData.GetVirtueLevel(virtue);

        //Effect
        if (PlayerEffectPoolManager.Instance == null) { return; }
        GameObject Explosion = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.FlameExplosion);
        
        if (Explosion != null)
        {
            BoonLeveledStats Stats = virtue.GetLeveledStats(Level);
            Explosion.GetComponent<BaseEffectSpawn>().Spawn(Position, 
                Stats.FinalArea, 
                Stats.FinalDamage,
                Stats.FinalFrequency, 
                Stats.FinalDuration);
        }
    }

    private static void DamageEffect_FireBoom(Virtue Boon, Vector2 Position)
    {

    }


    //Projectile
    private static void Projectile_IceCircle(Virtue virtue, PlayerEventContext ctx, Vector2 SpawnLocation)
    {
        //Get Level and Stats
        int Level = GameManager.Instance.runData.GetVirtueLevel(virtue);
        BoonLeveledStats Stats = virtue.GetLeveledStats(Level);

        //Effect
        if (PlayerEffectPoolManager.Instance == null) { return; }

        for (int i = 0; i < Stats.FinalFrequency; i++)
        {
            for (int a = 0; a < Stats.FinalEffectNumber; a++)
            {
                GameObject Proj = PlayerEffectPoolManager.Instance.getObjectFromPool(PlayerEffectObjectType.IceProj);
                Vector2 RandDirection = GetRandomDirectionAroundObject(ctx.Direction, 180);
                BaseProjectileEffectSpawn ProfRef = Proj.GetComponent<BaseProjectileEffectSpawn>();
                
                if (virtue.EffectOrigin == EffectOriginType.Target && ctx is AttackEventContext attackCtx)
                {
                    ProfRef.ignoredEnemy = attackCtx.Target.gameObject;
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
