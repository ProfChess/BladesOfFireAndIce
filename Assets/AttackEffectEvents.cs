using UnityEngine;

public class AttackEffectEvents : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    public void TurnOffSR()
    {
        if (sr != null)
        {
            sr.sortingOrder = -1;
        }
    }

}
