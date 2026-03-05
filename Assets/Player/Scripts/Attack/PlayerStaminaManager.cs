using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaminaManager : MonoBehaviour
{
    //Variables
    [SerializeField] private float MaxStamina;
    public void SetMaxStamina(float num) { MaxStamina = num; CurrentStamina = Mathf.Min(CurrentStamina, MaxStamina); }
    public void StartFillStamina() { CurrentStamina = MaxStamina; }

    [SerializeField] private float CurrentStamina;      //Current Stamina Level
    [SerializeField] private float StaminaRegenRate;    //Amount Gained Per Second
    private float RegenDelay = 1f;
    public float GetStamina () { return CurrentStamina;}
    public float GetMaxStamina() { return MaxStamina; }

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
            if (StaminaRegenerator != null)
            {
                StopCoroutine(StaminaRegenerator);
                StaminaRegenerator = null;
            }
            StaminaRegenerator = StartCoroutine(RegenStamina());
        }

    }
}
