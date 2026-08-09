using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffectVisual : MonoBehaviour
{
    [Header("Effect Transform")]
    [SerializeField] protected Vector2 VisualFacingLeft = Vector2.zero;
    [SerializeField] protected Vector2 VisualFacingRight = Vector2.zero;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected bool flipSprite = false;
    public virtual void MoveObject(bool isleft)
    {
        if (isleft)
        {
            gameObject.transform.localPosition = VisualFacingLeft;
        }
        else
        {
            gameObject.transform.localPosition = VisualFacingRight;
        }
        if (flipSprite) { sr.flipX = isleft; }
    }
    

}
