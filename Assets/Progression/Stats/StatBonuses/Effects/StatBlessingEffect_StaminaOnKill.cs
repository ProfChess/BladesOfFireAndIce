using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Blessings/BlessingEffect/StaminaGainedOnKill"))]
public class StatBlessingEffect_StaminaOnKill : StatBlessingEffect_EventTriggered
{
    [SerializeField] private float StaminaPercentageGainedPerKill = 5f;
    public override void ApplyEffect()
    {
        SubToEvents(StaminaGainedOnKill);
    }
    private void StaminaGainedOnKill(PlayerEventContext ctx)
    {
        PlayerEffectSubscriptionManager.Instance.RestoreStat(StatType.Endurance, StaminaPercentageGainedPerKill);
    }
}
