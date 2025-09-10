using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamageCol : BaseDamageDetection
{
    public ShrinkingCircleDamage MainDamageScript;

    private void Start()
    {
        MainDamageScript = transform.parent?.parent?.GetComponent<ShrinkingCircleDamage>();
    }
    public override float GetDamageNumber()
    {
        return MainDamageScript.GetDamageNumber();
    }
}
