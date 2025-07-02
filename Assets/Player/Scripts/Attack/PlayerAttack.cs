using System.Collections;
using System.Collections.Generic;
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
    //Types
    public enum PlayerAbilityType
    {
        None,
        FireSmash,
    }
    //Current Equipped Abilities
    private PlayerAbilityHolder playerAbilityHolder;
    public PlayerAbilityType Ability1 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType Ability2 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType GetFirstAbilityType() { return Ability1; }
    public PlayerAbilityType GetSecondAbilityType() { return Ability2; }

    //Assign and Call abilities by Type (Possibilities for Endless Mode)
    private void AssignAbility(PlayerAbilityType Type)
    {
        if (Ability1 == PlayerAbilityType.None)
        {
            Ability1 = Type;
        }
        else if (Ability2 == PlayerAbilityType.None)
        {
            Ability2 = Type;
        }
    }
    public void CallAbility(PlayerAbilityType Ability)
    {
        //Stops if Ability is Unassigned
        if (Ability != PlayerAbilityType.None)
        {
            playerAbilityHolder.GetAbilityFromType(Ability).UseAbility();
        }
    }
}
