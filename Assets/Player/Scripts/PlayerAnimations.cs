using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    //References 
    [Header("References")]
    [SerializeField] private Animator PlayerAnimationController;

    //Anims
    private static readonly int RunToggle = Animator.StringToHash("Run");
    private static readonly int AttackSpeedNum = Animator.StringToHash("AttackSpeed");
    private static readonly int FireAttackTrig = Animator.StringToHash("FireAttackTrigger");
    private static readonly int IceAttackTrig = Animator.StringToHash("IceAttackTrigger");
    private static readonly int RollTrigg = Animator.StringToHash("RollTrigger");
    private static readonly int IceFormTrig = Animator.StringToHash("IceFormTrigger");
    private static readonly int FireFormTrig = Animator.StringToHash("FireFormTrigger");

    //ANIMATION PARAMETERS
    //Run Toggling
    public void TurnRunOn() { PlayerAnimationController.SetBool(RunToggle, true); }
    public void TurnRunOff() { PlayerAnimationController.SetBool(RunToggle, false); }

    //Attacks
    public void IceAttack() { PlayerAnimationController.SetTrigger(IceAttackTrig); }
    public void FireAttack() { PlayerAnimationController.SetTrigger(FireAttackTrig); }
    public void SetAttackSpeed(float Num) { PlayerAnimationController.SetFloat(AttackSpeedNum, Num); }

    //Element Forms
    public void SwitchToFireForm() { PlayerAnimationController.SetTrigger(FireFormTrig); }
    public void SwitchToIceForm() { PlayerAnimationController.SetTrigger(IceFormTrig); }

    //Rolling
    public void DodgeRoll() { PlayerAnimationController.SetTrigger(RollTrigg); }



    //ANIMATION EVENTS
    private PlayerController controls;
    private PlayerHealth health;
    private PlayerAttack attack;
    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;
    //Physics Lock
    public void StopPlayer()
    {
        controls.SetPlayerStop();
    }
    public void GoPlayer()
    {
        controls.UndoPlayerStop();
    }

    //Attacks
    public void AttackEvent()
    {
        attack.callAttack();
    }
    public void SetAttack()
    {
        isAttacking = true;
    }
    public void DesetAttack()
    {
        isAttacking = false;
    }

    public void PlayerDeath()
    {
        health.CallPlayerDeathEvent();
    }

    private void Start()
    {
        //Set Script Variables
        GameObject obj = gameObject.transform.parent.gameObject;
        controls = obj.GetComponent<PlayerController>();
        attack = obj.GetComponentInChildren<PlayerAttack>();
        health = obj.GetComponentInChildren<PlayerHealth>();

        Animator anim = GetComponent<Animator>();
    }

}
