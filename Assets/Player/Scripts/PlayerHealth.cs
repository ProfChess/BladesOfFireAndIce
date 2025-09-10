using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;
public class PlayerHealth : BaseHealth
{
    [Header("References")]
    [SerializeField] private BoxCollider2D PlayerHitbox;
    [SerializeField] private Animator PlayerAnim;

    [SerializeField] private PlayerInput input;

    private bool isFalling = false;

    //Events
    //Event for player death
    public event Action PlayerIsDead;


    //Animations
    private static readonly int Hurt = Animator.StringToHash("PlayerHurt");
    private static readonly int Death = Animator.StringToHash("PlayerDeath");
    private static readonly int FallState = Animator.StringToHash("PlayerFall");

    public void SetMaxHealth(float num) { MaxHealth = num; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerHitbox != null)
        {
            //Hazard Interaction
            if (collision.CompareTag("Hazard-Hole") && !isFalling) { FallInHole(collision.transform.position); }
            //Attack Damage Interaction
            if (collision.CompareTag("EnemyAttack")) 
            {
                BaseDamageDetection DamageInstance = collision.GetComponent<BaseDamageDetection>();
                PlayerDamage(DamageInstance.GetDamageNumber());
            }
            //Collision Damage Interaction
            if (collision.CompareTag("EnemyHitbox"))
            {
                float CollisionDamage = collision.GetComponent<BaseHealth>().GetCollisionDamage();
                if (CollisionDamage > 0) { PlayerDamage(CollisionDamage); }
            }
            //Damage AOE Interaction
            if (collision.CompareTag("AOEBox"))
            {
                BaseAOEHealth CollidingHealth = collision.GetComponent<BaseAOEHealth>();
                if (CollidingHealth.collide) { return; }
                float CollisionDamage = CollidingHealth.GetCollisionDamage();
                if (CollisionDamage > 0)
                {
                    PlayerDamage(CollisionDamage);
                    CollidingHealth.DieOnCollision();
                }
            }
        }
    }

    //DAMAGE
    private void PlayerDamage(float damage)
    {
        if (damage > 0) { TakeDamage(damage); }
        if (curHealth > 0) { PlayerAnim.Play(Hurt, 1); }
        else { PlayerAnim.Play(Death, 1); }
    }
    public void CallPlayerDeathEvent() { PlayerIsDead?.Invoke(); }

    //HOLE HAZARD
    private void FallInHole(Vector3 HoleSpot)
    {
        isFalling = true;
        StartCoroutine(FallInHoleRoutine(HoleSpot));
    }
    //Hazard Hole Function
    private IEnumerator FallInHoleRoutine(Vector3 HoleSpot)
    {
        //Original State
        float fallTime = 0.5f;
        Vector3 OrigScale = transform.localScale;
        Vector3 StartPosition = transform.position;

        //Start Animation
        PlayerAnim.Play(FallState);

        //Disable Input
        input.enabled = false;

        //Fall In Hole 
        for (float t = 0f; t < fallTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(StartPosition, HoleSpot, t / fallTime);   //Towards Center of Hole
            transform.localScale = Vector3.Lerp(OrigScale, Vector3.zero, t / fallTime); //Shrinks player to appear falling
            yield return null;
        }

        //Reset Player to previous state
        gameObject.GetComponent<PlayerController>().GameBeginning();
        transform.localScale = OrigScale;
        isFalling = false;
        input.enabled = true;

        //WILL ADD IN FADE TO BLACK IN BETWEEN FALLING / RESPAWNING
    }
}
