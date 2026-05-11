using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/BlessingEffect/FlatStatChange"))]
public class StatBlessingStatBonus : BaseStatBlessingEffect
{
    [SerializeField] private StatType Stat;
    [SerializeField] private float Value;
    public override void ApplyEffect()
    {
        PlayerEffectSubscriptionManager.Instance.AddBonus(Stat, Value);
    }

    public override void RemoveEffect()
    {
        PlayerEffectSubscriptionManager.Instance.RemoveBonus(Stat, Value);
    }
}
