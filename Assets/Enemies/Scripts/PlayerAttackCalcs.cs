using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCalcs : BaseAttackDamage
{
    [SerializeField] private PlayerController playerController;

    private float FireAttackDamage;
    public void SetFireDamage(float num) {  FireAttackDamage = num; }
    private float IceAttackDamage;
    public void SetIceDamage(float num) {  IceAttackDamage = num; }


    private float crit = 0f;
    public void SetCritChance(float num) { crit = num; }
    public void SetDamage(float num) { AttackDamage = num; }
    public override float GetDamageNumber()
    {
        PlayerController.AttackForm Form = playerController.GetAttackForm();
        if (Form == PlayerController.AttackForm.Fire)
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
}
