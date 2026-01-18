using System;
using UnityEngine;

public abstract class EnemyDamage : MonoBehaviour
{
    protected BasePoolManager<EnemyType> PM => EnemyPoolManager.Instance;

    //Projectile Properties
    protected bool HasHit = false;
    public virtual void SetHitTrue() {  HasHit = true; }


    //Shared Functions
    protected Vector2 GetPlayerDirection(Transform playerSpot)
    {
        return (playerSpot.position - transform.position).normalized;
    }

}
