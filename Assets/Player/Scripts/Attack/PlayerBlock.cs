using System;
using System.Collections;
using UnityEngine;

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

    [Header("Ice")]
    [Tooltip("Stamina Consumed by Parry Trigger")]
    [SerializeField] private float StaminaConsumedParry;
    [Tooltip("Percentage of Damage Blocked in Ice")]
    [SerializeField] private float IceParryDamagePencentage;
    
    [Header("Neutral")]
    public Vector2 BlockDirection = Vector2.zero; //Set to either Vector2.Right or Vector.Left
    public float BlockArc = 180;                //Angle of Blocking Arc in either direction
    public bool IsBlocking { get; private set; } = false;
    [SerializeField] private float TimeBetweenStaminaComsume = 0.1f;
    [SerializeField] private Vector2 RightShieldPosition = new Vector2(0.25f, 0.05f);
    [SerializeField] private Vector2 LeftShieldPosition = new Vector2(-0.25f, 0.05f);

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStaminaManager staminaManager;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private BoxCollider2D blockBox;

    //Var Checks for Fire
    private bool isPlayerInFire => PlayerController.PlayerAttackForm == ElementType.Fire;

    //Events
    public event Action<PlayerEventContext> OnBlockStart;
    private PlayerEventContext BlockStartCtx = new();
    public event Action<PlayerEventContext> OnBlockEnd;
    private BlockEventContext BlockEndCtx = new();

    private void Start()
    {
        playerAnimations.SetBlockMoveSpeed(MoveBlockAnimSpeedFire);

    }

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
            BlockEndCtx.Setup(PlayerController.PlayerAttackForm, BlockDirection);

            BlockStartCtx.Setup(PlayerController.PlayerAttackForm, BlockDirection);
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
        Vector2 Dir = (hitPosition - (Vector2)transform.position).normalized;
        float dot = Vector2.Dot(BlockDirection, Dir);
        float halfBlockAngle = BlockArc * 0.5f;
        float minDot = Mathf.Cos(halfBlockAngle * Mathf.Deg2Rad);

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
                BlockDirection = GetBlockDirection();
                MoveShield(BlockDirection == Vector2.right);

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


}
