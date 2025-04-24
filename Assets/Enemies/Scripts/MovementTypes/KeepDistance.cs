using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BaseEnemy))]
public class KeepDistance : BaseEnemyMovement, IEnemyMovementBehaviour
{
    private BoundsInt Territory;
    private const float MAX_WANDER_DISTANCE = 8f;

    private void Start()
    {
        Territory = DungeonInfo.Instance.GrabArea(transform.position);
    }
    public void IdleMove(NavMeshAgent agent, float speed)
    {
        //Set Speed
        agent.speed = speed;

        //Point Agent Will Move To
        Vector2 PointOfInterest = GetPointWithinStartingRoom(Territory);

        //Shorten Vector if Too Long
        Vector2 Distance = PointOfInterest - (Vector2)transform.position;
        if (Distance.magnitude > MAX_WANDER_DISTANCE) 
        { 
            Distance = Distance.normalized * MAX_WANDER_DISTANCE;
            PointOfInterest = GetPointOnMesh((Vector2)transform.position + Distance);
        }
        
        if (PointOfInterest != Vector2.zero) { agent.SetDestination(PointOfInterest); }
    }
    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        //Sets Speed
        agent.speed = speed;

        //Creates Point For Ranger to Flee to
        Vector2 PointToFlee;

        //Casts in opposite Direction of Player to Check for Walls
        RaycastHit2D Ray = Physics2D.Raycast(gameObject.transform.position, -PlayerDirection(playerTransform), 
            range + 1, LayerMask.GetMask("Walls"));

        //If Opposite direction of Player is a wall -> Run to random valid spot at range 
        if (Ray.collider != null) { PointToFlee = GetMaxRangePointAroundEnemy(range); }
        //Otherwise run away from player
        else { PointToFlee = -PlayerDirection(playerTransform) * range; }
        agent.SetDestination(PointToFlee);
    }

    //Gets Direction to player
    private Vector2 PlayerDirection(Transform player)
    {
        return (player.position - transform.position).normalized;
    }
}
