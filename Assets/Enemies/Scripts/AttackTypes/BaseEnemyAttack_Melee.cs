using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAttack_Melee : BaseEnemyAttack
{
    [SerializeField] protected Collider2D AttackCol;
}
