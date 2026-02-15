using System.Collections;
using UnityEngine;

public class PlayerAttackSpeedManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimations playerAnimations;

    private float FireAttackSpeed = 0f;
    private float IceAttackSpeed = 0f;

    public void SetFireSpeed(float num) { FireAttackSpeed = num; SetAttackSpeed(); }
    public void SetIceSpeed(float num) { IceAttackSpeed = num; SetAttackSpeed(); }

    public void SetAttackSpeed()
    {
        //Changes attack speed based on Attack Form
        float AttackSpeedSet = (PlayerController.PlayerAttackForm == ElementType.Fire)? 
            FireAttackSpeed : IceAttackSpeed;

        //Changes anim Speed 
        playerAnimations.SetAttackSpeed(AttackSpeedSet);
    }

}
