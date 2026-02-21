using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Abilities/Storage"))]
public class AbilityStorage : ScriptableObject
{
    public Ability[] Collection;

    public IReadOnlyList<AbilityPair> AbilityPairs => generatedPairs;
    private List<AbilityPair> generatedPairs = new();

    public void GeneratePairs()
    {
        ClearAllLists();
        SortAbilityPairs();
    }
    private void SortAbilityPairs()
    {
        List<Ability> FireAbilities = new();
        List<Ability> IceAbilities = new();

        //Sort Abilities
        foreach (Ability ability in Collection)
        {
            if (ability.StanceForEffect == ElementType.Fire)
            {
                //Fire Ability
                FireAbilities.Add(ability);
            }
            else if (ability.StanceForEffect == ElementType.Ice)
            {
                //Ice Ability
                IceAbilities.Add(ability);
            }
        }

        int NumberOfPairs = Mathf.Min(FireAbilities.Count, IceAbilities.Count);
        List<Ability> FireCopy = new List<Ability>(FireAbilities);
        List<Ability> IceCopy = new List<Ability>(IceAbilities);
        for (int i = 0; i < NumberOfPairs; i++)
        {
            //Choose Random Abilities
            int FireIndex = Random.Range(0, FireCopy.Count);
            int IceIndex = Random.Range(0, IceCopy.Count);

            Ability FireChoice = FireCopy[FireIndex];
            Ability IceChoice = IceCopy[IceIndex];

            //Add to Pairs
            AbilityPair Pair = new AbilityPair { Fire = FireChoice, Ice = IceChoice };
            generatedPairs.Add(Pair);

            //Remove from Table
            FireCopy.RemoveAt(FireIndex);
            IceCopy.RemoveAt(IceIndex);
        }
    }
    private void ClearAllLists()
    {
        generatedPairs.Clear();
    }

}
public class AbilityPair
{
    public Ability Fire;
    public Ability Ice;
}
