using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //ABILITIES
    //Types

    //Current Equipped Abilities
    [SerializeField] private PlayerAbilityHolder playerAbilityHolder;
    public PlayerAbilityType Ability1 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType Ability2 = PlayerAbilityType.None; //Will be Set Private later
    public PlayerAbilityType GetFirstAbilityType() { return Ability1; }
    public PlayerAbilityType GetSecondAbilityType() { return Ability2; }

    //Assign and Call abilities by Type (Possibilities for Endless Mode)
    private void AssignAbility(PlayerAbilityType Type)
    {
        if (Ability1 == PlayerAbilityType.None)
        {
            Ability1 = Type;
        }
        else if (Ability2 == PlayerAbilityType.None)
        {
            Ability2 = Type;
        }
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