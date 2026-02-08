using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : LootBase
{
    private List<AbilityPair> SelectionList = new();
    private bool listMade = false;
    public override void Interact()
    {
        base.Interact();

        if (!listMade)
        {
            SelectionList = GameManager.Instance.abilityOptions.CreateListofPairs(this);
            listMade = true;
        }
        GameManager.Instance.abilityOptions.DisplayAbilityInformation(SelectionList);
    }
   

}
