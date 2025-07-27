using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmashAbility : BasePlayerAbility
{
    [SerializeField] private float Radius;
    [SerializeField] private float Duration;

    [Header("References")]
    [SerializeField] private CircleCollider2D Hitbox;
    [SerializeField] private Animator EffectAnim;
    [SerializeField] private SpriteRenderer EffectSprite;

    //Animation Strings 
    private static readonly int AbilityTrigger = Animator.StringToHash("UseTrigger");

    private void Start()
    {
        Hitbox.enabled = false;
        Hitbox.radius = Radius;
    }
    protected override void AbilityEffect()
    {
        StartCoroutine(AbilityDuration());
    }
    protected override void AbilityAnimation()
    {
        EffectAnim.SetTrigger(AbilityTrigger);
    }
    private IEnumerator AbilityDuration()
    {
        Hitbox.enabled = true;
        yield return new WaitForSeconds(Duration);
        Hitbox.enabled = false;
    }

}
