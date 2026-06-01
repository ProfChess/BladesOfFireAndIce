using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonPickup : BaseBonusSelectionPickup
{
    protected override List<BaseBoon> GetRelevantBonusListInfo()
    {
        return GameManager.Instance.boonOptions.PlayerSelectBoonChoices();
    }
    private void Start()
    {
        GetComponentInChildren<FloatingObject>()?.StartObject();
    }
}
