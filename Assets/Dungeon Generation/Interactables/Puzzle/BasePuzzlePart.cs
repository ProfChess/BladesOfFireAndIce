using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BasePuzzlePart : MonoBehaviour
{
    //Marked as True if Piece is Correct
    [HideInInspector] public bool isCorrect = false;

    //Damage Detection
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
