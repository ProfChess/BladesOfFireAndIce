using System.Collections.Generic;
using UnityEngine;

public class BoonSelection : BaseBonusSelection
{
    [SerializeField] protected VirtueStorage Collection;

    //Max Amount of Boon Options at Once
    private const int MaxBoonChoices = 3;
    private List<BaseBoon> SelectedBoons = new();
    private List<Virtue> FilteredBoons = new();

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
            Virtue RandomBoon = ChooseVirtue(FilteredBoons);
            if (RandomBoon == null) break;

            FilteredBoons.Remove(RandomBoon);
            SelectedBoons.Add(RandomBoon);
        }

        return SelectedBoons;
    }


    //Boon Filtering
    private void GetSelectableBoons()
    {
        foreach (Virtue virtue in Collection.AllBoons)
        {
            if (IsBoonSelectable(virtue)) { FilteredBoons.Add(virtue); }
        }
    }
    private bool IsBoonSelectable(Virtue virtue)
    {
        if (GM.runData.IsVirtueCollected(virtue))
        {
            int BoonLevel = GM.runData.GetVirtueLevel(virtue);
            if(BoonLevel == 2) { return false; }
        }
        return true;
    }

    private Virtue ChooseVirtue(List<Virtue> virtueList)
    {
        if (virtueList.Count <= 0) { return null; }
        Virtue Choice = virtueList[Random.Range(0, virtueList.Count)];
        return Choice;
    }


    //Close Menu
    private void CloseMenu()
    {
        SelectionPopup.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.Instance.MenuClosed += CloseMenu;
    }
    private void OnDisable()
    {
        GameManager.Instance.MenuClosed -= CloseMenu;
    }
}
