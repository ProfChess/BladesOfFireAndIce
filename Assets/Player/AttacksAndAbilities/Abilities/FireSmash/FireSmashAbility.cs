using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmashAbility : BasePlayerAbility
{
    [SerializeField] private float Radius;
    [SerializeField] private float Duration;

    [Header("Visuals")]
    [SerializeField] private Animator EffectAnim;
    [SerializeField] private SpriteRenderer EffectSprite;

    //Animation Strings 
    private static readonly int AbilityTrigger = Animator.StringToHash("UseTrigger");

    private void Start()
    {
        hitbox.enabled = false;
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
        hitbox.enabled = true;
        yield return new WaitForSeconds(Duration);
        hitbox.enabled = false;
    }

}
