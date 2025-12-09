using System;
using UnityEngine;

public class PlayerAttackCalcs : BasePlayerDamage
{
    //Events For Damaging
    public event Action<ElementType, BaseHealth> OnEnemyHitNormalAttack;
    public event Action<ElementType, BaseHealth> OnEnemyDeathNormalAttack;
    public event Action<ElementType, BaseHealth> OnCriticalHitNormalAttack;

    //Quick References
    protected ElementType attackForm => PlayerController.PlayerAttackForm;
    
    private float FireAttackDamage = 1f;
    public void SetFireDamage(float num) {  FireAttackDamage = num; }
    private float IceAttackDamage = 1f;
    public void SetIceDamage(float num) {  IceAttackDamage = num; }


    private float crit = 0f;
    public void SetCritChance(float num) { crit = num; }
    public void SetDamage(float num) { AttackDamage = num; }

    public override float GetAttackDamage(BaseHealth EnemyHealth)
    {
        ElementType Form = attackForm;
        if (Form == ElementType.Fire)
        {
            AttackDamage = FireAttackDamage;
        }
        else
        {
            AttackDamage = IceAttackDamage;
        }


        //Trigger Events
        OnEnemyHitNormalAttack?.Invoke(GetElement(), EnemyHealth);
        if (EnemyHealth.CurrentHealth <= AttackDamage) { OnEnemyDeathNormalAttack?.Invoke(GetElement(), EnemyHealth); }

            float CritChance = UnityEngine.Random.value;
        if (CritChance < crit)
        {
            OnCriticalHitNormalAttack?.Invoke(GetElement(), EnemyHealth);
            return (AttackDamage * 2);
        }
        else
        {
            return AttackDamage;
        }
    }

}
