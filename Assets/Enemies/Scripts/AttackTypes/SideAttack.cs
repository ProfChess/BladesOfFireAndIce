using System.Collections;
using UnityEngine;

public class SideAttack : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] private BoxCollider2D attackBox;
    
    public void Attack(float Damage, float Range, int Cooldown, float Offset, Transform playerTransform)
    {
        //Attack to the Right
        if (playerTransform.position.x >= gameObject.transform.position.x)
        {
            attackBox.offset = new Vector2(1, 0);
        }
        //Attack to the Left
        else
        {
            attackBox.offset = new Vector2(-1, 0);
        }

        attackBox.enabled = true;
        StartCoroutine(AttackBoxPersist());
    }
    private IEnumerator AttackBoxPersist()
    {
        yield return new WaitForSeconds(0.2f);
        attackBox.enabled = false;
    }

}
