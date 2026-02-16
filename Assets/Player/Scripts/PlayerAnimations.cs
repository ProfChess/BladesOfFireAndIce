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
    private static readonly int BlockToggle = Animator.StringToHash("isBlocking");
    private static readonly int AttackSpeedNum = Animator.StringToHash("AttackSpeed");
    private static readonly int FireAttackTrig = Animator.StringToHash("FireAttackTrigger");
    private static readonly int IceAttackTrig = Animator.StringToHash("IceAttackTrigger");
    private static readonly int RollTrigg = Animator.StringToHash("RollTrigger");
    private static readonly int SwitchElementTrig = Animator.StringToHash("ChangeElementTrigger");
    private static readonly int ComboIndex = Animator.StringToHash("ComboIndex");
    private static readonly int HitBlockedTrig = Animator.StringToHash("BlockedHitTrigger");
    private static readonly int BlockMoveSpeed = Animator.StringToHash("BlockMoveSpeed");

    //ANIMATION PARAMETERS
    //Run Toggling
    public void SetRun(bool cond) { PlayerAnimationController.SetBool(RunToggle, cond); }

    //Block
    public void SetBlock(bool cond) { PlayerAnimationController.SetBool(BlockToggle, cond); }
    public void HitBlocked() { PlayerAnimationController.SetTrigger(HitBlockedTrig); }
    public void SetBlockMoveSpeed(float speed) { PlayerAnimationController.SetFloat(BlockMoveSpeed, speed); }

    //Attacks
    public void IceAttack() { PlayerAnimationController.SetTrigger(IceAttackTrig); }
    public void FireAttack() { PlayerAnimationController.SetTrigger(FireAttackTrig); }
    public void SetAttackSpeed(float Num) { PlayerAnimationController.SetFloat(AttackSpeedNum, Num); }

    //Element Forms
    public void SwitchForm() { PlayerAnimationController.SetTrigger(SwitchElementTrig); }

    //Rolling
    public void DodgeRoll() { PlayerAnimationController.SetTrigger(RollTrigg); }

    //Combos
    public void IncreaseCombo() 
    { 
        int num = PlayerAnimationController.GetInteger(ComboIndex) + 1; 
        PlayerAnimationController.SetInteger(ComboIndex, num); 
    }
    public void ResetCombo() { PlayerAnimationController.SetInteger(ComboIndex, 0); }



    //ANIMATION EVENTS
    private PlayerController controls;
    private PlayerHealth health;
    private PlayerAttack attack;
    private bool isAttacking = false;
    public bool canReadyNextAttack = false;
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
        canReadyNextAttack = false;
    }
    public void BlockCheck()
    {
        if (controls.isBlockHeld)
        {
            controls.StartBlock();
        }
    }
    public void NextAttackReady() { canReadyNextAttack = true; }

    private void Start()
    {
        //Set Script Variables
        GameObject obj = gameObject.transform.parent.gameObject;
        controls = obj.GetComponent<PlayerController>();
        attack = obj.GetComponentInChildren<PlayerAttack>();
        health = obj.GetComponentInChildren<PlayerHealth>();
    }

}
