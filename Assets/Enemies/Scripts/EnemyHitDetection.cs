using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : BaseHealth
{
    [SerializeField] private BaseEnemy MainEnemyScript;
    [SerializeField] protected Collider2D HitBox;


    //Health and Damage Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HitBox != null)
        {
            if (collision.CompareTag("PlayerAttack"))
            {
                TakeDamage(collision.GetComponentInParent<PlayerAttack>().GetDamageNumber());
                if (curHealth > 0) { MainEnemyScript.GetAnim().Play("Hurt", 1); }
                else { MainEnemyScript.GetAnim().Play("Death", 1); }
            }
        }
    }
}
