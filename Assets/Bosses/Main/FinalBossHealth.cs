using System;
using UnityEngine;

public class FinalBossHealth : BossHealth
{
    [Header("Health Threshold")]
    [SerializeField] private float Phase2Threshold; //% of Health Remaining to Change to Phase 2
    [SerializeField] private float Phase3Threshold; //% of Health Remaining to Change to Phase 3

    //Event for Loosing Health
    public static event Action HealthBelowThreshold;
    [SerializeField] private BaseFinalBoss FinalBossScript;

    protected override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (!BossDefeated) 
        { 
            if (FinalBossScript.GetPhase() == BaseFinalBoss.Phase.Phase1) 
            { 
                if (curHealth < (MaxHealth * Phase2Threshold / 100))
                {
                    HealthBelowThreshold?.Invoke();
                }
            }
            else if (FinalBossScript.GetPhase() == BaseFinalBoss.Phase.Phase2)
            {
                if (curHealth < (MaxHealth * Phase3Threshold / 100))
                {
                    HealthBelowThreshold?.Invoke();
                }
            }
        }
    }
}
