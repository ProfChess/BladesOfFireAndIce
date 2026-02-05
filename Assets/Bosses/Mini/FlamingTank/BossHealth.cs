using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : BaseHealth
{
    [Header("References")]
    [SerializeField] private Animator BossAnim;
    private bool isDead = false;
    public bool BossDefeated => isDead;

    //Anim
    private static readonly int HurtTrig = Animator.StringToHash("HurtTrigger");
    private static readonly int DeathTrig = Animator.StringToHash("DeathTrigger");

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!isDead)
        {
            //Detect Which Damage Component Is Hurting the Enemy
            if (col.TryGetComponent<BasePlayerDamage>(out BasePlayerDamage DamageScript))
            {
                TakeDamage(DamageScript.GetAttackDamage());
                EvaluateDeath();
            }
            else if (col.TryGetComponent<BaseEffectSpawn>(out BaseEffectSpawn EffectScript))
            {
                TakeDamage(EffectScript.GetDamage());
                EvaluateDeath();
            }
        }
    }
    protected void EvaluateDeath()
    {
        int Trigger = curHealth <= 0 ? DeathTrig : HurtTrig;
        BossAnim.SetTrigger(Trigger);
        if (curHealth <= 0) { isDead = true; }
    }

}
