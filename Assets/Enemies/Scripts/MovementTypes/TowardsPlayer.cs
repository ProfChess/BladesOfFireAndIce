using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class TowardsPlayer : MonoBehaviour, IEnemyMovementBehaviour
{
    public void Move(Transform enemyTransform, Transform playerTransform, float speed)
    {
        enemyTransform.position = Vector2.MoveTowards
            (enemyTransform.position,
            playerTransform.position,
            speed * Time.deltaTime);
    }
}
