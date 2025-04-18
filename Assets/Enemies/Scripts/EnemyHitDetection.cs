using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitDetection : BaseHealth
{
    [SerializeField] private BaseEnemy MainEnemyScript;
    [SerializeField] protected Collider2D HitBox;

    private bool BoxShouldBeFlipped = false;
    private bool BoxIsFlipped = false;
    //Health and Damage Detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HitBox != null)
        {
            if (collision.CompareTag("PlayerAttack"))
            {
                TakeDamage(collision.GetComponentInParent<PlayerAttack>().GetDamageNumber());
                if (curHealth > 0) { MainEnemyScript.GetAnimator().Play("Hurt", 1); }
                else { MainEnemyScript.GetAnimator().Play("Death", 1); MainEnemyScript.canMove = false; }
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


}
