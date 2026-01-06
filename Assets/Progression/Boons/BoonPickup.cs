using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonPickup : InteractableObject
{
    private List<BaseBoon> SelectedBoons = new List<BaseBoon>();
    private bool boonListCreated = false;
    public override void Interact()
    {
        base.Interact();
        if (!boonListCreated)
        {
            if (GameManager.Instance != null)
            {
                SelectedBoons = new List<BaseBoon>(GameManager.Instance.boonOptions.PlayerSelectBoonChoices(this));
            }
            boonListCreated = true;
        }
        else
        {
            GameManager.Instance.boonOptions.DisplayUIBasedOnBoonsAvailable(SelectedBoons, this);
        }
    }


}
