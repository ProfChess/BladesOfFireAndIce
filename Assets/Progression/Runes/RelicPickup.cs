using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicPickup : BaseBonusSelectionPickup
{
    protected override List<BaseBoon> GetRelevantBonusListInfo()
    {
        return GameManager.Instance.relicOptions.PlayerSelectRelicChoices();
    }
}
