using UnityEngine;

public abstract class EnemyDamage : MonoBehaviour
{
    //Visual
    [SerializeField] Animator AttackAnim;
    [SerializeField] SpriteRenderer AttackSprite;

    protected void StartAttackAnim() //Play the attack hit effect
    {
        AttackAnim.Play("AttackEffectAnim");
    }
    protected float AttackDamage {get; set;}

    public float GetDamage() {  return AttackDamage; }


}
