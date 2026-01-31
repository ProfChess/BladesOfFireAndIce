using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Boon"))]
public class Virtue : BaseBoon
{
    [Tooltip("Effect of This Boon")]
    public DamageBoonEffectType EffectType;

    [Tooltip("Effect will Use This value for Spawning so Long as It is Available")]
    public EffectOriginType EffectOrigin;

    //Damage of Effect
    [Header("Boon Attributes")]
    public BoonBaseStats BaseStats;
    [Header("")]
    public BoonLevelScalers LevelScalers;

    public override void Effect(PlayerEventContext AttackDetails)
    {
        BoonEffectLibrary.PlayBoonEffect(this, EffectType, AttackDetails);
    }
    public override void BoonCollected()
    {
        runData.AddBoon(this);
    }

    public BoonLeveledStats GetLeveledStats(int Level)
    {
        return new BoonLeveledStats
        {
            FinalDamage = BaseStats.Damage * Mathf.Pow(LevelScalers.DamageScale, Level - 1),
            FinalFrequency = (int)(BaseStats.Freq * Mathf.Pow(LevelScalers.FrequencyScale, Level - 1)),
            FinalArea = Vector2.Scale(BaseStats.Area, new Vector2(
                Mathf.Pow(LevelScalers.AreaScale.x, Level - 1),
                Mathf.Pow(LevelScalers.AreaScale.y, Level - 1))),
            FinalDuration = BaseStats.Duration * Mathf.Pow(LevelScalers.DurationScale, Level - 1),
            FinalEffectNumber = (int)(BaseStats.EffectNum * Mathf.Pow(LevelScalers.EffectNumberScale, Level - 1)),
            FinalProjSpeed = BaseStats.ProjSpeed * Mathf.Pow(LevelScalers.ProjSpeedScale, Level - 1),
            FinalProjTravelDuration = BaseStats.ProjTravelTime * Mathf.Pow(LevelScalers.ProjSpeedScale, Level - 1),
        };
    }
}
//Boon Type Specific Enum
public enum DamageBoonEffectType { FireBoom, FireBurst, IceBoom}
public enum EffectOriginType { Player, Target, Attack}

//Base Stats
[System.Serializable]
public class BoonBaseStats
{
    //Base Values Each Boon Might Use
    [Header("All Boons")]
    [Tooltip("Base Damage of Boon Effect")]
    public float Damage = 0f;
    [Tooltip("Size of Effect")]
    public Vector2 Area = new Vector2(1, 1);
    [Tooltip("Number of Times Effect is Triggered After Event")]
    public int Freq = 1;
    [Tooltip("Duration of Timed Effects")]
    public float Duration = 1f;
    [Tooltip("Number of Effect Objects For Each Effect Triggered")]
    public int EffectNum = 1;

    [Header("Projectile Specific")]
    [Tooltip("Speed the Projectile Travels")]
    public float ProjSpeed = 1f;
    [Tooltip("Time the Proj Lasts")]
    public float ProjTravelTime = 1f;
}
//Leveling Values
[System.Serializable]
public class BoonLevelScalers
{
    //Multipliers on Base Values
    public float DamageScale = 1f;
    public float FrequencyScale = 1f;
    public Vector2 AreaScale = new Vector2(1, 1);
    public float DurationScale = 1f;
    public float EffectNumberScale = 1;
    public float ProjSpeedScale = 1f;
    public float ProjTravelTimeScale = 1f;
}
public struct BoonLeveledStats
{
    //Multipliers on Base Values
    public float FinalDamage;
    public int FinalFrequency;
    public Vector2 FinalArea;
    public float FinalDuration;
    public int FinalEffectNumber;
    public float FinalProjSpeed;
    public float FinalProjTravelDuration;
}