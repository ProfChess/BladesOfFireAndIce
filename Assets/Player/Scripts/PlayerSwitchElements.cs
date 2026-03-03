using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchElements : MonoBehaviour
{
    //Form Enum
    public static ElementType PlayerAttackForm;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerAnimations playerAnim;
    [SerializeField] private PlayerMagicCircleAnim playerCircleEffect;

    //Events
    public event Action<PlayerEventContext> OnSwitchElements;
    private PlayerEventContext SwitchElementsCtx = new();

    private void Start()
    {
        PlayerAttackForm = ElementType.Fire; //Player starts in fire form
    }

    //SwitchForm
    public void SwitchAttackForm() //Swtich to other form
    {
        if (PlayerAttackForm == ElementType.Fire)
        {
            PlayerAttackForm = ElementType.Ice;
            playerCircleEffect.SwitchToIce();
        }
        else if (PlayerAttackForm == ElementType.Ice)
        {
            PlayerAttackForm = ElementType.Fire;
            playerCircleEffect.SwitchToFire();
        }

        //Fire Event
        SwitchElementsCtx.Setup(PlayerAttackForm, Vector2.right);
        OnSwitchElements?.Invoke(SwitchElementsCtx);


        playerAnim.SwitchForm();
    }
}
