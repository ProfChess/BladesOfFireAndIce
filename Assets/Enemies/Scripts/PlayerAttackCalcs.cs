using System;
using UnityEngine;

public class PlayerAttackCalcs : BasePlayerDamage
{
    [SerializeField] private Collider2D AttackBox;

    //Events For Damaging
    public event Action<PlayerEventContext> OnEnemyHitNormalAttack;
    public event Action<PlayerEventContext> OnEnemyDeathNormalAttack;
    public event Action<PlayerEventContext> OnCriticalHitNormalAttack;

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

        AttackEventContext details = new AttackEventContext
        {
            Target = EnemyHealth,
            Element = GetElement(),
            AttackBoxOrigin = AttackBox.transform.position,
            Direction = gameObject.transform.right
        };

        OnEnemyHitNormalAttack?.Invoke(details);
        if (EnemyHealth.CurrentHealth <= AttackDamage) { OnEnemyDeathNormalAttack?.Invoke(details); }

        float CritChance = UnityEngine.Random.value;
        if (CritChance < crit)
        {
            Debug.Log("Crit");
            Debug.Log("Chance: " + crit);
            OnCriticalHitNormalAttack?.Invoke(details);
            return (AttackDamage * 2);
        }
        else
        {
            return AttackDamage;
        }
    }

}
