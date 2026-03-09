using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/Blessing"))]
public class StatBlessing : ScriptableObject
{
    [Header("Blessing Info")]
    public string BlessingName;
    [TextArea] public string BlessingDescription;

    [Header("")]
    public List<BaseStatBlessingEffect> EffectList = new();

    private RunDataManager rundata => GameManager.Instance.runData;

    public void Apply()
    {
        foreach (var effect in EffectList)
        {
            effect.ApplyEffect();
        }
    }
}
public abstract class BaseStatBlessingEffect : ScriptableObject { public abstract void ApplyEffect(); }
public abstract class StatBlessingEffect_EventTriggered : BaseStatBlessingEffect
{
    [SerializeField] private BoonEventType[] TriggerEvents;
    protected void SubToEvents(Action<PlayerEventContext> EffectToAttach)
    {
        //Check 
        if (TriggerEvents == null || TriggerEvents.Length == 0)
        {
            Debug.LogWarning($"{name} has no trigger events");
        }


        foreach (BoonEventType eventType in TriggerEvents)
        {
            PlayerEffectSubscriptionManager.Instance.SubscribeToPlayerEvent(EffectToAttach, eventType);
        }
    }
}
