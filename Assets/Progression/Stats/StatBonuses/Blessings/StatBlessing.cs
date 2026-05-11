using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Effect/Blessings/Blessing"))]
public class StatBlessing : BaseAttainedBonus
{
    [Header("")]
    public List<BaseStatBlessingEffect> FlatEffectList = new();
    public BlessingTriggerRule ConditionalEffects;
    [Tooltip("Time Before Events Will Trigger Another Stat Change")]
    public float Cooldown = 0f;
    private RunDataManager rundata => GameManager.Instance.runData;
    private Action<PlayerEventContext> BlessingEffectDelegate;
    public void ApplyEffects()
    {
        foreach (var effect in FlatEffectList)
        {
            effect.ApplyEffect();
        }
    }
    public void RemoveEffects()
    {
        foreach (var effect in FlatEffectList)
        {
            effect.RemoveEffect();
        }
    }
    public void AttachBlessingTempEffects()
    {
        BlessingEffectDelegate = TriggerConditionalEffects;
        foreach (var Event in ConditionalEffects.TriggerEvents)
        {
            PlayerEffectSubscriptionManager.Instance.SubscribeToPlayerEvent(BlessingEffectDelegate, Event);
        }
    }
    public void DeAttachBlessingTempEffects()
    {
        foreach (var Event in ConditionalEffects.TriggerEvents)
        {
            PlayerEffectSubscriptionManager.Instance.UnSubscribeFromPlayerEvent(BlessingEffectDelegate, Event);
        }
    }
    public void TriggerConditionalEffects(PlayerEventContext ctx)
    {
        if (rundata.CanBlessingTrigger(this))
        {
            foreach (var effect in ConditionalEffects.Effects)
            {
                effect.ApplyEffect();
                rundata.BlessingEffectActive(this, effect.Duration);
            }

            //Put Blessing On Cooldown
            rundata.BeginBlessingCooldown(this);
        }
    }
    public List<TimedBlessingEffect> GetTimedEffectList()
    {
        List<TimedBlessingEffect> TimedEffects = new();
        foreach (StatBlessingEffect_ConditionalStatChange Effect in ConditionalEffects.Effects)
        {
            TimedBlessingEffect timedBlessing = new TimedBlessingEffect
            {
                DisableEffect = Effect.RemoveEffect,
                Duration = Effect.Duration,
                IsActive = false,
            };
            TimedEffects.Add(timedBlessing);
        }

        return TimedEffects;
    }
    public override void DisplayStatsOfBonusInInventory(InventoryDescriptionUI inventoryObj)
    {
        
    }
}
public abstract class BaseStatBlessingEffect : ScriptableObject { public abstract void ApplyEffect(); public abstract void RemoveEffect(); }
public abstract class StatBlessingEffect_CustomEventTriggered : BaseStatBlessingEffect
{
    [Tooltip("Every Event in Group Will Trigger All Stat Changes, Then Begin Cooldown")]
    [SerializeField] private BoonEventType[] TriggerEvents;
    protected abstract void TriggerEffect(PlayerEventContext ctx);
}
[System.Serializable]
public class BlessingTriggerRule
{
    public BoonEventType[] TriggerEvents;
    public List<StatBlessingEffect_ConditionalStatChange> Effects;
}
