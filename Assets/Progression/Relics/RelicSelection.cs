using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSelection : BaseBonusSelection
{
    [SerializeField] RelicStorage Collection;

    private const int MaxRelicChoices = 3;
    private List<BaseBoon> ChosenRelics = new();    //Relics Passed to UI
    private List<Relic> FilteredRelics = new();  //All Possible Relics that Can Appear
    public List<BaseBoon> PlayerSelectRelicChoices()
    {
        ChosenRelics.Clear();
        FilteredRelics.Clear(); 

        foreach (Relic Relic in Collection.AllRelics)
        {
            FilteredRelics.Add(Relic);
        }

        int num = Mathf.Min(Collection.AllRelics.Length, MaxRelicChoices);
        for (int i = 0; i < num; i++)
        {
            Relic ChosenRelic = ChooseRelic(FilteredRelics);
            ChosenRelics.Add(ChosenRelic);
            FilteredRelics.Remove(ChosenRelic);
        }

        return ChosenRelics;
    }

    private Relic ChooseRelic(List<Relic> relicList)
    {
        if (relicList.Count <= 0) { return null; }
        Relic Choice = relicList[Random.Range(0, relicList.Count)];
        return Choice;
    }
}
