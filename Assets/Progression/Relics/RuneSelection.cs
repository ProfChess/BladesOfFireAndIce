using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSelection : BaseBonusSelection
{
    [SerializeField] RuneStorage EntireRelicCollection;
    [SerializeField] RuneStorage StartingRelics;

    private const int MaxRelicChoices = 3;
    private List<BaseBoon> ChosenRelics = new();    //Relics Passed to UI
    private List<Rune> FilteredRelics = new();  //All Possible Relics that Can Appear
    public List<BaseBoon> PlayerSelectRelicChoices()
    {
        ChosenRelics.Clear();
        FilteredRelics.Clear(); 

        foreach (Rune Relic in EntireRelicCollection.AllRelics)
        {
            FilteredRelics.Add(Relic);
        }

        int num = Mathf.Min(EntireRelicCollection.AllRelics.Length, MaxRelicChoices);
        for (int i = 0; i < num; i++)
        {
            Rune ChosenRelic = ChooseRelic(FilteredRelics);
            ChosenRelics.Add(ChosenRelic);
            FilteredRelics.Remove(ChosenRelic);
        }

        return ChosenRelics;
    }

    private Rune ChooseRelic(List<Rune> relicList)
    {
        if (relicList.Count <= 0) { return null; }
        Rune Choice = relicList[Random.Range(0, relicList.Count)];
        return Choice;
    }
    private void Start()
    {
        if (StartingRelics.AllRelics.Length <= 0) { return; }

        //Grant Player All Starting Relics 
        foreach(Rune relic in StartingRelics.AllRelics)
        {
            relic.BoonCollected();
        }
    }
}
