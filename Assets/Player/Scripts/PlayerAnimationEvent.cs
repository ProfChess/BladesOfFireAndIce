using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerController controls;
    [SerializeField] private PlayerAttack attacks;
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


    private void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("AttackSpeed", controls.GetAttackSpeed());
    }

}
