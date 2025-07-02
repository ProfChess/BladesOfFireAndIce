using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAttack;

public class PlayerAbilityHolder : MonoBehaviour
{
    [System.Serializable]
    private class PlayerAbilityPair
    {
        public PlayerAbilityType AbilityType;
        public BasePlayerAbility Ability;
    }
    [SerializeField] private List<PlayerAbilityPair> AbilityHolder = new List<PlayerAbilityPair>();
    private Dictionary<PlayerAbilityType, BasePlayerAbility> PlayerAbilityList =
        new Dictionary<PlayerAbilityType, BasePlayerAbility>();
    public BasePlayerAbility GetAbilityFromType(PlayerAbilityType Type)
    {
        return PlayerAbilityList[Type];
    }

}
