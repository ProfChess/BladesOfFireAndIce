using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSelection : BaseBonusSelection
{
    [SerializeField] RelicStorage EntireRelicCollection;
    [SerializeField] RelicStorage StartingRelics;

    private const int MaxRelicChoices = 3;
    private List<BaseBoon> ChosenRelics = new();    //Relics Passed to UI
    private List<Relic> FilteredRelics = new();  //All Possible Relics that Can Appear
    public List<BaseBoon> PlayerSelectRelicChoices()
    {
        ChosenRelics.Clear();
        FilteredRelics.Clear(); 

        foreach (Relic Relic in EntireRelicCollection.AllRelics)
        {
            FilteredRelics.Add(Relic);
        }

        int num = Mathf.Min(EntireRelicCollection.AllRelics.Length, MaxRelicChoices);
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
    private void Start()
    {
        if (StartingRelics.AllRelics.Length <= 0) { return; }

        //Grant Player All Starting Relics 
        foreach(Relic relic in StartingRelics.AllRelics)
        {
            relic.BoonCollected();
        }
    }
}
