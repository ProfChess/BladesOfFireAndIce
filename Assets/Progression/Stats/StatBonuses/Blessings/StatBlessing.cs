using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/Blessing"))]
public class StatBlessing : BaseAttainedBonus
{
    [Header("")]
    public List<BaseStatBlessingEffect> EffectList = new();

    private RunDataManager rundata => GameManager.Instance.runData;

    public void ApplyEffects()
    {
        foreach (var effect in EffectList)
        {
            effect.ApplyEffect();
        }
    }
    public void RemoveEffects()
    {
        foreach (var effect in EffectList)
        {
            effect.RemoveEffect();
        }
    }
}
public abstract class BaseStatBlessingEffect : ScriptableObject { public abstract void ApplyEffect(); public abstract void RemoveEffect(); }
public abstract class StatBlessingEffect_EventTriggered : BaseStatBlessingEffect
{
    [SerializeField] private BoonEventType[] TriggerEvents;
    protected abstract void TriggerEffect(PlayerEventContext ctx);
    protected void SubToEvents()
    {
        //Check 
        if (TriggerEvents == null || TriggerEvents.Length == 0)
        {
            Debug.LogWarning($"{name} has no trigger events");
        }


        foreach (BoonEventType eventType in TriggerEvents)
        {
            PlayerEffectSubscriptionManager.Instance.SubscribeToPlayerEvent(TriggerEffect, eventType);
        }
    }
    protected void UnSubToEvents()
    {
        //Check 
        if (TriggerEvents == null || TriggerEvents.Length == 0)
        {
            Debug.LogWarning($"{name} has no trigger events");
        }

        foreach (BoonEventType eventType in TriggerEvents)
        {
            PlayerEffectSubscriptionManager.Instance.UnSubscribeFromPlayerEvent(TriggerEffect, eventType);
        }
    }
}
