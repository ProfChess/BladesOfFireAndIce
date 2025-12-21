using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Boons/EventBoon"))]
public class EventBoon : BaseBoon
{
    //Damage of Effect
    [Header("Boon Attributes")]
    public BoonBaseStats BaseStats;
    public BoonLevelScalers LevelScalers;
    

    [Header("Effect and Event")]
    [Tooltip("Effect of This Boon")]
    public DamageBoonEffectType EffectType;

    [Tooltip("Event That Will Trigger This Boons Effect")]
    public BoonEventType EventToAttach;

    private Action<AttackEventDetails> effectDelegate;

    public override void BoonSelected()
    {
        effectDelegate = Effect;
        PlayerBoonManager.Instance.SubscribeToPlayerEvent(effectDelegate, EventToAttach);
    }
    public override void BoonRemoved()
    {
        if (effectDelegate != null)
        {
            PlayerBoonManager.Instance.UnSubscribeFromPlayerEvent(effectDelegate, EventToAttach);
        }
    }
    public virtual void Effect(AttackEventDetails AttackDetails)
    {
        BoonEffectLibrary.PlayBoonEffect(this, EffectType, AttackDetails);
    }

    public BoonLeveledStats GetLeveledStats(EventBoon Boon, int Level)
    {
        if (Level == 1)
        {
            return new BoonLeveledStats
            {
                FinalDamage = Boon.BaseStats.Damage,
                FinalFrequency = Boon.BaseStats.Freq,
                FinalArea = Boon.BaseStats.Area,
                FinalDuration = Boon.BaseStats.Duration,
                FinalEffectNumber = Boon.BaseStats.EffectNum,
            };
        }
        else
        {
            return new BoonLeveledStats
            {
                FinalDamage = Boon.BaseStats.Damage * Boon.LevelScalers.DamageScale,
                FinalFrequency = (int)(Boon.BaseStats.Freq * Boon.LevelScalers.FrequencyScale),
                FinalArea = Boon.BaseStats.Area * Boon.LevelScalers.AreaScale,
                FinalDuration = Boon.BaseStats.Duration * Boon.LevelScalers.DurationScale,
                FinalEffectNumber = Boon.BaseStats.EffectNum * Boon.LevelScalers.EffectNumberScale,
            };
        }


    }
}
//Boon Type Specific Enum
public enum DamageBoonEffectType { FireBoom, FireBurst, IceBoom}

//Base Stats
[System.Serializable]
public class BoonBaseStats
{
    //Base Values Each Boon Might Use
    [Tooltip("Base Damage of Boon Effect")]
    public float Damage = 0f;
    [Tooltip("Number of Times Effect is Triggered After Event")]
    public int Freq = 1;
    [Tooltip("Size of Effect")]
    public Vector2 Area = new Vector2(1, 1);
    [Tooltip("Duration of Timed Effects")]
    public float Duration = 1f;
    [Tooltip("Number of Effect Objects For Each Effect Triggered")]
    public int EffectNum = 1;
}
//Leveling Values
[System.Serializable]
public class BoonLevelScalers
{
    //Multipliers on Base Values
    public float DamageScale = 1f;
    public float FrequencyScale = 1;
    public Vector2 AreaScale = new Vector2(1, 1);
    public float DurationScale = 1f;
    public int EffectNumberScale = 1;
}
public class BoonLeveledStats
{
    //Multipliers on Base Values
    public float FinalDamage;
    public int FinalFrequency;
    public Vector2 FinalArea;
    public float FinalDuration;
    public int FinalEffectNumber;
}