using System;
using UnityEngine;

public class PlayerAttackCalcs : BasePlayerDamage
{
    [SerializeField] private Collider2D AttackBox;

    //Events For Damaging
    public event Action<PlayerEventContext> OnEnemyHitNormalAttack;
    private AttackEventContext EnemyHitContext = new();

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


        //Setup Context and Trigger Events
        EnemyHitContext.Setup(GetElement(), gameObject.transform.right, EnemyHealth, AttackBox.transform.position);

        OnEnemyHitNormalAttack?.Invoke(EnemyHitContext);
        if (EnemyHealth.CurrentHealth <= AttackDamage) { OnEnemyDeathNormalAttack?.Invoke(EnemyHitContext); }

        float CritChance = UnityEngine.Random.value;
        if (CritChance < crit)
        {
            Debug.Log("Crit");
            Debug.Log("Chance: " + crit);
            OnCriticalHitNormalAttack?.Invoke(EnemyHitContext);
            return (AttackDamage * 2);
        }
        else
        {
            return AttackDamage;
        }
    }

}
