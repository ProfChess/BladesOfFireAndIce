using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDoorVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SR;
    public void TurnOff()
    {
        SR.enabled = false;
    }
}
