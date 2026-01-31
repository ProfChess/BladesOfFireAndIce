using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBonusSelectionPickup : LootBase
{
    protected List<BaseBoon> SelectedBoons = new List<BaseBoon>();
    protected bool boonListCreated = false;

    public override void Interact()
    {
        base.Interact();
        ShowOptions();
        DisplayOptions();
    }

    protected void ShowOptions()
    {
        if (!boonListCreated)
        {
            if (GameManager.Instance != null)
            {
                SelectedBoons = GetRelevantBonusListInfo();
            }
            boonListCreated = true;
        }
    }
    protected abstract List<BaseBoon> GetRelevantBonusListInfo();

    protected virtual void DisplayOptions()
    {
        GameManager.Instance.boonOptions.DisplayUIBasedOnBoonsAvailable(SelectedBoons, this);
    }
}
