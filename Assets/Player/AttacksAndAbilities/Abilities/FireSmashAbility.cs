using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSmashAbility : BasePlayerAbility
{
    [SerializeField] private float Radius;
    [SerializeField] private float Duration;
    [SerializeField] private CircleCollider2D Hitbox;

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
        
    }
    private IEnumerator AbilityDuration()
    {
        Hitbox.enabled = true;
        yield return new WaitForSeconds(Duration);
        Hitbox.enabled = false;
    }

}
