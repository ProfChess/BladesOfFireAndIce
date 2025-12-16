using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Boons/DamageEffectBoon"))]
public class DamageEffectBoon : BaseBoon
{
    [Tooltip("Effect of This Boon")]
    public DamageBoonEffectType EffectType;

    [Tooltip("Event That Will Trigger This Boons Effect")]
    public BoonEventType EventToAttach;

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
        BoonEffectLibrary.PlayDamageEffectBoon(this, EffectType, Element, target);
    }
}
//Boon Type Specific Enum
public enum DamageBoonEffectType { FireBoom, IceBoom}

