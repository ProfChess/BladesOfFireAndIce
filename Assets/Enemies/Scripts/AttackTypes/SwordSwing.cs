using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class SwordSwing : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] protected Collider2D AttackCollider;
    [SerializeField] protected Animator AttackEffectAnim;
    public void Attack(float Damage, float Range, int Cooldown, float Offset, Transform playerTransform)
    {
        //Assign Var
        AttackDamage = Damage;

        //Get Direction and Angle For Attack
        Vector2 AttackDirection = GetPlayerDirection(playerTransform).normalized * Offset;
        float rotateAngle = Mathf.Atan2(AttackDirection.y, AttackDirection.x) * Mathf.Rad2Deg;                //Angle

        //Place and Rotate Attack Box
        AttackCollider.transform.localPosition = AttackDirection;
        AttackCollider.transform.localRotation = Quaternion.Euler(0, 0, rotateAngle);
        AttackCollider.enabled = true;

        //Play Attack Effect and Delete Box
        AttackEffectAnim.Play("AttackEffect");
        StartCoroutine(AttackBoxDuration());
    }

    private IEnumerator AttackBoxDuration() { yield return new WaitForSeconds(0.1f); AttackCollider.enabled = false; }
}
