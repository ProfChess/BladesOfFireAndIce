using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDataManager : MonoBehaviour
{
    //TEMPORARY DATA FOR EACH RUN THROUGH THE DUNGEON
    //Boons
    [SerializeField] private List<BaseBoon> BoonsCollected = new List<BaseBoon>();
    //Relics
    //Currency


    public List<BaseBoon> GetCollectedBoons() {  return BoonsCollected; }
    //Does The Player Already Have This Boon
    public bool IsBoonCollected(BaseBoon Boon) { return BoonsCollected.Contains(Boon); }
    public void AddBoon(BaseBoon Boon) { BoonsCollected.Add(Boon); }
    public void ClearRunData()
    {
        BoonsCollected.Clear(); 
    }
}
