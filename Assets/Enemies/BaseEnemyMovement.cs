using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemyMovement : MonoBehaviour
{
    //Base Attributes
    [Tooltip("Speed Enemy Can Move When using This Movement Type")]
    [SerializeField] private float MovementSpeed = 1f;
    public float GetMoveSpeed => MovementSpeed;
    public abstract void EnemyMove(NavMeshAgent agent, float speed);

    protected bool isArrived(NavMeshAgent agent)
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    //Extra Helpful Functions
    //Gets spot on mesh from coordinates
    protected Vector2 GetPlayerLocation()
    {
        if (GameManager.Instance == null) { return Vector2.zero; }
        return GameManager.Instance.getPlayer().transform.position;
    }
    //Converts Vector Point into Location on NavMesh
    protected Vector2 GetPointOnMesh(Vector2 Point)
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

        return GetPointOnMesh(RandomDirection);
    }

    //Gets Random Point At Max Range
    protected Vector2 GetMaxRangePointAroundEnemy(float Range)
    {
        Vector3 RandomPointToFlee = Random.insideUnitCircle.normalized * Range;
        RandomPointToFlee += transform.position;

        return GetPointOnMesh(RandomPointToFlee);
    }

    //Picks random spot on the navmesh within enemy starting room
    protected Vector2 GetPointWithinStartingRoom(RectInt roomSpace)
    {
        Vector3 RandomPointInRoom = new Vector2(Random.Range(roomSpace.xMin, roomSpace.xMax),
                                                Random.Range(roomSpace.yMin, roomSpace.yMax));
        return GetPointOnMesh(RandomPointInRoom);
    }
}
