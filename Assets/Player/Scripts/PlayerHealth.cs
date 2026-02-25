using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
public class PlayerHealth : BaseHealth
{
    [Header("Hitbox Adjustment")]
    [SerializeField] private Vector2 hitboxFaceLeft = Vector2.zero;
    private Vector2 hitboxFaceRight = Vector2.zero;

    [Header("References")]
    [SerializeField] private BoxCollider2D PlayerHitbox;
    [SerializeField] private Animator PlayerAnim;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private HitFlash HF;
    [SerializeField] private PlayerBlock playerBlock;

    [SerializeField] private PlayerInput input;

    private bool isFalling = false;
    private float damageResist = 0;

    //Events
    //Event for player death
    public event Action PlayerIsDead;
    //Event for Damage
    public event Action<PlayerEventContext> PlayerHealthChange;
    private StatChangeEventContext healthChangeContext = new();

    //Animations
    private static readonly int FallState = Animator.StringToHash("PlayerFall");

    public void SetMaxHealth(float num) { MaxHealth = num; curHealth = MaxHealth; }
    public float GetPlayerMaxHealth => MaxHealth;
    public void AddDamageResistance(float num) { damageResist += num; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerHitbox != null)
        {
            //Hazard Interaction
            if (collision.CompareTag("Hazard-Hole") && !isFalling) { FallInHole(collision.transform.position); }

            //Attack Damage Interaction
            if (collision.TryGetComponent(out BaseDamageDetection DamageDetection)) 
            {
                //Fire Block Check
                if (PlayerController.PlayerAttackForm == ElementType.Fire)
                {
                    if (playerBlock.WasHitBlocked(DamageDetection.transform.position))
                    {
                        float reducedDamage = playerBlock.BlockedDamagePercentageFire * DamageDetection.GetAttackDamage();
                        PlayerDamage(DamageDetection.GetAttackDamage() - reducedDamage);
                        playerBlock.PlayerBlockedAHit(reducedDamage);
                    }
                    else
                    {
                        //Hit was not blocked
                        PlayerDamage(DamageDetection.GetAttackDamage());
                    }
                }
                //Ice Parry Check
                else
                {
                    if (playerBlock.WasHitParried(DamageDetection.transform.position))
                    {
                        float reducedDamage = playerBlock.IceParryDamagePencentage * DamageDetection.GetAttackDamage();
                        PlayerDamage(DamageDetection.GetAttackDamage() - reducedDamage);
                        playerBlock.PlayerParriedAHit();
                    }
                    else
                    {
                        //Hit was not blocked
                        PlayerDamage(DamageDetection.GetAttackDamage());
                    }
                }

            }
            //Collision Damage Interaction
            if (collision.TryGetComponent(out BaseHealth Health))
            {
                float CollisionDamage = Health.GetCollisionDamage();
                if (CollisionDamage > 0) { PlayerDamage(CollisionDamage); }
            }
            //Damage AOE Interaction
            if (collision.TryGetComponent(out BaseAOEHealth HealthBox))
            {
                if (HealthBox.collide) { return; }
                float CollisionDamage = HealthBox.GetCollisionDamage();
                if (CollisionDamage > 0)
                {
                    PlayerDamage(CollisionDamage);
                    HealthBox.DieOnCollision();
                }
            }
        }
    }

    //DAMAGE
    private void PlayerDamage(float damage)
    {
        if (damage > 0) 
        { 
            float totaldamage = damage - damageResist;
            
            TakeDamage(totaldamage); HF.Flash();

            //Fire Event for Health Changing
            healthChangeContext.Setup(PlayerController.PlayerAttackForm, curHealth + totaldamage, curHealth, MaxHealth);
            PlayerHealthChange?.Invoke(healthChangeContext);
        }
        //ADD CUSTOM DEATH VISUAL LOGIC, AS ASSET PACK DOES NOT CONTAIN DEATH ANIMATION
    }

    public void CallPlayerDeathEvent() { PlayerIsDead?.Invoke(); }
    //HEAL
    public void PlayerHeal(float amount)
    {
        if (curHealth + amount > MaxHealth) curHealth = MaxHealth;
        else
        {
            curHealth += amount;
        }
        healthChangeContext.Setup(PlayerController.PlayerAttackForm, curHealth - amount, curHealth, MaxHealth);
        PlayerHealthChange?.Invoke(healthChangeContext);
    }
    private void Update()
    {
        //Move Hitbox Depending on Visual of Player
        if (playerSprite.flipX)
        {
            gameObject.transform.localPosition = hitboxFaceLeft;
        }
        else if (!playerSprite.flipX)
        {
            gameObject.transform.localPosition = hitboxFaceRight;
        }
    }
    protected override void Start()
    {
        base.Start();
        hitboxFaceRight = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
    }

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
