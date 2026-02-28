using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    private Coroutine BlockingCoroutine;

    [Header("Blocking Stats")]
    [Header("Fire")]
    [Tooltip("Amount of Stamina Consumed Each Second When Holding Block in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHold;
    [Tooltip("Amount of Stamina Consumed When Hit is Blocked in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHit;
    [Tooltip("Percentage of Damage Blocked in Fire")]
    public float BlockedDamagePercentageFire;
    public float BlockMoveSpeedFire = 1f;
    [SerializeField] private float MoveBlockAnimSpeedFire = 1f;
    public float BlockArc = 180f;               
    public bool IsBlocking { get; private set; } = false;
    [SerializeField] private float TimeBetweenStaminaComsume = 0.1f;

    [Header("Ice")]
    [Tooltip("Stamina Consumed by Parry Trigger")]
    [SerializeField] private float StaminaConsumedOnParry;
    [Tooltip("Percentage of Damage Blocked in Ice")]
    public float IceParryDamagePencentage;
    [Tooltip("Duration of Immunity After Successful Parry")]
    [SerializeField] private float ImmunityDurationOnParry = 1f;
    public float ParryArc = 220f;
    public bool isInParryState { get; private set; } = false;
    private bool isParrying = false;
    public bool isImmune { get; private set; } = false;
    private Coroutine ParryImmunityCoroutine;
    public void ResetParry() { isInParryState = false; }

    [Header("Neutral")]
    public Vector2 ShieldDirection = Vector2.zero; //Set to either Vector2.Right or Vector.Left
    [SerializeField] private Vector2 RightShieldPosition = new Vector2(0.25f, 0.05f);
    [SerializeField] private Vector2 LeftShieldPosition = new Vector2(-0.25f, 0.05f);

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStaminaManager staminaManager;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private BoxCollider2D blockBox;

    //Events
    public event Action<PlayerEventContext> OnBlockStart;
    private PlayerEventContext BlockStartCtx = new();
    public event Action<PlayerEventContext> OnBlockEnd;
    private BlockEventContext BlockEndCtx = new();
    public event Action<PlayerEventContext> OnParrySuccess;
    private PlayerEventContext ParryCtx = new();

    private void Start()
    {
        playerAnimations.SetBlockMoveSpeed(MoveBlockAnimSpeedFire);
    }

    //BLOCK
    //Begin Block by Setting Stats and Starting Coroutine, Animation, and Turn on Blocking Hitbox
    public void Block()
    {
        if (staminaManager.GetStamina() < StaminaConsumedFireHold) { return; }

        IsBlocking = true;
        if (BlockingCoroutine == null)
        {
            blockBox.enabled = true;

            //Stamina Drain
            BlockingCoroutine = StartCoroutine(BlockRoutine(StaminaConsumedFireHold));

            //Adjust and Play Anim
            playerAnimations.SetBlock(true);

            //Event
            BlockEndCtx.ResetBlockedHits();
            BlockEndCtx.Setup(PlayerController.PlayerAttackForm, ShieldDirection);

            BlockStartCtx.Setup(PlayerController.PlayerAttackForm, ShieldDirection);
            OnBlockStart?.Invoke(BlockStartCtx);

        }
    }
    public void ReleaseBlock()
    {
        if (BlockingCoroutine != null)
        {
            StopCoroutine(BlockingCoroutine);
            BlockingCoroutine = null;
            IsBlocking = false;
            blockBox.enabled = false;
            playerAnimations.SetBlock(false);

            //Invoke Event Then Reset Numbers
            OnBlockEnd?.Invoke(BlockEndCtx);
        }
    }

    public void MoveShield(bool isRight)
    {
        blockBox.offset = isRight ? RightShieldPosition : LeftShieldPosition;
    }
    public bool WasHitBlocked(Vector2 hitPosition)
    {
        if (!IsBlocking) { return false; }

        //Player is Blocking
        return IsDirectionWithinArc(BlockArc, hitPosition);

    }
    private bool IsDirectionWithinArc(float arc, Vector2 hitPosition)
    {
        Vector2 Dir = (hitPosition - (Vector2)transform.position).normalized;
        float dot = Vector2.Dot(ShieldDirection, Dir);
        float halfArcAngle = arc * 0.5f;
        float minDot = Mathf.Cos(halfArcAngle * Mathf.Deg2Rad);
        return dot >= minDot;
    }
    //Lowers stamina depending on stance -> Called every time player blocks an attack
    public void PlayerBlockedAHit(float DamageBlocked)
    {
        //Consume Stamina for Hit
        float CurrentStamina = staminaManager.GetStamina();

        staminaManager.DecreaseStamina(Mathf.Min(CurrentStamina, StaminaConsumedFireHit));

        //Store Hit and Damage of Blocked Hit
        BlockEndCtx.RegisterHitAsBlocked(DamageBlocked);

        if (CurrentStamina < StaminaConsumedFireHit)
        {
            ReleaseBlock();
            //Add Stun Effect Here Later Maybe
        }
        //Play Animation
        playerAnimations.HitBlocked();
    }

    private IEnumerator BlockRoutine(float StaminaConsumedOnHold)
    {
        //Comsume Stamina For as Long as Block is Active --> Force out of Block if out of Stamina
        while (true)
        {
            float staminaCost = StaminaConsumedOnHold * TimeBetweenStaminaComsume;
            if (staminaManager.GetStamina() >= staminaCost)
            {
                staminaManager.DecreaseStamina(staminaCost);
                ShieldDirection = GetBlockDirection();
                MoveShield(ShieldDirection == Vector2.right);

                yield return new WaitForSeconds(TimeBetweenStaminaComsume);
            }
            else if (staminaManager.GetStamina() < staminaCost)
            {
                staminaManager.DecreaseStamina(staminaManager.GetStamina());
                break;
            }
        }
        ReleaseBlock();
    }
    private Vector2 GetBlockDirection()
    {
        //Player is not Moving
        if (playerController.MovingDirection == Vector2.zero)
        {
            Vector2 MouseDir = playerController.GetMouseDir();
            return MouseDir.x >= 0 ? Vector2.right : Vector2.left;
        }
        return playerController.MovingDirection.x > 0 ? Vector2.right : Vector2.left;
    }

    //PARRY
    public void Parry()
    {
        if (staminaManager.GetStamina() < StaminaConsumedOnParry) { return; }
        if (!IsBlocking && !isInParryState)
        {
            //Set State
            isInParryState = true;

            //Move Shield to Correct Side
            ShieldDirection = GetBlockDirection();
            MoveShield(ShieldDirection == Vector2.right);

            //Start the Parry Animation and Take Stamina
            playerAnimations.Parry();
            staminaManager.DecreaseStamina(StaminaConsumedOnParry);
        }
    }
    public bool WasHitParried(Vector2 hitPosition)
    {
        if (!isInParryState) { return false; }

        //Player has Triggered a Parry
        bool withinArcDirection = IsDirectionWithinArc(ParryArc, hitPosition);

        if (withinArcDirection)
        {
            return isParrying;
        }
        else { return false; }
    }
    public void OpenParryWindow() { isParrying = true; blockBox.enabled = true; }
    public void CloseParryWindow() { isParrying = false; blockBox.enabled = false; }
    public void PlayerParriedAHit()
    {
        //INSERT PARRY SUCCESS EVENT LOGIC
        Debug.Log("Player Parried");

        ParryCtx.Setup(PlayerController.PlayerAttackForm, ShieldDirection);
        OnParrySuccess?.Invoke(ParryCtx);

        //Apply Immunity
        if (ParryImmunityCoroutine == null)
        {
            ParryImmunityCoroutine = StartCoroutine(ParryImmunityRoutine());
        }
        else
        {
            StopCoroutine(ParryImmunityCoroutine);
            ParryImmunityCoroutine = null;
            ParryImmunityCoroutine = StartCoroutine(ParryImmunityRoutine());
        }
    }
    private IEnumerator ParryImmunityRoutine()
    {
        //Set Immunity (Used by Hitbox)
        isImmune = true;

        //Count to Time
        yield return GameTimeManager.WaitFor(ImmunityDurationOnParry);

        //Time Over
        isImmune = false;
        ParryImmunityCoroutine = null;
    }
}
