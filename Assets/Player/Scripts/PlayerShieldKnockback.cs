using UnityEngine;

public class PlayerShieldKnockback : MonoBehaviour
{
    //Fire
    [Header("Fire")]
    [SerializeField] private float FireKnockbackStrength = 3f;
    [SerializeField] private float FireKnockbackRadius = 5f;
    [SerializeField] private float FireKnockBackDuration = 0.2f;

    [Header("Ice")]
    [SerializeField] private float IceKnockbackStrength = 3f;
    [SerializeField] private float IceKnockbackRadius = 5f;
    [SerializeField] private float IceKnockbackDuration = 0.2f;

    private AnimationCurve animCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private LayerMask EnemyMask;

    private void Start()
    {
        EnemyMask = LayerMask.GetMask("Enemy");
    }

    public void IceKnockBack()
    {
        AreaKnockback(transform.position, IceKnockbackRadius, IceKnockbackStrength, IceKnockbackDuration);
    }
    public void FireKnockBack()
    {
        AreaKnockback(transform.position, FireKnockbackRadius, FireKnockbackStrength, FireKnockBackDuration);
    }


    private void AreaKnockback(Vector2 Origin, float KnockRadius, float KnockPower, float Duration)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(Origin, KnockRadius, EnemyMask);
        foreach (var obj in hits)
        {
            if(obj.TryGetComponent(out EnemyKnockback EnemyKnock))
            {
                Vector2 Direction = ((Vector2)obj.transform.position - Origin).normalized;
                EnemyKnock.KnockbackObject(Direction, KnockPower, Duration, animCurve);
            }
        }
    }
}
