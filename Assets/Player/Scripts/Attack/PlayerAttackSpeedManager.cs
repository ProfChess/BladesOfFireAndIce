using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpeedManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator playerAnim;

    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

    private float FireAttackSpeed = 0f;
    private float IceAttackSpeed = 0f;

    public void SetFireSpeed(float num) { FireAttackSpeed = num; }
    public void SetIceSpeed(float num) { IceAttackSpeed = num; }

    public void SetAttackSpeed()
    {
        //Changes attack speed based on Attack Form
        float AttackSpeedSet = (playerController.GetAttackForm() == ElementType.Fire)? 
            FireAttackSpeed : IceAttackSpeed;

        //Changes anim Speed 
        playerAnim.SetFloat(AttackSpeed, AttackSpeedSet);
    }

}
