using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

[RequireComponent(typeof(BaseEnemy))]
public class PatrolRoom : BaseEnemyMovement, IEnemyMovementBehaviour
{
    private BoundsInt Territory;
    private BaseEnemy Enemy;

    //Patrol Points
    private Vector2 Point1; //Left or Bottom
    private Vector2 Point2; //Right or Top
    private Vector2 CurrentDestination;

    private void Start()
    {
        Enemy = GetComponent<BaseEnemy>();  

        Territory = DungeonInfo.Instance.GrabArea(transform.position).space;

        int Length = Territory.yMax - Territory.yMin;
        int Width = Territory.xMax - Territory.xMin;
        Vector2 middle = Territory.center;

        //Select 2 Patrol Points Based on Room Dimentions
        if (Length > Width) 
        {
            Point1 = new Vector2(middle.x, middle.y - Length / 4);
            Point2 = new Vector2(middle.x, middle.y + Length / 4);
        }
        else
        {
            Point1 = new Vector2(middle.x - Length / 4, middle.y);
            Point2 = new Vector2(middle.x + Length / 4, middle.y);
        }
        CurrentDestination = Point1;
    }
    public void IdleMove(NavMeshAgent agent, float speed)
    {
        agent.speed = speed;

        if (Enemy.Arrived() && CurrentDestination == Point1)
        {
            CurrentDestination = Point2;
            agent.SetDestination(GetPointOnMesh(Point2));
        }
        else if (Enemy.Arrived() && CurrentDestination == Point2)
        {
            CurrentDestination = Point1;
            agent.SetDestination(GetPointOnMesh(Point1));
        }
        
    }

    public void ChaseMove(NavMeshAgent agent, Transform playerTransform, float speed, float range)
    {
        agent.speed = speed;
        agent.stoppingDistance = range;
        agent.SetDestination(playerTransform.position);
    }



}
