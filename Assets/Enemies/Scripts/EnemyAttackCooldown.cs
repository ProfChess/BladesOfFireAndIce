using UnityEngine;

public class EnemyAttackCooldown : MonoBehaviour
{
    [SerializeField] BaseEnemy Enemy;
    public void AttackCooldown()
    {
        Enemy.StartEnemyCooldown();
    }
}
