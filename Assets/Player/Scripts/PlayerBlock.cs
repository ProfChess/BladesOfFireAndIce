using System.Collections;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public bool IsBlocking = false;
    //public bool IsBlocking { get; private set; } = false;
    public Vector2 BlockDirection = Vector2.zero; //Set to either Vector2.Right or Vector.Left
    public float BlockArc = 180;                //Angle of Blocking Arc in either direction
    private Coroutine BlockingCoroutine;

    [Header("Blocking Stats")]
    [Header("Fire")]
    [Tooltip("Amount of Stamina Consumed Each Second When Holding Block in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHold;
    [Tooltip("Amount of Stamina Consumed When Hit is Blocked in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHit;
    [Tooltip("Percentage of Damage Blocked in Fire")]
    [SerializeField] private float BlockedDamagePercentageFire;
    [SerializeField] private float BlockMoveSpeedFire = 1f;
    [SerializeField] private float MoveBlockAnimSpeedFire = 1f;
    [Header("Ice")]
    [Tooltip("Amount of Stamina Consumed Each Second When Holding Block in Ice Stance")]
    [SerializeField] private float StaminaConsumedIceHold;
    [Tooltip("Amount of Stamina Consumed When Hit is Blocked in Ice Stance")]
    [SerializeField] private float StaminaConsumedIceHit;
    [Tooltip("Percentage of Damage Blocked in Ice")]
    [SerializeField] private float BlockedDamagePercentageIce;
    [SerializeField] private float BlockMoveSpeedIce = 1f;
    [SerializeField] private float MoveBlockAnimSpeedIce = 1f;
    [Header("Neutral")]
    [SerializeField] private float TimeBetweenStaminaComsume = 0.1f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStaminaManager staminaManager;
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private BoxCollider2D blockBox;

    public float GetBlockDamageReductionPercentage => PlayerController.PlayerAttackForm == ElementType.Fire ? BlockedDamagePercentageFire : BlockedDamagePercentageIce;
    public float GetStaminaConsumedOnHitAmount => PlayerController.PlayerAttackForm == ElementType.Fire ? StaminaConsumedFireHit : StaminaConsumedIceHit;
    private float GetStaminaConsumedHold => PlayerController.PlayerAttackForm == ElementType.Fire ? StaminaConsumedFireHold : StaminaConsumedIceHold;
    private float GetMoveBlockAnimSpeed => PlayerController.PlayerAttackForm == ElementType.Fire ? MoveBlockAnimSpeedFire : MoveBlockAnimSpeedIce;
    private void Start()
    {
        //Set Starting Settings
        if (PlayerController.PlayerAttackForm == ElementType.Fire)
        {
            playerAnimations.SetBlockMoveSpeed(MoveBlockAnimSpeedFire);
        }
        else
        {
            playerAnimations.SetBlockMoveSpeed(MoveBlockAnimSpeedIce);
        }
    }

    //Begin Block by Setting Stats and Starting Coroutine, Animation, and Turn on Blocking Hitbox
    public void Block()
    {
        if (staminaManager.GetStamina() < GetStaminaConsumedHold) { return; }

        IsBlocking = true;
        if (BlockingCoroutine == null)
        {
            BlockingCoroutine = StartCoroutine(BlockRoutine(GetStaminaConsumedHold));
            playerAnimations.SetBlockMoveSpeed(GetMoveBlockAnimSpeed);
            blockBox.enabled = true;
            playerAnimations.SetBlock(true);
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
        }
    }
    public void MoveShield(bool isRight)
    {
        if (isRight)
        {
            blockBox.offset = new Vector2(0.25f, 0.05f);
        }
        else
        {
            blockBox.offset = new Vector2(-0.25f, 0.0f);
        }
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
    public float GetBlockMoveSpeed(ElementType CurrentElement)
    {
        if (CurrentElement == ElementType.Fire)
        {
            return BlockMoveSpeedFire;
        }
        else if (CurrentElement == ElementType.Ice)
        {
            return BlockMoveSpeedIce;
        }
        return 0f;
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
