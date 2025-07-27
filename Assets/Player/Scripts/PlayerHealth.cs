using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;
public class PlayerHealth : BaseHealth
{
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



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerHitbox != null)
        {
            //Hazard Collision
            if (collision.CompareTag("Hazard-Hole") && !isFalling) { FallInHole(collision.transform.position); }
            //Damage Collision
            if (collision.CompareTag("EnemyAttack")) 
            {
                BaseAttackDamage DamageInstance = collision.GetComponent<BaseAttackDamage>();
                PlayerDamage(DamageInstance.GetDamageNumber());
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
