using UnityEngine;

public abstract class EnemyDamage : MonoBehaviour
{
    //Projectile Properties
    protected bool HasHit = false;
    public virtual void SetHitTrue() {  HasHit = true; }


    //Shared Functions
    protected Vector2 GetPlayerDirection(Transform playerSpot)
    {
        return (playerSpot.position - transform.position).normalized;
    }

}
