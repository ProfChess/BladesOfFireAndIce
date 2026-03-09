using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/BlessingEffect/ReduceRollStaminaCost"))]
public class StatBlessingEffect_ReduceRollStaminaCost : BaseStatBlessingEffect
{
    [SerializeField] private float RollStaminaPercentageReduction = 0f;
    public override void ApplyEffect()
    {
        PlayerEffectSubscriptionManager.Instance.AddBonus(StatType.RollEndurance, RollStaminaPercentageReduction);
    }

    public override void RemoveEffect()
    {
        PlayerEffectSubscriptionManager.Instance.RemoveBonus(StatType.RollEndurance, RollStaminaPercentageReduction);
    }
}
