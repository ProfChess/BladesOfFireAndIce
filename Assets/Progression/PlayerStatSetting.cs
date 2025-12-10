using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatSetting : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private PlayerStats Stats;

    [Header("Refs")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStaminaManager playerStamina;
    [SerializeField] private PlayerAttackCalcs playerDamage;
    [SerializeField] private PlayerAttackSpeedManager playerAttackSpeed;

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStart += AttributeStats;
        }
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStart -= AttributeStats;
        }
    }


    private float HealthBonus = 0f;
    private float StaminaBonus = 0f;
    private float FireDamageBonus = 0f;
    private float IceDamageBonus = 0f;
    private float FireSpeedBonus = 0f;
    private float IceSpeedBonus = 0f;
    private float CriticalChanceBonus = 0f;
    public void ApplyBonusStat(StatType Stat, float Bonus)
    {
        switch (Stat)
        {
            case StatType.Vitality:       HealthBonus += Bonus;         break;
            case StatType.Endurance:      StaminaBonus += Bonus;        break;
            case StatType.StrengthFire:   FireDamageBonus += Bonus;     break;
            case StatType.StrengthIce:    IceDamageBonus += Bonus;      break;
            case StatType.DexterityFire:  FireSpeedBonus += Bonus;      break;
            case StatType.DexterityIce:   IceSpeedBonus += Bonus;       break;
            case StatType.Luck:           CriticalChanceBonus += Bonus; break;
        }
        AttributeStats();
    }

    private void AttributeStats()
    {
        playerHealth.SetMaxHealth(Stats.Health + HealthBonus);
        playerStamina.SetMaxStamina(Stats.Stamina + StaminaBonus);
        playerDamage.SetFireDamage(Stats.FireDamage + FireDamageBonus);
        playerDamage.SetIceDamage(Stats.IceDamage + IceDamageBonus);
        playerDamage.SetCritChance(Stats.CriticalChance + CriticalChanceBonus);
        playerAttackSpeed.SetFireSpeed(Stats.AttackSpeedFire + FireSpeedBonus);
        playerAttackSpeed.SetIceSpeed(Stats.AttackSpeedIce + IceSpeedBonus);
    }

    //Stat Types
}
public enum StatType { Vitality, Endurance, StrengthFire, StrengthIce, DexterityFire, DexterityIce, Luck };
