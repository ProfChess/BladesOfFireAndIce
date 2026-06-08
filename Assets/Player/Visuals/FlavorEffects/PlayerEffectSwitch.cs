using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectSwitch : MonoBehaviour
{
    public void TurnOffSelf()
    {
        gameObject.SetActive(false);
    }
}
