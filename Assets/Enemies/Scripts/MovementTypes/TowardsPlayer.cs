using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseEnemy))]
public class TowardsPlayer : MonoBehaviour, IEnemyMovementBehaviour
{
    public void Move(NavMeshAgent agent, Transform playerTransform, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(playerTransform.position);
    }
}
