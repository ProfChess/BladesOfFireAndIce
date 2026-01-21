using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BasePuzzlePart : MonoBehaviour
{
    public bool isCorrect = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<BasePlayerDamage>(out BasePlayerDamage damageInstance))
        {
            PartHit(damageInstance);
        }
    }
    protected virtual void PartHit(BasePlayerDamage damageSource)
    {

    }
}
