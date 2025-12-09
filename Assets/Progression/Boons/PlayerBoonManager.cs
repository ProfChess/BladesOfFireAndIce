using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoonManager : MonoBehaviour
{
    //Events Locations
    [SerializeField] private PlayerAttackCalcs AttackEvents;

    public void SubscribeToEvent(Action<ElementType, BaseHealth> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalEnemyHit: AttackEvents.OnEnemyHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: AttackEvents.OnEnemyDeathNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: AttackEvents.OnCriticalHitNormalAttack += BoonEffect; break;
        }
    }


    public void UnSubscribeFromEvent(Action<ElementType, BaseHealth> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalEnemyHit: AttackEvents.OnEnemyHitNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: AttackEvents.OnEnemyDeathNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: AttackEvents.OnCriticalHitNormalAttack -= BoonEffect; break;
        }
    }
}

public enum BoonEventType { OnNormalEnemyHit, OnNormalDamageEnemyDeath, OnNormalCriticalHit}
