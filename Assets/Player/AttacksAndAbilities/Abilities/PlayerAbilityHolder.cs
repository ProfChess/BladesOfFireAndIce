using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void Start()
    {
        foreach (PlayerAbilityPair pair in AbilityHolder)
        {
            PlayerAbilityList[pair.AbilityType] = pair.Ability;
        }
    }

}
