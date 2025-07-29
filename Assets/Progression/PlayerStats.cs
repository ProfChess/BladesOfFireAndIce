using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Stats")]
    [Header("Health")]
    [Tooltip("Increases Health")]
    [SerializeField] private int Vitality = 1;
    [SerializeField] private float VitalityScale = 15f;
    [SerializeField] private float BaseHealth = 20f;

    [Header("Stamina")]
    [Tooltip("Increases Stamina")]
    [SerializeField] private int Endurance = 1;
    [SerializeField] private float EnduranceScale = 10f; 
    [SerializeField] private float BaseStamina = 15f;

    [Header("Damage")]
    [Tooltip("Increases Damage Before Critical Hits")]
    [SerializeField] private int Strength = 1;
    [SerializeField] private float StrengthScale = 5f;  
    [SerializeField] private float BaseDamage = 8f;

    [Header("Attack Speed")]
    [Tooltip("Increase Attack Speed")]
    [SerializeField] private int Dexterity = 1;
    [SerializeField] private float DexterityScale = 3f; //Acts as Percent (IE 5f = 5% Increase/Point)
    [SerializeField] private float BaseAttackSpeed = 1.0f;

    [Header("Critical Chance")]
    [Tooltip("Increases Critical Chance")]
    [SerializeField] private int Luck = 1;
    [SerializeField] private float LuckScale = 3f; //Acts as Percent (IE 3f = 3% Increase/Point)
    [SerializeField] private float BaseCriticalChance = 10f;


    //Getting Each Value
    public float Health => BaseHealth + (Vitality * VitalityScale); 
    public float Stamina => BaseStamina + (Endurance * EnduranceScale);
    public float Damage => BaseDamage + (Strength * StrengthScale);
    public float AttackSpeed => BaseAttackSpeed + (Dexterity * (DexterityScale/100f)); 
    public float CriticalChance => BaseCriticalChance + (Luck * (LuckScale/100f)); 

}
