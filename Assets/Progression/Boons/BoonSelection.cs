using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonSelection : InteractableObject
{
    //WILL LATER ADD UI POPUP FOR SELECTING BOON
    [SerializeField] private BoonStorage BoonCollection;
    public override void Interact()
    {
        base.Interact();
        BaseBoon ChosenBoon = GetBoon();
        ChosenBoon.BoonSelected();
    }

    //ADD RANDOM SELECTION OF 3 BOONS TO SELECT UPON INTERACTING WITH BASE OBJECT
    private BaseBoon GetBoon()
    {
        if (BoonCollection == null || BoonCollection.AllBoons.Length == 0) { return null; }
        int chosen = Random.Range(0, BoonCollection.AllBoons.Length);
        return BoonCollection.AllBoons[chosen];
    }
}
