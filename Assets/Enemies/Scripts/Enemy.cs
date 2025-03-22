using UnityEngine;

public class Enemy : BaseEnemy
{
    private float checkInterval = 0.2f;
    private float nextCheck = 0f;

    private void Update()
    {
        if (Time.time >= nextCheck)
        {
            nextCheck = Time.time + checkInterval;
            if (PlayerWithinRange())
            {
                agent.SetDestination(playerLocation.position);
            }
        }
        else
        {

        }
    }
}


