using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerController controls;
    [SerializeField] private PlayerAttack attacks;

    private bool isAttacking = false;

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


    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", controls.GetAttackSpeed());
    }

}
