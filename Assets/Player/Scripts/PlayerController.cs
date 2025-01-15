using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private float FireAttackSpeed;
    [SerializeField] private float FireAttackDamage;
    [Header("Ice Stats")]
    [SerializeField] private float IceRollCooldown;
    [SerializeField] private float IceAttackSpeed;
    [SerializeField] private float IceAttackDamage;
    [Header("General")]
    [SerializeField] private float FormSwitchCooldown;
    [SerializeField] private float BasicAttackDuration;
    [SerializeField] private float BasicAttackCooldown;
    private float playerRollSpeed;
    private float playerAttackSpeed;
    private float playerAttackDamage;

    //Form Enum
    [HideInInspector]
    public enum AttackForm {Fire, Ice};
    AttackForm PlayerAttackForm;


    //References
    [Header("References")]
    [SerializeField] private BoxCollider2D playerHitBox;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAttack playerAttack;
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator playerAnim;

    //Physics 
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private bool isDashing = false;
    private float DashCooldownTimer;
    private float FormSwitchCooldownTimer;
    private float BasicAttackCooldownTimer;
    private bool playerStop = false;

    //WASD MOVEMENT CALLS
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();        //Read Input Value
        if (moveDirection != Vector2.zero && !isDashing) //Remember Last moved direction for dash
        {
            dashDirection = moveDirection;
        }
    }
    //DASH
    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (DashCooldownTimer <= 0f && !isDashing && !playerStop)
            {
                isDashing = true;
                playerAnim.Play("PlayerRoll");
                StartCoroutine(DashCoroutine());
            }
        }

    }
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(DashDuration);
        isDashing = false;
        DashCooldownTimer = playerRollSpeed;
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
            if (!isDashing && BasicAttackCooldownTimer <= 0 && !playerStop)
            {
                playerAttack.BasicAttack();
                BasicAttackCooldownTimer = BasicAttackCooldown;
            }
        }

    }
    //Physics Movement
    private void FixedUpdate()
    {
        //Moveing or Dashing
        if (playerStop)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (isDashing)
            {
                rb.velocity = dashDirection * DashSpeed;
            }
            else
            {
                rb.velocity = moveDirection * MoveSpeed;
            }
        }
    }

    private void Update()
    {
        //COOLDOWNS
        //Dash Cooldown
        if(!isDashing && DashCooldownTimer > 0) { DashCooldownTimer -= Time.deltaTime; }
        //Form Switch Cooldown
        if (FormSwitchCooldownTimer > 0) { FormSwitchCooldownTimer -= Time.deltaTime; }
        //Attack Cooldown
        if (BasicAttackCooldownTimer > 0) { BasicAttackCooldownTimer -= Time.deltaTime; }

        //Player State
        if (moveDirection == Vector2.zero && !isDashing)
        {
            playerAnim.SetBool("Run", false);
        }
        else if (moveDirection != Vector2.zero && !isDashing)
        {
            playerAnim.SetBool("Run", true);
        }

        //Flip Sprite
        if (moveDirection.x > 0)
        {
            playerSprite.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            playerSprite.flipX = true;
        }

    }
    private void Start()
    {
        PlayerAttackForm = AttackForm.Fire; //Player starts in fire form
        SetFormStats();
    }

    //FORM SWITCHING/TRACKING
    //SwitchForm
    private void SwitchAttackForm() //Swtich from other form
    {
        if (PlayerAttackForm == AttackForm.Fire)
        {
            playerAnim.Play("PlayerChangeToIce");
            PlayerAttackForm = AttackForm.Ice;
        }
        else if (PlayerAttackForm == AttackForm.Ice)
        {
            playerAnim.Play("PlayerChangeToFire");
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
            playerAttackSpeed = FireAttackSpeed;
            playerAttackDamage = FireAttackDamage;
        }
        if (PlayerAttackForm == AttackForm.Ice)
        {
            playerRollSpeed = IceRollCooldown;
            playerAttackSpeed = IceAttackSpeed;
            playerAttackDamage = IceAttackDamage;
        }
        playerAnim.SetFloat("AttackSpeed", playerAttackSpeed);
    }
    //Get Form
    public AttackForm GetAttackForm()
    {
        return PlayerAttackForm;
    }
    //Get Movement Vector
    public Vector2 GetMoveVector()
    {
        return dashDirection;
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
    public float GetAttackSpeed()       //Get Attack Speed
    {
        return playerAttackSpeed;
    }
}
