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
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCooldown;
    [Header("Player Attack")]
    [SerializeField] private float FormSwitchCooldown;
    [SerializeField] private float BasicAttackDuration;
    [SerializeField] private float BasicAttackCooldown;

    //Form Enum
    [HideInInspector]
    public enum AttackForm {Fire, Ice};
    AttackForm PlayerAttackForm;

    //References
    [Header("References")]
    [SerializeField] private BoxCollider2D playerHitBox;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerAttack playerAttack;

    //Physics 
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private bool isDashing = false;
    private float DashCooldownTimer;
    private float FormSwitchCooldownTimer;
    private float BasicAttackCooldownTimer;

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
        if (DashCooldownTimer <= 0f)
        {
            isDashing = true;
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(DashDuration);
        isDashing = false;
        DashCooldownTimer = DashCooldown;
    }
    //SWITCH ATTACK FORM
    public void OnFormSwitch(InputAction.CallbackContext ctx)
    {
        if (!isDashing && FormSwitchCooldownTimer <= 0)
        {
            SwitchAttackForm();
        }
    }
    //ATTACK
    public void OnBasicAttack(InputAction.CallbackContext ctx)
    {
        if (!isDashing && BasicAttackCooldownTimer <= 0)
        {
            playerAttack.BasicAttack();
            BasicAttackCooldownTimer = BasicAttackCooldown;
        }
    }
    //Physics Movement
    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = dashDirection * DashSpeed;
        }
        else if (!isDashing)
        {
            rb.velocity = moveDirection * MoveSpeed;
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

    }
    private void Start()
    {
        PlayerAttackForm = AttackForm.Fire; //Player starts in fire form
    }

    //FORM SWITCHING/TRACKING
    //SwitchForm
    private void SwitchAttackForm() //Swtich from other form
    {
        PlayerAttackForm = PlayerAttackForm == AttackForm.Fire ? AttackForm.Ice : AttackForm.Fire;
        FormSwitchCooldownTimer = FormSwitchCooldown;
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
    //Get Attack Duration
    public float GetAttackDuration()
    {
        return BasicAttackDuration;
    }

}
