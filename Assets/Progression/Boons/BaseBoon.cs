using System;
using UnityEngine;

public abstract class BaseBoon : ScriptableObject
{
    [Header("Boon Stats")]
    public string boonName;
    [TextArea] public string boonDescription;
    //public Sprite Icon;

    //Restrictions and Specifics
    [Header("Event and Restrictions")]
    [Tooltip("Event That Will Trigger This Boons Effect")]
    public BoonEventType EventToAttach;

    [Tooltip("Effect can Only be Triggered by Events Passing This Element Type (None = All Elements)")]
    public ElementType ElementRestriction;

    private Action<PlayerEventContext> effectDelegate;


    public virtual void Effect(PlayerEventContext context)
    {
        //OVERRIDE WITH SPECIFIC EFFECT LIBRARY
    }


    //Accessing Boon Level
    protected RunDataManager runData => GameManager.Instance.runData;
    protected int Level => runData.GetBoonLevel(this);
    
    public virtual void BoonCollected()
    {
        //OVERRIDE AND ADD COMMAND FOR PLACING BOON/RELIC/OTHER COLLECTABLE INTO CORRECT STORAGE
    }
    public virtual void BoonSelected()
    {
        effectDelegate = Effect;
        PlayerEffectSubscriptionManager.Instance.SubscribeToPlayerEvent(effectDelegate, EventToAttach);
    }
    public virtual void BoonRemoved()
    {
        effectDelegate = Effect;
        PlayerEffectSubscriptionManager.Instance.UnSubscribeFromPlayerEvent(effectDelegate, EventToAttach);
    }

}

