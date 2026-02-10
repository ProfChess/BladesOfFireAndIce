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
        RelicEffectLibrary.PlayRelicEffect(this, context);
    }
    public override void BoonCollected()
    {
        runData.AddRelic(this);
        StatChangeEventContext StartContext = PlayerEffectSubscriptionManager.Instance.GetPlayerState(PlayerStateNeeded);
        RelicEffectLibrary.PlayRelicEffect(this, StartContext);
    }
}
public enum RelicEffectType { HealthBuff, DamageBuff, AttackSpeedBuff, MoveSpeedBuff}

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