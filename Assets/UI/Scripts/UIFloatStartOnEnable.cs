using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloatStartOnEnable : MonoBehaviour
{
    [SerializeField] private FloatingObject floatObject;

    private void OnEnable()
    {
        floatObject.StartObject();
    }
    private void OnDisable()
    {
        floatObject.StopObject();
    }
}
