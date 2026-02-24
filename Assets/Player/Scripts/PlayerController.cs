using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //TESTING
    [SerializeField] private bool TESTINGMODE = false;

    //Player Stats
    [Header("Player Stats")]
    [Header("Player Move")]
    [SerializeField] private float MoveSpeed;
    [Header("Player Dash")]
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashSpeed;
    [Header("Player Attack")]
    [Header("Fire Stats")]
    [SerializeField] private float FireRollCooldown;
    [Tooltip("Distance To Knockback")]
    [SerializeField] private float FireKnockbackStrength = 2.5f;
    [Tooltip("Time it takes to Travel Knockback Distance")]
    [SerializeField] private float FireKnockbackDuration = 0.7f;
    [SerializeField] private AnimationCurve FireKnockbackCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public static float FireStanceHitStopScale { get; private set; } = 0.08f;
    public static float FireStanceHitStopDuration { get; private set; } = 0.08f;
    [Header("Ice Stats")]
    [SerializeField] private float IceRollCooldown;
    [Tooltip("Distance To Knockback")]
    [SerializeField] private float IceKnockbackStrength = 1.5f;
    [Tooltip("Time it takes to Travel Knockback Distance")]
    [SerializeField] private float IceKnockbackDuration = 0.6f;
    [SerializeField] private AnimationCurve IceKnockbackCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public static float IceStanceHitStopScale { get; private set; } = 0.18f;
    public static float IceStanceHitStopTime { get; private set; } = 0.06f;
    [Header("General")]
    [SerializeField] private float FormSwitchCooldown;
    [SerializeField] private float BasicAttackDuration;
    [SerializeField] private float BasicAttackCooldown;
    private float playerRollSpeed;

    public (float,  float) GetKnockbackStrAndDur() => 
        PlayerAttackForm == ElementType.Fire
        ? (FireKnockbackStrength, FireKnockbackDuration) 
        : (IceKnockbackStrength, IceKnockbackDuration);
    public AnimationCurve GetKnockbackCurve() => PlayerAttackForm == ElementType.Fire ? FireKnockbackCurve : IceKnockbackCurve;

    //Form Enum
    [HideInInspector]

    public static ElementType PlayerAttackForm;

    //References
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerBlock playerBlock;
    [SerializeField] private PlayerAttackCalcs playerAttackCalcs;
    [SerializeField] private PlayerAbilities playerAbilities;
    [SerializeField] private PlayerAttackSpeedManager attackSpeedManager;
    [SerializeField] private BoxCollider2D playerInteractBox;

    //Visuals and Animations
    //Visuals
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private PlayerAnimations playerAnimations;

    //Physics 
    private Vector2 moveDirection = Vector2.zero;
    public Vector2 MovingDirection => moveDirection;

    private Vector2 dashDirection = Vector2.right;
    private bool isDashing = false;
    private float FormSwitchCooldownTimer;
    private bool playerStop = false;

    //Abilities
    private PlayerAbilitySlot AbilitySlot1 = PlayerAbilitySlot.Slot1;
    private PlayerAbilitySlot AbilitySlot2 = PlayerAbilitySlot.Slot2;

    //Blocking Button
    public bool isBlockHeld { get; private set; } = false;

    //START/UPDATE/ETC
    private void Start()
    {
        PlayerAttackForm = ElementType.Fire; //Player starts in fire form
    }
    private void FixedUpdate()
    {
        //Moving or Dashing
        if (playerStop) //Player needs to stop moving (EX: Form Switch)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (isDashing) //Player moves in chosen direction without change
            {
                rb.velocity = dashDirection.normalized * DashSpeed;
            }
            else if (playerBlock.IsBlocking)
            {
                rb.velocity = moveDirection.normalized * playerBlock.BlockMoveSpeedFire;
            }
            else          //Player moves in one of eight directions
            {
                rb.velocity = moveDirection.normalized * MoveSpeed;
            }
        }
    }
    private void Update()
    {
        //COOLDOWNS
        //Form Switch Cooldown
        if (FormSwitchCooldownTimer > 0) { FormSwitchCooldownTimer -= Time.deltaTime; }

        //Player State
        if (!isDashing)
        {
            bool run = moveDirection != Vector2.zero;
            playerAnimations.SetRun(run);
        }

        //Flip Sprite
        DecideSpriteFlip();
    }
    private void DecideSpriteFlip()
    {
        //Attacking Means use attackdirection 
        if (playerAnimations.IsAttacking)
        {
            Vector2 mouseDir = playerAttack.GetMouseDirection();
            playerSprite.flipX = mouseDir.x < 0;
        }
        else
        {
            //Not Attacking
            if (moveDirection.x != 0)
            {
                //Flip Based on Horizontal Movement
                playerSprite.flipX = moveDirection.x < 0;
            }
            else
            {
                FlipSpriteByMouse();
            }
        }
    }

    //ENABLE/DISABLE/EVENTS
    private void OnEnable() 
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStart += GameBeginning;
        }
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStart -= GameBeginning;
        }
    }
    public void GameBeginning() //Handles Start of Game
    {
        if (!TESTINGMODE)
        {
            transform.position = GameManager.Instance.DungeonStartingRoomCenter;
        }
    }


    //WASD MOVEMENT CALLS
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();        //Read Input Value
    }
    //DASH
    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (isDashing || playerStop || playerAnimations.IsAttacking) { return; }

        if (ctx.started)
        {
            if (playerAttack.CheckStamina(PlayerAttack.AttackList.Roll, PlayerAttackForm))
            {
                //Stop Blocking
                if (playerBlock.IsBlocking) { playerBlock.ReleaseBlock(); }

                isDashing = true;

                //Get Direction
                Vector2 Direction = moveDirection;
                if (Direction != Vector2.zero) { dashDirection = Direction; }

                //If not moving, Sets direction based on mouse position
                else { dashDirection = SnapMousePositionTo8Directions(GetMouseDir()); }

                playerAnimations.DodgeRoll();
                playerAttack.UseSkill(PlayerAttack.AttackList.Roll);
                StartCoroutine(DashCoroutine());
            }
        }
    }
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(DashDuration);
        isDashing = false;
    }

    //SWITCH ATTACK FORM
    public void OnFormSwitch(InputAction.CallbackContext ctx)
    {
        if (playerBlock.IsBlocking || isDashing || playerAnimations.IsAttacking) { return; }

        if (ctx.started)
        {
            if (FormSwitchCooldownTimer <= 0)
            {
                SwitchAttackForm();
            }
        }

    }

    //ATTACK AND ABILITIES
    public void OnBasicAttack(InputAction.CallbackContext ctx)
    {
        //Ignore Input if 
        if (isDashing || playerStop) { return; }

        if (ctx.started)
        {
            if (playerAttack.CheckStamina(PlayerAttack.AttackList.BasicAttack, PlayerAttackForm))
            {
                if (playerBlock.IsBlocking) { playerBlock.ReleaseBlock(); }

                if (!playerAnimations.IsAttacking)
                {
                    playerAnimations.ResetCombo();
                    if (PlayerAttackForm == ElementType.Fire) { playerAnimations.FireAttack(); }
                    else { playerAnimations.IceAttack(); }
                }
                else if (playerAnimations.canReadyNextAttack)
                {
                    playerAnimations.IncreaseCombo();
                    playerAnimations.canReadyNextAttack = false;
                }
                playerAttack.UseSkill(PlayerAttack.AttackList.BasicAttack);
            }
        }

    }
    public void OnAbilityOne(InputAction.CallbackContext ctx)
    {
        if (playerBlock.IsBlocking || isDashing || playerAnimations.IsAttacking) { return; }

        if (ctx.started)
        {
            playerAbilities.ActivateAbility(AbilitySlot1, PlayerAttackForm);
            Debug.Log("First Ability Pressed");
        }

    }
    public void OnAbilityTwo(InputAction.CallbackContext ctx)
    {
        if (playerBlock.IsBlocking || isDashing || playerAnimations.IsAttacking) { return; }

        if (ctx.started)
        {
            playerAbilities.ActivateAbility(AbilitySlot2, PlayerAttackForm);
            Debug.Log("Second Ability Pressed");
        }
    }

    //BLOCKING
    public void OnBlock(InputAction.CallbackContext ctx)
    {
        /*
        Sets isBlockHeld to reflect if the button is held down, 
        allowing blocking to activate while still holding the 
        button after attacking or dashing
        */

        //Block Logic For Fire Mode
        if(PlayerAttackForm == ElementType.Fire)
        {
            if (ctx.started)
            {
                isBlockHeld = true;
                if (playerAnimations.IsAttacking) return;
                StartBlock();
            }
            else if (ctx.canceled)
            {
                isBlockHeld = false;
                StopBlock();
            }
        }
        else
        {
            //Logic for Parry
            if (ctx.started)
            {
                Debug.Log("Parry");
            }
        }

    }
    public void StartBlock()
    {
        playerBlock.Block();
    }
    public void StopBlock()
    {
        playerBlock.ReleaseBlock();
    }


    //FORM SWITCHING/TRACKING
    //SwitchForm
    private void SwitchAttackForm() //Swtich to other form
    {
        if (PlayerAttackForm == ElementType.Fire)
        {
            PlayerAttackForm = ElementType.Ice;
        }
        else if (PlayerAttackForm == ElementType.Ice)
        {
            PlayerAttackForm = ElementType.Fire;
        }
        playerAnimations.SwitchForm();
        SetFormStats();
        playerAttackCalcs.ApplyAttackAreaSize();
        FormSwitchCooldownTimer = FormSwitchCooldown;
    }
    private void SetFormStats()
    {
        if (PlayerAttackForm == ElementType.Fire)
        {
            playerRollSpeed = FireRollCooldown;
        }
        if (PlayerAttackForm == ElementType.Ice)
        {
            playerRollSpeed = IceRollCooldown;
        }
        attackSpeedManager.SetAttackSpeed();
    }
    
    public void SetPlayerStop()
    {
        playerStop = true;
    }
    public void UndoPlayerStop()
    {
        playerStop = false;
    }
    
    
    //GET ATTACK STATS
    public float GetAttackDuration()    //Get Attack Duration
    {
        return BasicAttackDuration;
    }

    //SPRITE FLIPPING FUNCTIONS
    private void FlipSpriteByMouse() //Flip Sprite in direction of mouse
    {
        Vector2 MouseDirection = GetMouseDir();
        if (MouseDirection.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (MouseDirection.x < 0)
        {
            playerSprite.flipX = true;
        }
    }
    public Vector2 GetMouseDir()    //Get Direction of mouse based on player position
    {
        Vector3 MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition); MouseLocation.z = 0;
        Vector2 MouseDirection = (MouseLocation - gameObject.transform.position).normalized;
        return MouseDirection;
    }
    private Vector2 SnapMousePositionTo8Directions(Vector2 Direction)
    {
        //Default Angle
        if (Direction == Vector2.zero) { return Vector2.right; }

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg; //Angle of direction
        angle = (angle + 360f) % 360f; //Ensures Angle is between 0-360

        int snappedAngle = Mathf.RoundToInt(angle / 45f) * 45; //Snaps angle to nearest 45 (360/8 directions)

        float AngleInRadians = snappedAngle * Mathf.Deg2Rad;
        Vector2 snappedDirection = new Vector2(Mathf.Cos(AngleInRadians), Mathf.Sin(AngleInRadians)).normalized;
        return snappedDirection;
    }


    //Interacting
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(
            playerInteractBox.bounds.center,
            playerInteractBox.bounds.size, 0f);

            foreach (Collider2D hit in hits)
            {
                InteractableObject hitObj = hit.GetComponent<InteractableObject>();
                if (hitObj != null)
                {
                    hitObj.Interact();
                    break;
                }
            }
        }

    }

    //UI 
    public void OnESC(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(GameManager.Instance != null)
            {
                GameManager.Instance.CloseMenus();
            }
        }
    }

}
