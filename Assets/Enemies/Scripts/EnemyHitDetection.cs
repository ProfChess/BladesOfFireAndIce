using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : BaseHealth
{
    [SerializeField] protected Collider2D HitBox;
    
    [Header("References")]
    [SerializeField] private BaseEnemy MainEnemyScript;


    //Box Flipping
    private bool BoxShouldBeFlipped = false;
    private bool BoxIsFlipped = false;

    //Animations
    private static readonly int EnemyHurt = Animator.StringToHash("Hurt");
    private static readonly int EnemyDeath = Animator.StringToHash("Death");


    //Health and Damage Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HitBox != null)
        {
            if (collision.CompareTag("PlayerAttack"))
            {
                BasePlayerDamage DamageScript = collision.GetComponent<BasePlayerDamage>();
                if (DamageScript.AttackElement == BasePlayerDamage.PlayerAttackType.NormalAttack)
                {
                    TakeDamage(DamageScript.GetDamageNumber());
                }
                else if (DamageScript.AttackElement == BasePlayerDamage.PlayerAttackType.Ability)
                {
                    TakeDamage(DamageScript.GetDamageWithoutCrit());
                }

                if (curHealth > 0) { MainEnemyScript.GetAnimator().Play(EnemyHurt, 1); }
                else
                {
                    MainEnemyScript.GetAnimator().Play(EnemyDeath, 1);
                    MainEnemyScript.canMove = false;
                    Invoke(nameof(TurnOff), 1.2f);
                }
            }
        }
    }

    private void Update()
    {
        if (MainEnemyScript != null)
        {
            if (MainEnemyScript.GetSpriteRenderer().flipX) { BoxShouldBeFlipped = true; }
            else { BoxShouldBeFlipped = false; }

            if (BoxShouldBeFlipped && !BoxIsFlipped) { FlipHitBox(); BoxIsFlipped = true; }
            if (!BoxShouldBeFlipped && BoxIsFlipped) { FlipHitBox(); BoxIsFlipped = false; }
        }
    }
    private void FlipHitBox()
    {
        if (HitBox != null)
        {
            HitBox.offset = new Vector2(HitBox.offset.x * -1, HitBox.offset.y);
        }
    }

    private void TurnOff()
    {
        MainEnemyScript.DeactivateEnemy();
    }
}
