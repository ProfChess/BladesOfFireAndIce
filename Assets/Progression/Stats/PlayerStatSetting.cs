using Unity.VisualScripting;
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
    private float ArmorBonus = 0f;
    private float GetBonusAsMultiplier(float num) { return 1f + (num / 100); }
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
            case StatType.Armor:          ArmorBonus += Bonus;          break;
        }
        AttributeStats();
    }

    private void AttributeStats()
    {
        playerHealth.SetMaxHealth(Stats.Health * GetBonusAsMultiplier(HealthBonus));
        playerStamina.SetMaxStamina(Stats.Stamina * GetBonusAsMultiplier(StaminaBonus));
        playerDamage.SetFireDamage(Stats.FireDamage * GetBonusAsMultiplier(FireDamageBonus));
        playerDamage.SetIceDamage(Stats.IceDamage * GetBonusAsMultiplier(IceDamageBonus));
        playerDamage.SetCritChance(Stats.CriticalChance * GetBonusAsMultiplier(CriticalChanceBonus));
        playerAttackSpeed.SetFireSpeed(Stats.AttackSpeedFire * GetBonusAsMultiplier(FireSpeedBonus));
        playerAttackSpeed.SetIceSpeed(Stats.AttackSpeedIce * GetBonusAsMultiplier(IceSpeedBonus));
        playerHealth.AddDamageResistance(ArmorBonus);
    }
    //Stat Types
}
public enum StatType { Vitality, Endurance, StrengthFire, StrengthIce, DexterityFire, DexterityIce, Luck, Armor};
