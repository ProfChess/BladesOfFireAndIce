using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controls;
    [SerializeField] private BoxCollider2D attackBox;

    [Header("Visual")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator attackAnim;
    [SerializeField] private SpriteRenderer attackSprite;

    private float offsetDistance = 1f;
    private float attackDuration;
    PlayerController.AttackForm attackForm;
    private void Start()
    {
        attackSprite.sortingOrder = -1;                 //Hide Attack Sprite
        attackBox.enabled = false;                      //Turn Collider Off
        attackDuration = controls.GetAttackDuration();  //Assign Duration 
    }

    //Places Attack Box Collider in Direction Player is Moving
    public void BasicAttack()
    {
        //Animation
        attackForm = controls.GetAttackForm();

        if (attackForm == PlayerController.AttackForm.Fire)
        {
            playerAnim.Play("PlayerSlash");
        }
        else
        {
            playerAnim.Play("PlayerStab");
        }

    }
    public void callAttack()
    {
        //Position Calculations
        Vector2 attackDirection = controls.GetMoveVector() * offsetDistance;                    //Direction
        float rotateAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;  //Angle
        attackBox.transform.localPosition = attackDirection;
        attackBox.transform.localRotation = Quaternion.Euler(0, 0, rotateAngle);

        //Hit Effect
        AttackHitAnimation();

        //Collider on and Start Coroutine
        attackBox.enabled = true;
        StartCoroutine(BaseAttackStay());
    }
    private void AttackHitAnimation()
    {
        attackAnim.SetBool("Fire", attackForm == PlayerController.AttackForm.Fire);
        attackAnim.SetTrigger("BasicAttack");
        attackSprite.sortingOrder = 2;
    }
    private IEnumerator BaseAttackStay() //Collider stays for attack duration
    {
        yield return new WaitForSeconds(attackDuration);
        attackBox.enabled = false;
    }

}
