using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectSwitch : BaseEffectVisual
{
    public void TurnOffSelf()
    {
        gameObject.SetActive(false);
    }
}
