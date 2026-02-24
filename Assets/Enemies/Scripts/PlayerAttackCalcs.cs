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
    //Damage
    public float FireAttackDamage = 1f;
    public void SetFireDamage(float num) {  FireAttackDamage = num; }
    public float IceAttackDamage = 1f;
    public void SetIceDamage(float num) {  IceAttackDamage = num; }

    //Crit
    private float crit = 0f;
    public void SetCritChance(float num) { crit = num; }
    public void SetDamage(float num) { AttackDamage = num; }

    //Area
    private float FireAreaMult = 1f;
    public void SetFireAOE(float num) { FireAreaMult = num; ApplyAttackAreaSize(); }
    private float IceAreaMult = 1f; 
    public void SetIceAOE(float num) { IceAreaMult = num; ApplyAttackAreaSize(); }
    public void ApplyAttackAreaSize() 
    {
        float AreaScalar = PlayerController.PlayerAttackForm == ElementType.Fire ? FireAreaMult : IceAreaMult;
        gameObject.transform.localScale = new Vector3(AreaScalar, AreaScalar, 1);
    }

    public override float GetAttackDamage(BaseHealth EnemyHealth)
    {
        if (PlayerController.PlayerAttackForm == ElementType.Fire)
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



        //Calculate and Return Crit
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
