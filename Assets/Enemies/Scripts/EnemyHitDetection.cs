using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDetection : BaseHealth
{
    [SerializeField] protected Collider2D HitBox;
    
    [Header("References")]
    [SerializeField] private BaseEnemy MainEnemyScript;
    [SerializeField] private HitFlash HF;


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
            //Detect Which Damage Component Is Hurting the Enemy
            if (collision.TryGetComponent(out BasePlayerDamage DamageScript))
            {
                TakeDamage(DamageScript.GetAttackDamage(this));
                EvaluateDeath();
            }
            else if (collision.TryGetComponent(out BaseEffectSpawn EffectScript))
            {
                //Checks Colliding Object if This Collision Should be Ignored
                if(EffectScript.ignoredEnemy == gameObject) { return; }
                TakeDamage(EffectScript.GetDamage());
                EvaluateDeath();
            }

            //Knockback
            if (gameObject.TryGetComponent(out Knockback knockback))
            {
                if (collision.transform.parent.TryGetComponent(out PlayerAttack attack))
                {
                    HitData data = attack.hitData;
                    knockback.KnockbackObject(data.KnockBackDirection, data.KnockBackPower); 
                }
            }
        }
    }
    //Checks if the enemy is dead or still alive -> Plays animation accordingly
    private void EvaluateDeath()
    {
        if (curHealth > 0) { HitReaction(); }
        else if (MainEnemyScript != null)
        {
            MainEnemyScript.GetAnimator().Play(EnemyDeath, 1);
            MainEnemyScript.canMove = false;
            Invoke(nameof(TurnOff), 1.2f);
        }
    }
    private void HitReaction()
    {
        HF.Flash();
        GameManager.Instance.hitStopManager.BeginHitStop();
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
        if (MainEnemyScript != null)
        {
            MainEnemyScript.DeactivateEnemy();
        }
    }

}
public struct HitData
{
    public Vector2 KnockBackDirection;
    public float KnockBackPower;
    public void SetData(Vector2 KnockDir, float KnockPower) 
    { 
        KnockBackDirection = KnockDir;
        KnockBackPower = KnockPower;
    }
}
