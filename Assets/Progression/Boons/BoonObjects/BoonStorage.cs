using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Boons/BoonStorageObject")]
public class BoonStorage : ScriptableObject
{
    public BaseBoon[] AllBoons;
}
