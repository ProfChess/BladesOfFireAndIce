using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Relic"))]
public class Relic : BaseBoon
{
    public RelicEffectType EffectType;
    [Header("Attributes")]
    public RelicBaseStats BaseStats;

    public override void Effect(PlayerEventContext context)
    {
        RelicEffectLibrary.PlayBoonEffect(this, EffectType, context);
    }
}
public enum RelicEffectType { HealthBuff, DamageBuff, AttackSpeedBuff, MoveSpeedBuff}

[System.Serializable]
public class RelicBaseStats
{
    [Header("Base Stats")]
    [Tooltip("Amount of a Bonus Provided by Relic")]
    public float PercentageIncrease = 0f;
    [Tooltip("Time of Each Instance of Buff")]
    public float Duration = 0f;
}