using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchElements : MonoBehaviour
{
    //Form Enum
    public static ElementType PlayerAttackForm = ElementType.Fire;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimations playerAnim;
    [SerializeField] private PlayerMagicCircleAnim playerCircleEffect;
    [SerializeField] private PlayerEffectCtrls playerEffectCtrls;

    //Events
    public event Action<PlayerEventContext> OnSwitchElements;
    private PlayerEventContext SwitchElementsCtx = new();

    //SwitchForm
    public void SwitchAttackForm() //Swtich to other form
    {
        if (PlayerAttackForm == ElementType.Fire)
        {
            PlayerAttackForm = ElementType.Ice;
            playerCircleEffect.SwitchToIce();
            playerEffectCtrls.PlayEffect_SwitchElement_ToIce();
        }
        else if (PlayerAttackForm == ElementType.Ice)
        {
            PlayerAttackForm = ElementType.Fire;
            playerCircleEffect.SwitchToFire();
            playerEffectCtrls.PlayEffect_SwitchElement_ToFire();
        }

        //Fire Event
        SwitchElementsCtx.Setup(PlayerAttackForm, Vector2.right);
        OnSwitchElements?.Invoke(SwitchElementsCtx);


        playerAnim.SwitchForm();
    }
}
