using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossAttack : BaseAttackDamage
{
    [Header("Cooldown")]
    [SerializeField] protected float AttackCooldown = 3f;
    private bool OnCooldown = false;
    private Coroutine CooldownRoutine;

    private IEnumerator AttackCooldownRoutine()
    {
        OnCooldown = true;
        yield return new WaitForSeconds(AttackCooldown);
        OnCooldown = false;
    }


    protected virtual void BeginCooldown()
    {
        CooldownRoutine = StartCoroutine(AttackCooldownRoutine());
    }
    protected bool IsOnCooldown() { return OnCooldown; }

}
