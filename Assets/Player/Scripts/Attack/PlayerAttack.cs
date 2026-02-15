using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    [SerializeField] private PlayerStaminaManager playerStamina;

    [Header("Visual")]
    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private Animator attackAnim;
    [SerializeField] private SpriteRenderer attackSprite;
    private const int attackEffectLayer = 3;

    private Vector2 MouseDirection = Vector2.zero;

    private float offsetDistance = 1f;
    private float attackDuration;
    private ElementType attackForm;

    [HideInInspector] public HitData hitData;

    //Events for Extra Effects
    public event Action<PlayerEventContext> OnNormalAttack;
    private AttackEventContext NormalAttackContext = new();
    public enum AttackList { BasicAttack, Roll };
    private void Start()
    {
        attackBox.enabled = false;                      //Turn Collider Off
        attackDuration = controls.GetAttackDuration();  //Assign Duration
    }

    //Places Attack Box Collider in Direction Player is Moving
    public void UseSkill(AttackList type)
    {
        switch (type)
        {
            case AttackList.BasicAttack:
                //Fire Stance
                if (PlayerController.PlayerAttackForm == ElementType.Fire)
                {
                    if (playerStamina.GetStamina() >= FireStaminaCost)
                    {
                        ConsumeStamina(FireStaminaCost);
                        BasicAttack();
                    }
                    else { Debug.Log("Not Enough Stamina"); }
                }

                //Ice Stance
                if (PlayerController.PlayerAttackForm == ElementType.Ice)
                {
                    if (playerStamina.GetStamina() >= IceStaminaCost)
                    {
                        ConsumeStamina(IceStaminaCost);
                        BasicAttack();
                    }
                    else { Debug.Log("Not Enough Stamina"); }
                }
                break;
            case AttackList.Roll:
                ConsumeStamina(RollStaminaCost);
                break;
        }
    }
    public bool CheckStamina(AttackList Attacktype, ElementType StanceType) //returns true if there is enough stamina to perform attack or skill
    {
        switch (Attacktype)
        {
            case AttackList.BasicAttack:
                if (StanceType == ElementType.Fire)
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
        attackForm = PlayerController.PlayerAttackForm;
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

        //Fire Event
        NormalAttackContext.Setup(PlayerController.PlayerAttackForm, attackDirection.normalized, null, attackBox.transform.position);
        OnNormalAttack?.Invoke(NormalAttackContext);

        //Hit Effect
        AttackHitAnimation();

        //Collider on and Start Coroutine
        var KnockbackDurAndStr = controls.GetKnockbackStrAndDur();
        hitData.SetData(attackDirection, KnockbackDurAndStr.Item1, KnockbackDurAndStr.Item2, controls.GetKnockbackCurve());
        attackBox.enabled = true;


        StartCoroutine(BaseAttackStay());
    }
    public Vector2 GetMouseDirection() {return MouseDirection;}
    private void AttackHitAnimation()
    {
        attackAnim.SetBool("Fire", attackForm == ElementType.Fire);
        attackAnim.SetTrigger("BasicAttack");
        attackSprite.sortingOrder = attackEffectLayer;
    }
    private IEnumerator BaseAttackStay() //Collider stays for attack duration
    {
        yield return new WaitForSeconds(attackDuration);
        attackBox.enabled = false;
    }

    private void ConsumeStamina(float Amount)
    {
        playerStamina.DecreaseStamina(Amount);
    }

}
