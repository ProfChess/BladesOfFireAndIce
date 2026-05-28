using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopEligibleBonus : BaseAttainedBonus
{
    public abstract bool isItemAvailable();
    public abstract void BonusCollected();
}
