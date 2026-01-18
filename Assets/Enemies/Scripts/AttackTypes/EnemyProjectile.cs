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
    [SerializeField] private EnemyType poolType;

    //Visuals
    [SerializeField] private GameObject visual;

    private void Start()
    {
        self = GetComponent<Transform>();
        Stoplayers = LayerMask.GetMask("Walls", "Player");
    }

    //Begins Projectile Movement
    public void ShootProjectile(float Speed, Vector2 Dir)
    {
        ProjectileSpeed = Speed;
        Direction = Dir;
        isMoving = true;
        float RotateAngle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        visual.transform.rotation = Quaternion.Euler(0, 0, RotateAngle);
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
        PM.ReturnObjectToPool(poolType, gameObject);
    }

    //Routinely moves along set direction at set speed 
    //Routinely checks infront of the arrow if it has hit a wall
    private void FixedUpdate()
    {
        if (isMoving)
        {
            self.Translate(Direction.normalized * ProjectileSpeed * Time.fixedDeltaTime);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction.normalized, 0.1f, Stoplayers);
            if (hit.collider != null) { DisableProjectile(); }
        }
    }

}
