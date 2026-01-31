using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSelection : BaseBonusSelection
{
    private const int MaxRelicChoices = 3;
    private List<BaseBoon> ChosenRelics = new();    //Relics Passed to UI
    private List<BaseBoon> FilteredRelics = new();  //All Possible Relics that Can Appear
    public List<BaseBoon> PlayerSelectRelicChoices()
    {
        ChosenRelics.Clear();
        FilteredRelics.Clear(); 

        foreach (BaseBoon Relic in Collection.AllBoons)
        {
            FilteredRelics.Add(Relic);
        }

        int num = Mathf.Min(Collection.AllBoons.Length, MaxRelicChoices);
        for (int i = 0; i < num; i++)
        {
            BaseBoon ChosenRelic = GetBoon(FilteredRelics);
            ChosenRelics.Add(ChosenRelic);
            FilteredRelics.Remove(ChosenRelic);
        }

        return ChosenRelics;
    }
}
