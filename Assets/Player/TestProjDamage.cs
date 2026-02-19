using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjDamage : BaseDamageDetection
{
    [SerializeField] private Vector2 MoveDirection;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AttackDamage = 10f;
    [SerializeField] private Rigidbody2D rb;
    public override float GetAttackDamage()
    {
        return AttackDamage;
    }

    private void FixedUpdate()
    {
        rb.velocity = MoveDirection * MoveSpeed;
    }
}
