using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttack;

public class PlayerStaminaManager : MonoBehaviour
{
    //Variables
    [Header("Stamina Stats")]
    [SerializeField] private float MaxStamina;
    [SerializeField] private float CurrentStamina;      //Current Stamina Level
    [SerializeField] private float StaminaRegenRate;    //Amount Gained Per Second
    private float RegenDelay = 1f;
    public void SetMaxStamina(float num) { MaxStamina = num; CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina); }
    public float GetStamina() { return CurrentStamina; }
    public float GetMaxStamina() { return MaxStamina; }
    public void StartFillStamina() { CurrentStamina = MaxStamina; }
    public void SetCurrentStaminaFromPastScene(float num) 
    { 
        CurrentStamina = Mathf.Clamp(num, 0f, MaxStamina); 
        if(CurrentStamina < MaxStamina) { BeginStaminaRegen(); }
    }

    [Header("Stamina Costs")]
    [SerializeField] private float FireStaminaCost;
    [SerializeField] private float IceStaminaCost;
    [SerializeField] private float BaseRollStaminaCost;
    public float RollStaminaCostMultiplier { set; private get; } = 0f;
    private float TotalRollCost => BaseRollStaminaCost * RollStaminaCostMultiplier;

    public void SaveCurrentStamina() { GameManager.Instance.runData.StoreStamina(CurrentStamina); }

    //Coroutine
    private Coroutine StaminaRegenerator;

    //Coroutine For Increasing Stamina
    public IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(RegenDelay);
        while (CurrentStamina < MaxStamina)
        {
            CurrentStamina += StaminaRegenRate * GameTimeManager.GameDeltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);

            yield return null;
        }
        StaminaRegenerator = null;
    }

    //Increase and Decrease Stamina
    public void IncreaseStamina(float amountAsPercentage) 
    {
        float amount = (amountAsPercentage/100) * MaxStamina;

        CurrentStamina += amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);

        Debug.Log("Amount: " + amount);
        Debug.Log("Current: " + CurrentStamina);
        Debug.Log("Max: " + MaxStamina);
    }
    public void DecreaseStamina(float Number)
    {
        if (CurrentStamina < Number)
        {
            return;
        }
        else
        {
            CurrentStamina -= Number;
            if (CurrentStamina < 0)
            {
                CurrentStamina = 0;
            }
            BeginStaminaRegen();
        }
    }
    private void BeginStaminaRegen()
    {
        if (StaminaRegenerator != null)
        {
            StopCoroutine(StaminaRegenerator);
            StaminaRegenerator = null;
        }
        StaminaRegenerator = StartCoroutine(RegenStamina());
    }

    //Check Stamina For Actions
    public bool CheckStamina(AttackList Attacktype, ElementType StanceType)
    {
        switch (Attacktype)
        {
            case AttackList.BasicAttack:
                if (StanceType == ElementType.Fire)
                {
                    return CurrentStamina >= FireStaminaCost;
                }
                else
                {
                    return CurrentStamina >= IceStaminaCost;
                }
            case AttackList.Roll:
                return CurrentStamina >= TotalRollCost;
        }
        return false;
    }
    public void ConsumeStamina(AttackList attackType)
    {
        switch(attackType)
        {
            case AttackList.BasicAttack:
                if (PlayerSwitchElements.PlayerAttackForm == ElementType.Fire)
                {
                    DecreaseStamina(FireStaminaCost);
                }
                else
                {
                    DecreaseStamina(IceStaminaCost);
                }
                break;
            case AttackList.Roll: DecreaseStamina(TotalRollCost); break;
        }
    }
}
