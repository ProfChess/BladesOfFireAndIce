using NavMeshPlus.Extensions;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class ChargeAttack : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] private Collider2D AttackBox;
    [SerializeField] private Animator anim;

    //Charge Attack Settings
    [Header("Charge Settings")]
    private float chargeDuration;

    public void Attack(float Damage, float Range, int Cooldown, float Speed, Transform playerTransform)
    {
        AttackDamage = Damage;
        chargeDuration = Range;

        if (AttackBox.enabled == false)
        {
            AttackBox.enabled = true;
            StartCoroutine(ChargeAttackStart());
        }
        
    }
    
    //Stays in Charge anim for Duration, then starts cooldown
    private IEnumerator ChargeAttackStart()
    {
        yield return new WaitForSeconds(chargeDuration);
        anim.SetTrigger("FinishSpinAttack");
        TurnOffSpin();
    }
    private void TurnOffSpin()
    {
        AttackBox.enabled = false;
    }
}
