using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAttack : BaseAttackDamage
{
    protected BaseBoss BossRef;
    private Coroutine Cooldown;
    private IEnumerator CooldownRoutine(BossAttackOption AttackOption)
    {
        AttackOption.OnCooldown = true;
        yield return new WaitForSeconds(AttackOption.AttackCooldown);
        AttackOption.OnCooldown = false;
        Cooldown = null;
    }
    public virtual void StartAttack(BossAttackOption AttackOption)
    {
        //INPUT ATTACK LOGIC

        if (Cooldown == null)
        {
            Cooldown = StartCoroutine(CooldownRoutine(AttackOption));
        }
    }
    private void Start()
    {
        Transform tempObject = GetComponent<Transform>();
        BossRef = tempObject.GetComponentInParent<BaseBoss>();
    }
}
