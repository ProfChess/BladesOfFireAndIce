using System.Collections;
using UnityEngine;

public abstract class BasePlayerAbility : BasePlayerDamage
{
    [Header("Ability Stats:")]
    [SerializeField] protected float AbilityCooldown = 0f;
    [SerializeField] protected float abilityDelay = 0.5f; //Delay for animation Startup
    protected bool AbilityOffCooldown = true;

    public override float GetAbilityDamage(BaseHealth EnemyHealth)
    {
        return AttackDamage;
    }
    public void UseAbility()
    {
        //Visual and Cooldown
        if (AbilityOffCooldown == false)
        {
            Debug.Log("Ability On Cooldown");
            return;
        }

        //Individual Ability Behaviour
        AbilityAnimation();
        InvokeAbility(abilityDelay); //Starts coroutine after given delay depending on anim
        StartCoroutine(Cooldown()); //Begins cooldown according to each abilities CD time
    }
    protected abstract void AbilityEffect(); //Will be Overwritten in Each Instance
    protected abstract void AbilityAnimation(); //Overwrite to Play Animation
    protected void InvokeAbility(float Delay)
    {
        StartCoroutine(AbilityDelayCo(Delay));
    }
    private IEnumerator AbilityDelayCo(float Delay) //Starts ability effect and damage after delay
    {
        yield return new WaitForSeconds(Delay);
        AbilityEffect();
    }
    protected virtual IEnumerator Cooldown() //Basic cooldown for each ability
    {
        AbilityOffCooldown = false;
        yield return new WaitForSeconds(AbilityCooldown);
        AbilityOffCooldown = true;
    } 

}
