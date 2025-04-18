using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    //Moving Var
    private float ProjectileSpeed = 0f;
    private Vector2 Direction = Vector2.zero;
    private bool isMoving = false;
    private Transform self;
    private LayerMask Stoplayers;
    [SerializeField] private PoolType poolType;
    private void Start()
    {
        self = GetComponent<Transform>();
        Stoplayers = LayerMask.GetMask("Walls");
    }

    //Begins Projectile Movement
    public void ShootProjectile(float Damage, float Speed, Vector2 Dir)
    {
        AttackDamage = Damage;
        ProjectileSpeed = Speed;
        Direction = Dir;
        isMoving = true;
    }

    //Called when projectile Hits player
    public override void SetHitTrue()
    {
        base.SetHitTrue();
        isMoving = false;
        DisableProjectile();
    }

    //Turns off projectile and returns it to the pool
    private void DisableProjectile()
    {
        isMoving = false;
        HasHit = false;
        gameObject.SetActive(false);
        PoolManager.Instance.ReturnObjectToPool(poolType, gameObject);
    }

    //Routinely moves along set direction at set speed 
    //Routinely checks infront of the arrow if it has hit a wall
    private void FixedUpdate()
    {
        if (isMoving)
        {
            self.Translate(Direction.normalized * ProjectileSpeed * Time.fixedDeltaTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * Time.fixedDeltaTime, 1, Stoplayers);
            if (hit.collider != null) { DisableProjectile(); }
        }
    }

}
