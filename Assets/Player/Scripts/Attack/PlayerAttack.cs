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
    private int attackVisualSortingLevel = 3; //Level the attack visual goes to when on

    [Header("Damage Stats")]
    [SerializeField] private float AttackDamage;

    private Vector2 MouseDirection = Vector2.zero;

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
        Vector3 MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition); MouseLocation.z = 0;
        MouseDirection = (MouseLocation - controls.transform.position).normalized;
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
        Vector2 attackDirection = MouseDirection * offsetDistance;                                            //Direction
        float rotateAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;                //Angle
        attackBox.transform.localPosition = attackDirection;
        attackBox.transform.localRotation = Quaternion.Euler(0, 0, rotateAngle);

        //Hit Effect
        AttackHitAnimation();

        //Collider on and Start Coroutine
        attackBox.enabled = true;
        StartCoroutine(BaseAttackStay());
    }
    public Vector2 GetMouseDirection() {return MouseDirection;}
    private void AttackHitAnimation()
    {
        attackAnim.SetBool("Fire", attackForm == PlayerController.AttackForm.Fire);
        attackAnim.SetTrigger("BasicAttack");
        attackSprite.sortingOrder = attackVisualSortingLevel;
    }
    private IEnumerator BaseAttackStay() //Collider stays for attack duration
    {
        yield return new WaitForSeconds(attackDuration);
        attackBox.enabled = false;
    }

    public float GetDamageNumber() { return AttackDamage; }

    //ABILITIES
    //[SerializeField] private List<PlayerAbility> AbilityList;
    //private PlayerAbility Ability1;
    //private PlayerAbility Ability2;

    //private void AssignAbility(PlayerAbility ability)
    //{
    //    if (ability != null)
    //    {
    //        if(Ability1 == null)
    //        {
    //            Ability1 = ability;
    //        }
    //        else
    //        {
    //            Ability2 = ability;
    //        }
    //    }
    //}

    //public void CallAbility(int x)
    //{
    //    switch (x)
    //    {
    //        case 1:
    //            Ability1.UseAbility();
    //            break;
    //        case 2:
    //            Ability2.UseAbility();
    //            break;
    //        default:
    //            Debug.Log("Ability Num Incorrect");
    //            break;
    //    }
    //}


}
