using System.Collections.Generic;
using UnityEngine;

public class BoonSelection : MonoBehaviour
{
    //WILL LATER ADD UI POPUP FOR SELECTING BOON
    [SerializeField] private BoonStorage BoonCollection;
    [SerializeField] private GameObject BoonSelectionPopup;
    [SerializeField] private List<BoonChoiceUI> BoonChoicesUI = new List<BoonChoiceUI>();

    //ADD RANDOM SELECTION OF 3 BOONS TO SELECT UPON INTERACTING WITH BASE OBJECT
    public void PlayerSelectBoonChoices()
    {
        //Update UI
        foreach (BoonChoiceUI boonUI in BoonChoicesUI)
        {
            BaseBoon RandomBoon = GetBoon();
            boonUI.AssignBoonVisuals(RandomBoon);
        }
        //Display UI
        BoonSelectionPopup.SetActive(true);

    }
    public void BoonChoiceMade(BaseBoon ChoiceMade)
    {
        BoonSelectionPopup.SetActive(false);
        ChoiceMade.BoonSelected();
    }
    private BaseBoon GetBoon()
    {
        if (BoonCollection == null || BoonCollection.AllBoons.Length == 0) { return null; }
        int chosen = Random.Range(0, BoonCollection.AllBoons.Length);
        return BoonCollection.AllBoons[chosen];
    }

}
