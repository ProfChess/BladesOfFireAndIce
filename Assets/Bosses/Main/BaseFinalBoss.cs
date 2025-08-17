using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFinalBoss : BaseBoss
{
    //Phases
    public enum Phase { Phase1, Phase2, Phase3 }
    protected Phase CurrentPhase;
    public Phase GetPhase() {  return CurrentPhase; }
    protected override void Start() //Start at First Phase
    {
        base.Start();
        CurrentPhase = Phase.Phase1;
    }
    protected void ChangePhase() //Go to Next Phase
    {
        if (CurrentPhase == Phase.Phase3) { return; }

        if (CurrentPhase == Phase.Phase1) { CurrentPhase = Phase.Phase2; return; }

        else if (CurrentPhase == Phase.Phase2) { CurrentPhase = Phase.Phase3; return; }
    }

    //Sub to Health Event
    private void OnEnable()
    {
        FinalBossHealth.HealthBelowThreshold += ChangePhase;
    }
    private void OnDisable()
    {
        FinalBossHealth.HealthBelowThreshold -= ChangePhase;
    }
}
