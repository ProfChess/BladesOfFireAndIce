using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Blessings/BlessingEffect/ConditionalStatChange"))]
public class StatBlessingEffect_ConditionalStatChange : BaseStatBlessingEffect
{
    [Tooltip("Every StatChange Will Take Place When Any Event in TriggerEvents is Triggered")]
    [SerializeField] private StatPercentageValuePair[] StatChanges;
    public float Duration = 0f;

    public override void ApplyEffect()
    {
        foreach (var ValuePair in StatChanges)
        {
            PlayerEffectSubscriptionManager.Instance.AddBonus(ValuePair.Stat, ValuePair.PercentageIncrease);
        }
    }

    public override void RemoveEffect()
    {
        foreach (var ValuePair in StatChanges)
        {
            PlayerEffectSubscriptionManager.Instance.RemoveBonus(ValuePair.Stat, ValuePair.PercentageIncrease);
        }
    }
    public List<StatDisplayEntry> GetStatDisplayInfo()
    {
        List<StatDisplayEntry> EntryList = new();

        foreach(var ValuePair in StatChanges)
        {
            StatDisplayEntry newEntry = new();
            newEntry.DisplayInfo = UIStatDefinitions.GetInfo(ValuePair.Stat);
            newEntry.Value = ValuePair.PercentageIncrease;
            newEntry.IsPercentage = true;
            EntryList.Add(newEntry);
        }
        return EntryList;
    }

}
public class TimedBlessingEffect
{
    public Action DisableEffect;
    public float Duration;
    public bool IsActive;
}
