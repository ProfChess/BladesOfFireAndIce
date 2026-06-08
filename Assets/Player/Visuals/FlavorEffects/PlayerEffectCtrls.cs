using System.Collections;
using UnityEngine;

public class PlayerEffectCtrls : MonoBehaviour
{
    [SerializeField] private Animator SwitchElementController;
    [SerializeField] private Animator KnockbackEffectController;
    [SerializeField] private Animator FireShieldHitEffectController;
    [SerializeField] private Animator IceShieldEffectController;

    //Anim Triggers
    private static readonly int SwitchToFireAnim = Animator.StringToHash("SwitchToFireEffectAnim");
    private static readonly int SwitchToIceAnim = Animator.StringToHash("SwitchToIceEffectAnim");
    private static readonly int KnockbackFireAnim = Animator.StringToHash("FireKnockbackEffectAnim");
    private static readonly int KnockbackIceAnim= Animator.StringToHash("IceKnockbackEffectAnim");
    private static readonly int FireShieldHitAnim = Animator.StringToHash("FireBlockHitAnim");
    private static readonly int IceShieldHitAnim = Animator.StringToHash("IceParryAnim");


    //Switch Element
    public void PlayEffect_SwitchElement_ToFire() 
    { 
        SwitchElementController.gameObject.SetActive(true);
        SwitchElementController.Play(SwitchToFireAnim);
    }
    public void PlayEffect_SwitchElement_ToIce() 
    {
        SwitchElementController.gameObject.SetActive(true);
        SwitchElementController.Play(SwitchToIceAnim);
    }

    //Knockback
    public void PlayEffect_Knockback_Fire() 
    {
        KnockbackEffectController.gameObject.SetActive(true);
        KnockbackEffectController.Play(KnockbackFireAnim);
    }
    public void PlayEffect_Knockback_ToIce() 
    {
        KnockbackEffectController.gameObject.SetActive(true);
        KnockbackEffectController.Play(KnockbackIceAnim); 
    }

    //Shield
    public void PlayEffect_ShieldHit_Fire() 
    {
        FireShieldHitEffectController.gameObject.SetActive(true);
        FireShieldHitEffectController.Play(FireShieldHitAnim); 
    }
    public void PlayEffect_ShieldHit_Ice() 
    {
        IceShieldEffectController.gameObject.SetActive(true);
        IceShieldEffectController.Play(IceShieldHitAnim); 
    }

}
