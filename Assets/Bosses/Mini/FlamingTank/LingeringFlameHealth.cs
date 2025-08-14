using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringFlameHealth : BaseAOEHealth
{
    [SerializeField] private LingeringFlame Main;
    [SerializeField] private Animator FlameAnim;

    private static readonly int DeathTrig = Animator.StringToHash("FireDeath");
    protected override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        if (Main != null)
        {
            if (curHealth <= 0)
            {
                FlameAnim.SetTrigger(DeathTrig);
                Main.BeginFading();
            }
        }
    }
}
