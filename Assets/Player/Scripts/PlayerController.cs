using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public enum AttackForm { Fire, Ice };

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
    [Header("Ice Stats")]
    [SerializeField] private float IceRollCooldown;
    [Header("General")]
    [SerializeField] private float FormSwitchCooldown;
    [SerializeField] private float BasicAttackDuration;
    [SerializeField] private float BasicAttackCooldown;
    private float playerRollSpeed;

    //Form Enum
    [HideInInspector]

    public AttackForm PlayerAttackForm;

    //References
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private DungeonGenerator DunGen;
    [SerializeField] private PlayerAttackCalcs playerAttackCalcs;
    [SerializeField] private PlayerAttackSpeedManager attackSpeedManager;

    //Visuals and Animations
    //Visuals
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private PlayerAnimationEvent pae;
    //Animations
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int RollState = Animator.StringToHash("PlayerRoll");
    private static readonly int ChangeToFire = Animator.StringToHash("PlayerChangeToFire");
    private static readonly int ChangeToIce = Animator.StringToHash("PlayerChangeToIce");

    //Physics 
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection = Vector2.right;
    private bool isDashing = false;
    private float FormSwitchCooldownTimer;
    private bool playerStop = false;

    //START/UPDATE/ETC
    private void Start()
    {
        PlayerAttackForm = AttackForm.Fire; //Player starts in fire form
        SetFormStats();
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
        if (moveDirection == Vector2.zero && !isDashing)
        {
            playerAnim.SetBool(Run, false);
        }
        else if (moveDirection != Vector2.zero && !isDashing)
        {
            playerAnim.SetBool(Run, true);
        }

        //Flip Sprite
        if (moveDirection == Vector2.zero && !pae.GetIsAttacking())         //Player is not moving or attacking
        {
            FlipSpriteByMouse();
        }
        else if (moveDirection != Vector2.zero && !pae.GetIsAttacking())    //PLayer is moving but not attacking
        {
            playerSprite.flipX = moveDirection.x < 0;
        }
        else                                                                //Player is attacking
        {
            Vector2 MD = playerAttack.GetMouseDirection();
            playerSprite.flipX = MD.x < 0;
        }
    }

    //ENABLE/DISABLE/EVENTS
    private void OnEnable() 
    {
        GameManager.Instance.GameStart += GameBeginning;
    }
    private void OnDisable()
    {
        GameManager.Instance.GameStart -= GameBeginning;
    }
    public void GameBeginning() //Handles Start of Game
    {
        if (!TESTINGMODE)
        {
            transform.position = DunGen.StartingRoom.center;
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
        if (ctx.started)
        {
            if (playerAttack.CheckStamina(PlayerAttack.AttackList.Roll, PlayerAttackForm) 
                && !isDashing && !playerStop)
            {
                isDashing = true;

                //Get Direction
                Vector2 Direction = moveDirection;
                if (Direction != Vector2.zero) { dashDirection = Direction; }

                //If not moving, Sets direction based on mouse position
                else { dashDirection = SnapMousePositionTo8Directions(GetMouseDir()); }

                playerAnim.Play(RollState);
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
        if (ctx.started)
        {
            if (!isDashing && FormSwitchCooldownTimer <= 0)
            {
                SwitchAttackForm();
            }
        }

    }
    //ATTACK
    public void OnBasicAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (!isDashing && !playerStop && !pae.GetIsAttacking()
                && playerAttack.CheckStamina(PlayerAttack.AttackList.BasicAttack, PlayerAttackForm))
            {
                playerAttack.UseSkill(PlayerAttack.AttackList.BasicAttack);
            }
        }

    }
    //ABILITIES
    public void OnAbilityOne(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            playerAttack.CallAbility(playerAttack.GetFirstAbilityType());
            Debug.Log("First Ability Pressed");
        }

    }
    public void OnAbilityTwo(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            playerAttack.CallAbility(playerAttack.GetFirstAbilityType());
            Debug.Log("Second Ability Pressed");
        }
    }


    //FORM SWITCHING/TRACKING
    //SwitchForm
    private void SwitchAttackForm() //Swtich from other form
    {
        if (PlayerAttackForm == AttackForm.Fire)
        {
            playerAnim.Play(ChangeToIce);
            PlayerAttackForm = AttackForm.Ice;
        }
        else if (PlayerAttackForm == AttackForm.Ice)
        {
            playerAnim.Play(ChangeToFire);
            PlayerAttackForm = AttackForm.Fire;
        }
        SetFormStats();
        FormSwitchCooldownTimer = FormSwitchCooldown;
    }
    private void SetFormStats()
    {
        if (PlayerAttackForm == AttackForm.Fire)
        {
            playerRollSpeed = FireRollCooldown;
        }
        if (PlayerAttackForm == AttackForm.Ice)
        {
            playerRollSpeed = IceRollCooldown;
        }
        attackSpeedManager.SetAttackSpeed();
    }
    //Get Form
    public AttackForm GetAttackForm()
    {
        return PlayerAttackForm;
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
    private Vector2 GetMouseDir()    //Get Direction of mouse based on player position
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

}
