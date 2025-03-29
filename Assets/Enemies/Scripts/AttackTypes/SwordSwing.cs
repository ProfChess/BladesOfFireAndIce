using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class SwordSwing : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] BoxCollider2D AttackHitbox;
    public void Attack(float Damage, float Range, int Cooldown, int Speed, Transform playerTransform)
    {
        AttackDamage = Damage;
        Vector2 AttackDirection = GetPlayerDirection(playerTransform).normalized * Range;
        float rotateAngle = Mathf.Atan2(AttackDirection.y, AttackDirection.x) * Mathf.Rad2Deg;                //Angle

        //Place and Rotate Attack Box
        AttackHitbox.transform.localPosition = AttackDirection;
        AttackHitbox.transform.localRotation = Quaternion.Euler(0, 0, rotateAngle);
        AttackHitbox.enabled = true;
        StartCoroutine(AttackBoxDuration());
    }
    private Vector2 GetPlayerDirection(Transform playerSpot)
    {
        return playerSpot.position - transform.position;
    }
    private IEnumerator AttackBoxDuration() { yield return new WaitForSeconds(0.5f); AttackHitbox.enabled = false; }
}
