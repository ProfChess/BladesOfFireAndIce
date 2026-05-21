using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/BlessingEffect/FlatStatChange"))]
public class StatBlessingFlatEffect : BaseStatBlessingEffect
{
    [SerializeField] private StatType Stat;
    [SerializeField] private float PercentageValue;
    public override void ApplyEffect()
    {
        PlayerEffectSubscriptionManager.Instance.AddBonus(Stat, PercentageValue);
    }

    public override void RemoveEffect()
    {
        PlayerEffectSubscriptionManager.Instance.RemoveBonus(Stat, PercentageValue);
    }
    public StatDisplayEntry GetDisplayInfo()
    {
        return new StatDisplayEntry
        {
            DisplayInfo = UIStatDefinitions.GetInfo(Stat),
            NewValue = PercentageValue,
            IsPercentage = true
        };
    }
}
