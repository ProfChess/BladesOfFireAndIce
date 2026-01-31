using System.Collections.Generic;
using UnityEngine;

public class BoonSelection : BaseBonusSelection
{
    //Max Amount of Boon Options at Once
    private const int MaxBoonChoices = 3;
    private List<BaseBoon> SelectedBoons = new();
    private List<BaseBoon> FilteredBoons = new();

    //ADD RANDOM SELECTION OF 3 BOONS TO SELECT UPON INTERACTING WITH BASE OBJECT
    public List<BaseBoon> PlayerSelectBoonChoices()
    {
        //Update UI
        FilteredBoons.Clear();
        SelectedBoons.Clear();

        GetSelectableBoons();
        //Choose Either Max Number of Choices, Or However Many Are Left
        int count = Mathf.Min(MaxBoonChoices, FilteredBoons.Count);
        for (int i = 0; i < count; i++)
        {
            BaseBoon RandomBoon = GetBoon(FilteredBoons);
            if (RandomBoon == null) break;

            FilteredBoons.Remove(RandomBoon);
            SelectedBoons.Add(RandomBoon);
        }

        return SelectedBoons;
    }


    //Boon Filtering
    private void GetSelectableBoons()
    {
        foreach (BaseBoon Boon in Collection.AllBoons)
        {
            if (IsBoonSelectable(Boon)) { FilteredBoons.Add(Boon); }
        }
    }
    private bool IsBoonSelectable(BaseBoon Boon)
    {
        if (GM.runData.IsBoonCollected(Boon))
        {
            int BoonLevel = GM.runData.GetBoonLevel(Boon);
            if(BoonLevel == 2) { return false; }
        }
        return true;
    }

}
