using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlockHitSpriteMove : PlayerEffectSwitch
{
    [Header("Change Angle")]
    [SerializeField] private Vector3 angleFaceLeft = Vector3.zero;
    [SerializeField] private Vector3 angleFaceRight = Vector3.zero;
    public override void MoveObject(bool isleft)
    {
        if (isleft)
        {
            gameObject.transform.localPosition = VisualFacingLeft;
            gameObject.transform.localEulerAngles = angleFaceLeft;
        }
        else
        {
            gameObject.transform.localPosition = VisualFacingRight;
            gameObject.transform.localEulerAngles = angleFaceRight;
        }
    }
}
