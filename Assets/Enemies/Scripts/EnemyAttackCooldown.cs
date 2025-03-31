using UnityEngine;

public class EnemyAttackCooldown : MonoBehaviour
{
    [SerializeField] BaseEnemy Enemy;

    //Begins enemy attack cooldown
    public void AttackCooldown()
    {
        Enemy.StartEnemyCooldown();
    }
    
    //Spawns the damage trigger box of enemys attack (called at specific frame)
    public void AttackDamageCall()
    {
        Enemy.StartEnemyAttackDamage();
    }

    //Enemy death notification
    public void DeathAnimOver()
    {
        Enemy.DeactivateEnemy();
    }
}
