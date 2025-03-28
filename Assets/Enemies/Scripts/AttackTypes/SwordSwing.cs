using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class SwordSwing : MonoBehaviour, IEnemyAttackBehaviour
{
    [SerializeField] BoxCollider2D AttackHitbox;
    public void Attack(float Damage, float Range, int Cooldown, int Speed, Transform playerTransform)
    {
        
    }
}
