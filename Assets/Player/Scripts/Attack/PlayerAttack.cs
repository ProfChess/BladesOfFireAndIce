using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Stamina Cost")]
    [SerializeField] private float FireStaminaCost;
    [SerializeField] private float IceStaminaCost;
    [SerializeField] private float RollStaminaCost;

    [Header("References")]
    [SerializeField] private PlayerController controls;
    [SerializeField] private BoxCollider2D attackBox;
    private PlayerStaminaManager playerStamina;

    [Header("Visual")]
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator attackAnim;
    [SerializeField] private SpriteRenderer attackSprite;
    private const int attackEffectLayer = 3;

    [Header("Damage Stats")]
    [SerializeField] private float AttackDamage;

    private Vector2 MouseDirection = Vector2.zero;

    private float offsetDistance = 1f;
    private float attackDuration;
    PlayerController.AttackForm attackForm;

    public enum AttackList { BasicAttack, Roll };
    private void Start()
    {
        attackBox.enabled = false;                      //Turn Collider Off
        attackDuration = controls.GetAttackDuration();  //Assign Duration
        playerStamina = GetComponent<PlayerStaminaManager>();                                           
    }

    //Places Attack Box Collider in Direction Player is Moving
    public void UseSkill(AttackList type)
    {
        switch (type)
        {
            case AttackList.BasicAttack:
                //Fire Stance
                if (controls.PlayerAttackForm == PlayerController.AttackForm.Fire)
                {
                    if (playerStamina.GetStamina() >= FireStaminaCost)
                    {
                        ConsumeStamina(FireStaminaCost);
                        BasicAttack();
                        playerAnim.Play("PlayerSlash");
                    }
                    else { Debug.Log("Not Enough Stamina"); }
                }

                //Ice Stance
                if (controls.PlayerAttackForm == PlayerController.AttackForm.Ice)
                {
                    if (playerStamina.GetStamina() >= IceStaminaCost)
                    {
                        ConsumeStamina(IceStaminaCost);
                        BasicAttack();
                        playerAnim.Play("PlayerStab");
                    }
                    else { Debug.Log("Not Enough Stamina"); }
                }
                break;
            case AttackList.Roll:
                ConsumeStamina(RollStaminaCost);
                break;
        }
    }
    public bool CheckStamina(AttackList Attacktype, PlayerController.AttackForm StanceType) //returns true if there is enough stamina to perform attack or skill
    {
        switch (Attacktype)
        {
            case AttackList.BasicAttack:
                if (StanceType == PlayerController.AttackForm.Fire)
                {
                    return playerStamina.GetStamina() >= FireStaminaCost;
                }
                else
                {
                    return playerStamina.GetStamina() >= IceStaminaCost;
                }
            case AttackList.Roll:
                return playerStamina.GetStamina() >= RollStaminaCost;
        }
        return false;

    }
    private void BasicAttack()
    {
        //Animation
        attackForm = controls.GetAttackForm();
        Vector3 MouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition); MouseLocation.z = 0;
        MouseDirection = (MouseLocation - controls.transform.position).normalized;
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
        attackSprite.sortingOrder = attackEffectLayer;
    }
    private IEnumerator BaseAttackStay() //Collider stays for attack duration
    {
        yield return new WaitForSeconds(attackDuration);
        attackBox.enabled = false;
    }

    public float GetDamageNumber() { return AttackDamage; }

    private void ConsumeStamina(float Amount)
    {
        playerStamina.DecreaseStamina(Amount);
    }



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
