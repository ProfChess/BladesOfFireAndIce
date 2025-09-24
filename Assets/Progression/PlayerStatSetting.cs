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

    private void AttributeStats()
    {
        playerHealth.SetMaxHealth(Stats.Health);
        playerStamina.SetMaxStamina(Stats.Stamina);
        playerDamage.SetFireDamage(Stats.FireDamage);
        playerDamage.SetIceDamage(Stats.IceDamage);
        playerDamage.SetCritChance(Stats.CriticalChance);
        playerAttackSpeed.SetFireSpeed(Stats.AttackSpeedFire);
        playerAttackSpeed.SetIceSpeed(Stats.AttackSpeedIce);
    }
}
