using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoon : ScriptableObject
{
    [Header("Boon Stats")]
    public string boonName;
    [TextArea] public string boonDescription;
    //public Sprite Icon;

    //Accessing Boon Level
    protected RunDataManager runData => GameManager.Instance.runData;
    protected int Level => runData.GetBoonLevel(this);
    public abstract void BoonSelected();
    public abstract void BoonRemoved();

}

