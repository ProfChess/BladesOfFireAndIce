using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStops : MonoBehaviour
{
    [SerializeField] private ChargerEnemy EnemyScript;
    public void ChargeCooldownStop() { EnemyScript.StartChargeCooldown(); }
}
