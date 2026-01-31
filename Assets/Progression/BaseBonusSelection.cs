using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBonusSelection : MonoBehaviour
{
    [SerializeField] protected BoonStorage Collection;

    public GameObject SelectionPopup;
    [SerializeField] protected List<BoonChoiceUI> BoonChoicesUI = new List<BoonChoiceUI>();
    protected BaseBonusSelectionPickup Pickup;

    //shortcut to game manager
    protected GameManager GM => GameManager.Instance;

    //UI Display Functions
    public void DisplayUIBasedOnBoonsAvailable(List<BaseBoon> FilteredBoons, BaseBonusSelectionPickup pickupTrigger)
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
        SelectionPopup.SetActive(true);
    }
    private void DisableAllBoonUI()
    {
        foreach (var ui in BoonChoicesUI) { ui.gameObject.SetActive(false); }
    }

    public virtual void ChoiceMade(BaseBoon Choice)
    {
        GM.ChangePlayerToPlayerActions();
        SelectionPopup.SetActive(false);
        Pickup.gameObject.SetActive(false);
        Choice.BoonCollected();
    }


    //Random Selection From Collection
    protected BaseBoon GetBoon(List<BaseBoon> Boons)
    {
        if (Collection == null || Collection.AllBoons.Length == 0 || Boons.Count == 0) { return null; }
        int chosen = Random.Range(0, Boons.Count);
        return Boons[chosen];
    }

}
