using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCalcs : BasePlayerDamage
{    
    //Quick References
    protected ElementType attackForm => PlayerController.PlayerAttackForm;
    
    private float FireAttackDamage;
    public void SetFireDamage(float num) {  FireAttackDamage = num; }
    private float IceAttackDamage;
    public void SetIceDamage(float num) {  IceAttackDamage = num; }


    private float crit = 0f;
    public void SetCritChance(float num) { crit = num; }
    public void SetDamage(float num) { AttackDamage = num; }

    public override float GetDamageNumber()
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

        float CritChance = Random.value;
        if (CritChance < crit)
        {
            return (AttackDamage * 2);
        }
        else
        {
            return AttackDamage;
        }
    }

    public override float GetDamageWithoutCrit()
    {
        if (attackForm == ElementType.Fire)
        {
            return FireAttackDamage;
        }
        else { return IceAttackDamage; }
    }
}
