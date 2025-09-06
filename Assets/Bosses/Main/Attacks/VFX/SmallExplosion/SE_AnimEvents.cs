using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_AnimEvents : MonoBehaviour
{
    [SerializeField] private SmallExplosion MainScript;
    public void EndObject()
    {
        MainScript.FinishObject();
    }
    public void DamageStart() { MainScript.ActivateCol(); }
    public void DamageEnd() { MainScript.DeactivateCol(); }
}
