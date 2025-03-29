using UnityEngine;

public abstract class EnemyDamage : MonoBehaviour
{
    protected float AttackDamage {get; set;}

    public float GetDamage() {  return AttackDamage; }
}
