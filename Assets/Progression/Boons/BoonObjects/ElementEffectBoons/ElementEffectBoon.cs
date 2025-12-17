using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Boons/ElementEffectBoon"))]
public class ElementEffectBoon : BaseBoon
{
    //Damage of Effect
    public float Damage = 0f;

    [Tooltip("Effect of This Boon")]
    public ElementBoonEffectType EffectType;

    [Tooltip("Event That Will Trigger This Boons Effect")]
    public BoonEventType EventToAttach;

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
        BoonEffectLibrary.PlayElementEffectBoon(this, EffectType, Element);
    }
}

//Boon Type Specific Enum
public enum ElementBoonEffectType { FireBurst, IcicleSwirl}