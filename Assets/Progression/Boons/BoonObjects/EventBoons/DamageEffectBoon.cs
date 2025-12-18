using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Boons/EventBoon"))]
public class DamageEffectBoon : BaseBoon
{
    //Damage of Effect
    public float Damage = 0f;

    [Tooltip("Effect of This Boon")]
    public DamageBoonEffectType EffectType;

    [Tooltip("Event That Will Trigger This Boons Effect")]
    public BoonEventType EventToAttach;

    private Action<AttackEventDetails> effectDelegate;

    public override void BoonSelected()
    {
        effectDelegate = Effect;
        PlayerBoonManager.Instance.SubscribeToPlayerEvent(effectDelegate, EventToAttach);
    }
    public override void BoonRemoved()
    {
        if (effectDelegate != null)
        {
            PlayerBoonManager.Instance.UnSubscribeFromPlayerEvent(effectDelegate, EventToAttach);
        }
    }
    public virtual void Effect(AttackEventDetails AttackDetails)
    {
        BoonEffectLibrary.PlayBoonEffect(this, EffectType, AttackDetails);
    }
}
//Boon Type Specific Enum
public enum DamageBoonEffectType { FireBoom, FireBurst, IceBoom}

