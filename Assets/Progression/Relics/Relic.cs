using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Relics/Relic"))]
public class Relic : BaseBoon
{
    public RelicEffectType EffectType;
    public PlayerStateCheckType PlayerStateNeeded;

    [Header("Attributes")]
    public RelicBaseStats BaseStats;

    public override void Effect(PlayerEventContext context)
    {
        if (runData.CanRelicTrigger(this))
        {
            RelicEffectLibrary.PlayRelicEffect(this, context);
            runData.BeginRelicCooldown(this);
        }
    }
    public override void BoonCollected()
    {
        runData.AddRelic(this);

        if (PlayerStateNeeded == PlayerStateCheckType.None) { return; }

        //Create Starting Context for Relics That Need it
        StatChangeEventContext StartContext = PlayerEffectSubscriptionManager.Instance.GetPlayerState(PlayerStateNeeded);
        RelicEffectLibrary.PlayRelicEffect(this, StartContext);
    }
}
public enum RelicEffectType { FireShieldBuff = 0, IceShieldBuff = 1, HealthBuff = 2, 
    DamageBuff = 3, AttackSpeedBuff = 4, MoveSpeedBuff = 5}

[System.Serializable]
public class RelicBaseStats
{
    [Header("Base Stats")]
    [Tooltip("Amount of a Bonus Provided by Relic")]
    public float PercentageIncrease = 0f;
    [Tooltip("Time of Each Instance of Buff")]
    public float Duration = 0f;
    [Tooltip("Time Before Relic Can Trigger Again")]
    public float Cooldown = 0f;
}