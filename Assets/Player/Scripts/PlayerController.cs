using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //Player Stats
    [Header("Player Stats")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashDuration;
    [SerializeField] private float DashCooldown;

    //References
    [SerializeField] private BoxCollider2D playerHitBox;
    [SerializeField] private Rigidbody2D rb;


    //Physics 
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 dashDirection = Vector2.zero;
    private bool isDashing = false;
    private float DashCooldownTimer;

    //WASD MOVEMENT CALLS
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();        //Read Input Value
        if (moveDirection != Vector2.zero && !isDashing) //Remember Last moved direction for dash
        {
            dashDirection = moveDirection;
        }
    }
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
        //Cooldowns
        if(!isDashing && DashCooldownTimer > 0)
        {
            DashCooldownTimer -= Time.deltaTime;
        }
    }
}
