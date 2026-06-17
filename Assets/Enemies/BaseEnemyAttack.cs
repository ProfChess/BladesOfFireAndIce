using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAttack : MonoBehaviour
{
    [Tooltip("Time Inbetween Each Basic Attack")]
    [SerializeField] protected float AttackCooldown;
    public float AttackCD => AttackCooldown;
    public float AttackChance;
    public bool canAttack = true;
    public bool attackInProgress = false;

    public abstract void Attack();
    //Cooldowns
    protected virtual IEnumerator BasicAttackCooldown()
    {
        canAttack = false;
        yield return GameTimeManager.WaitFor(AttackCD);
        canAttack = true;
    }
    protected virtual Vector2 GetPlayerLocation()
    {
        if (GameManager.Instance == null) { return Vector2.zero; }
        return GameManager.Instance.getPlayer().transform.position;
    }
    protected virtual Vector2 GetPlayerDirection()
    {
        if (GameManager.Instance == null) { return Vector2.zero; }
        return (GameManager.Instance.getPlayer().transform.position - transform.position).normalized;
    }
}
