using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/AI Settings")]
public class EnemyAISettings : ScriptableObject
{
    [Tooltip("Distance From Player to Trigger Chase State")]
    public float ChaseRange = 0f;
    [Tooltip("Distance From Player to Trigger Attack State")]
    public float AttackRange = 0f;
    [Tooltip("Distance The Enemy Can go From Where They Spawned Without Returning Home")]
    public float LeashRange = 0f;
    [Tooltip("Time Enemy Will Chase Player While Still in LOS After Passing Leash Range")]
    public float ChasePastLeashTime = 0f;
    [Tooltip("Speed of Enemy When Returning to Their Spawn")]
    public float returnSpeed = 0f;
}
