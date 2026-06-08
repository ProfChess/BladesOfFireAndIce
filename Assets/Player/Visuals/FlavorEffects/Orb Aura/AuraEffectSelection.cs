using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraEffectSelection : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public enum AuraEffect { 
        circle = 0, 
        pentagram = 1, 
        areaStrike = 2, 
    }
    public AuraEffect EffectChoice;

    private void Awake()
    {
        animator.SetInteger("EffectType", (int)EffectChoice);
    }

}
