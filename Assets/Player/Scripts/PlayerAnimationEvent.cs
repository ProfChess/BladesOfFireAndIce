using System;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerController controls;
    private PlayerAttack attacks;

    private bool isAttacking = false;

    //Event for player death
    public event Action PlayerIsDead;

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
        attacks.callAttack();
    }
    public void SetAttack()
    {
        isAttacking = true;
    }
    public void DesetAttack()
    {
        isAttacking = false;
    }
    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    public void PlayerDeath()
    {
        PlayerIsDead.Invoke();
    }

    private void Start()
    {
        //Set Script Variables
        GameObject obj = gameObject.transform.parent.gameObject;
        controls = obj.GetComponent<PlayerController>();
        attacks = obj.GetComponentInChildren<PlayerAttack>();

        Animator anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", controls.GetAttackSpeed());
    }

}
