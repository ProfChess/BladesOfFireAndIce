using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyMovement : MonoBehaviour
{
    //Gets spot on mesh from coordinates
    protected Vector2 GetPointOnMesh(Vector3 Point)
    {
        if (NavMesh.SamplePosition(Point, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }

    //Picks random spot on the navmesh within a circle around enemy
    protected Vector2 GetPointAroundEnemy(float Radius) 
    {
        Vector3 RandomDirection = Random.insideUnitCircle * Radius;
        RandomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(RandomDirection, out hit, Radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }

    //Gets Random Point At Max Range
    protected Vector2 GetMaxRangePointAroundEnemy(float Range)
    {
        Vector3 RandomPointToFlee = Random.insideUnitCircle.normalized * Range;
        RandomPointToFlee += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(RandomPointToFlee, out hit, Range, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }

    //Picks random spot on the navmesh within enemy starting room
    protected Vector2 GetPointWithinStartingRoom(BoundsInt roomSpace)
    {
        Vector3 RandomDirection = new Vector2(Random.Range(roomSpace.xMin, roomSpace.xMax), 
                                                Random.Range(roomSpace.yMin, roomSpace.yMax));

        if (NavMesh.SamplePosition(RandomDirection, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }
}

[RequireComponent(typeof(BaseEnemy))]
public class TowardsPlayer : BaseEnemyMovement, IEnemyMovementBehaviour
{
    [Header("Wandering")]
    [SerializeField] private int WanderRadius = 3;

    public void IdleMove(NavMeshAgent agent, float speed)
    {
        agent.speed = speed;
        Vector2 PointOfInterest = GetPointAroundEnemy(WanderRadius);
        agent.SetDestination(PointOfInterest);
        if (PointOfInterest != Vector2.zero) {  }
    }

    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        agent.speed = speed;
        agent.stoppingDistance = range;
        agent.SetDestination(playerTransform.position);
    }

}
