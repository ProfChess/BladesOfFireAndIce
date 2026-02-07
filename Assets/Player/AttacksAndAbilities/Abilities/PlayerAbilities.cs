using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //ABILITIES
    //Types
    private Dictionary<(int AbilityNumber, ElementType BoundElement), Action> ActiveAbilities = new();

    //Current Equipped Abilities
    [SerializeField] private PlayerAbilityHolder playerAbilityHolder;
    public PlayerAbilityType Ability1 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType Ability2 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType GetFirstAbilityType() { return Ability1; }
    public PlayerAbilityType GetSecondAbilityType() { return Ability2; }

    //Assign and Call abilities by Type (Possibilities for Endless Mode)
    public void AssignAbility(int AbilityNumber, ElementType Element, Action Effect)
    {
        if (ActiveAbilities.ContainsKey((AbilityNumber, Element)))
        {
            //REPLACE WITH REPLACING ABILITY LATER MAYBE
            Debug.Log("Error: Ability Spot Already Taken");
            return;
        }
        ActiveAbilities.Add((AbilityNumber, Element), Effect);
    }
    public void ActivateAbility(int AbilityNumber, ElementType CurretElement)
    {
        ActiveAbilities[(AbilityNumber, CurretElement)]?.Invoke();
    }
    public void CallAbility(PlayerAbilityType Ability)
    {
        //Stops if Ability is Unassigned
        if (Ability != PlayerAbilityType.None)
        {
            playerAbilityHolder.GetAbilityFromType(Ability).UseAbility();
        }
    }
}
public enum PlayerAbilityType
{
    None,
    FireSmash,
}