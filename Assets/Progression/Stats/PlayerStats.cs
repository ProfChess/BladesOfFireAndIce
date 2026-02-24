using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Stats")]
    [Header("Health")]
    [Tooltip("Increases Health")]
    [SerializeField] private float VitalityScale = 15f;
    [SerializeField] private float BaseHealth = 20f;

    [Header("Stamina")]
    [Tooltip("Increases Stamina")]
    [SerializeField] private float EnduranceScale = 10f; 
    [SerializeField] private float BaseStamina = 15f;

    [Header("Damage")]
    [Tooltip("Increases Damage Before Critical Hits")]
    [SerializeField] private float StrengthScaleFire = 5f;  
    [SerializeField] private float BaseDamageFire = 8f;
    [SerializeField] private float StrengthScaleIce = 4f;
    [SerializeField] private float BaseDamageIce = 5f;

    [Header("Range")]
    [SerializeField] private float BaseAreaFire = 1f;
    [SerializeField] private float BaseAreaIce = 0.8f;

    [Header("Attack Speed")]
    [Tooltip("Increase Attack Speed")]
    [SerializeField] private float DexterityScaleFire = 3f; //Acts as Percent (IE 5f = 5% Increase/Point)
    [SerializeField] private float BaseAttackSpeedFire = 1.0f;
    [SerializeField] private float DexterityScaleIce = 5f;  //Acts as Percent (IE 5f = 5% Increase/Point)
    [SerializeField] private float BaseAttackSpeedIce = 1.25f;

    [Header("Critical Chance")]
    [Tooltip("Increases Critical Chance")]
    [SerializeField] private float LuckScale = 3f; //Acts as Percent (IE 3f = 3% Increase/Point)
    [SerializeField] private float BaseCriticalChance = 10f;


    //Stat Manager Access
    private StatManager SM => GameManager.Instance.statManager;


    //Getting Each Value
    public float Health => BaseHealth + (SM.VitalityPoints * VitalityScale); 
    public float Stamina => BaseStamina + (SM.EndurancePoints * EnduranceScale);
    public float FireDamage => BaseDamageFire + (SM.StrengthPoints * StrengthScaleFire);
    public float IceDamage => BaseDamageIce + (SM.StrengthPoints * StrengthScaleIce);
    public float BaseFireAOE => BaseAreaFire;
    public float BaseIceAOE => BaseAreaIce;
    public float AttackSpeedFire => BaseAttackSpeedFire + (SM.DexterityPoints * (DexterityScaleFire/100f));
    public float AttackSpeedIce => BaseAttackSpeedIce + (SM.DexterityPoints * (DexterityScaleIce / 100f));
    public float CriticalChance => (BaseCriticalChance/100f) + (SM.LuckPoints * (LuckScale/100f)); 
}
