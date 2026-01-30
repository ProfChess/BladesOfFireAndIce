using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoonSelection : MonoBehaviour
{
    //WILL LATER ADD UI POPUP FOR SELECTING BOON
    [SerializeField] private BoonStorage BoonCollection;
    public GameObject BoonSelectionPopup;
    [SerializeField] private List<BoonChoiceUI> BoonChoicesUI = new List<BoonChoiceUI>();
    private BoonPickup Pickup;
    //Max Amount of Boon Options at Once
    private const int MaxBoonChoices = 3;

    //shortcut to game manager
    private GameManager GM => GameManager.Instance;

    //ADD RANDOM SELECTION OF 3 BOONS TO SELECT UPON INTERACTING WITH BASE OBJECT
    public List<BaseBoon> PlayerSelectBoonChoices(BoonPickup pickupTrigger)
    {
        Pickup = pickupTrigger;
        //Update UI
        List<BaseBoon> AvailableBoons = GetSelectableBoons();
        List<BaseBoon> SelectedBoons = new List<BaseBoon>();

        //Choose Either Max Number of Choices, Or However Many Are Left
        int count = Mathf.Min(MaxBoonChoices, AvailableBoons.Count);
        for (int i = 0; i < count; i++)
        {
            BaseBoon RandomBoon = GetBoon(AvailableBoons);
            if (RandomBoon == null) break;

            AvailableBoons.Remove(RandomBoon);
            SelectedBoons.Add(RandomBoon);
        }
        //Display Each Option Depending on How Many There are
        DisplayUIBasedOnBoonsAvailable(SelectedBoons, pickupTrigger);

        return SelectedBoons;
    }

    //UI Display Functions
    public void DisplayUIBasedOnBoonsAvailable(List<BaseBoon> FilteredBoons, BoonPickup pickupTrigger)
    {
        //Assign Pickup
        Pickup = pickupTrigger;

        //Turn off All UI
        DisableAllBoonUI();

        switch (FilteredBoons.Count)
        {
            case 0: Debug.Log("No Boons Available"); return;
            case 1: //Activate Middle UI Only
                BoonChoicesUI[1].AssignBoonVisuals(FilteredBoons[0]);
                BoonChoicesUI[1].gameObject.SetActive(true);
                ActivateBoonSelectionUI();
                return;

            case 2: //Disable Middle UI Only
                BoonChoicesUI[0].AssignBoonVisuals(FilteredBoons[0]);
                BoonChoicesUI[2].AssignBoonVisuals(FilteredBoons[1]);

                BoonChoicesUI[0].gameObject.SetActive(true);
                BoonChoicesUI[2].gameObject.SetActive(true);
                ActivateBoonSelectionUI();
                return;
        }

        for (int i = 0; i < FilteredBoons.Count; i++)
        {
            BoonChoicesUI[i].AssignBoonVisuals(FilteredBoons[i]);
            BoonChoicesUI[i].gameObject.SetActive(true);
        }
        //Display UI
        if (FilteredBoons.Count > 0)
        {
            ActivateBoonSelectionUI();
        }
    }
    private void ActivateBoonSelectionUI()
    {
        GM.ChangePlayerToUIActions();
        BoonSelectionPopup.SetActive(true);
    }
    private void DisableAllBoonUI()
    {
        foreach (var ui in BoonChoicesUI) {  ui.gameObject.SetActive(false); }
    }



    //Getting And Applying Boons
    public void BoonChoiceMade(BaseBoon ChoiceMade)
    {
        GM.ChangePlayerToPlayerActions();
        BoonSelectionPopup.SetActive(false);
        Pickup.gameObject.SetActive(false);
        GM.runData.AddBoon(ChoiceMade);
    }
    private BaseBoon GetBoon(List<BaseBoon> Boons)
    {
        if (BoonCollection == null || BoonCollection.AllBoons.Length == 0 || Boons.Count == 0) { return null; }
        int chosen = Random.Range(0, Boons.Count);
        return Boons[chosen];
    }



    //Boon Filtering
    private List<BaseBoon> GetSelectableBoons()
    {
        List<BaseBoon> FilteredBoons = new List<BaseBoon>();
        foreach (BaseBoon Boon in BoonCollection.AllBoons)
        {
            if (IsBoonSelectable(Boon)) { FilteredBoons.Add(Boon); }
        }
        return FilteredBoons;
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
