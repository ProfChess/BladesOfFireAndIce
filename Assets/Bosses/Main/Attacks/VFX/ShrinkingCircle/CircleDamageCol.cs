using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamageCol : BaseDamageDetection
{
    public ShrinkingCircleDamage MainDamageScript;

    private void Awake()
    {
        MainDamageScript = transform.parent?.parent?.GetComponent<ShrinkingCircleDamage>();
    }
    public override float GetAttackDamage()
    {
        return MainDamageScript.GetAttackDamage();
    }
}
