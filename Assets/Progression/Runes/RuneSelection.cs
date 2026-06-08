using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSelection : BaseBonusSelection
{
    [SerializeField] RuneStorage EntireRuneCollection;
    [SerializeField] RuneStorage StartingRunes;

    private const int MaxRuneChoices = 3;
    private List<BaseBoon> ChosenRunes = new();    //Relics Passed to UI
    private List<Rune> FilteredRunes = new();      //All Possible Relics that Can Appear
    public List<BaseBoon> PlayerSelectRuneChoices()
    {
        ChosenRunes.Clear();
        FilteredRunes.Clear(); 

        foreach (Rune Relic in EntireRuneCollection.AllRelics)
        {
            FilteredRunes.Add(Relic);
        }

        int num = Mathf.Min(EntireRuneCollection.AllRelics.Length, MaxRuneChoices);
        for (int i = 0; i < num; i++)
        {
            Rune ChosenRelic = ChooseRune(FilteredRunes);
            ChosenRunes.Add(ChosenRelic);
            FilteredRunes.Remove(ChosenRelic);
        }

        return ChosenRunes;
    }

    private Rune ChooseRune(List<Rune> relicList)
    {
        if (relicList.Count <= 0) { return null; }
        Rune Choice = relicList[Random.Range(0, relicList.Count)];
        return Choice;
    }
    private void Start()
    {
        if (StartingRunes.AllRelics.Length <= 0) { return; }

        //Grant Player All Starting Relics 
        foreach(Rune relic in StartingRunes.AllRelics)
        {
            relic.BonusCollected();
        }
    }
}
