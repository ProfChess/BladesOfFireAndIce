using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoon : ScriptableObject
{
    public string boonName;
    public abstract void BoonSelected();
    public abstract void BoonRemoved();
}

[CreateAssetMenu(menuName ="Boons/StatBoostBoon")]
public class NumberBoostBoon : BaseBoon
{
    public StatType Stat;
    public float BoonValue;
    public override void BoonSelected()
    {
        PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValue);
    }
    public override void BoonRemoved()
    {
        PlayerBoonManager.Instance.UndoChange(Stat, BoonValue);
    }
}

[CreateAssetMenu(menuName = "Boons/DamageEffectBoon")]
public class DamageEffectBoon : BaseBoon
{
    [Tooltip("Event That Will Trigger This Boons Effect")]
    BoonEventType EventToAttach;

    private Action<ElementType, BaseHealth> effectDelegate;

    public override void BoonSelected()
    {
        effectDelegate = Effect;
        PlayerBoonManager.Instance.SubscribeToDamageEvent(effectDelegate, EventToAttach);
    }
    public override void BoonRemoved()
    {
        if (effectDelegate != null)
        {
            PlayerBoonManager.Instance.UnSubscribeFromDamageEvent(effectDelegate, EventToAttach);
        }
    }
    public virtual void Effect(ElementType Element, BaseHealth target)
    {
        //INSERT SPECIFIC BOON EFFECT
    }
}

[CreateAssetMenu(menuName = "Boons/ElementEffectBoon")]
public class ElementEffectBoon : BaseBoon
{
    [Tooltip("Event That Will Trigger This Boons Effect")]
    BoonEventType EventToAttach;

    private Action<ElementType> effectDelegate;

    public override void BoonSelected()
    {
        effectDelegate = Effect;
        PlayerBoonManager.Instance.SubscribeToElementEvent(effectDelegate, EventToAttach);
    }
    public override void BoonRemoved()
    {
        if (effectDelegate != null)
        {
            PlayerBoonManager.Instance.UnSubscribeFromElementEvent(effectDelegate, EventToAttach);
        }
    }
    public virtual void Effect(ElementType Element)
    {
        //INSERT SPECIFIC BOON EFFECT
    }
}
