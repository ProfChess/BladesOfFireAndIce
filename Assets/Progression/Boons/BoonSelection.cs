using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        List<BaseBoon> AvailableBoons = BoonCollection.AllBoons.ToList();
        foreach (BoonChoiceUI boonUI in BoonChoicesUI)
        {
            BaseBoon RandomBoon = GetBoon(AvailableBoons);
            AvailableBoons.Remove(RandomBoon);
            boonUI.AssignBoonVisuals(RandomBoon);
        }

        //Display UI
        GameManager.Instance.getPlayer().GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        BoonSelectionPopup.SetActive(true);

    }
    public void BoonChoiceMade(BaseBoon ChoiceMade)
    {
        GameManager.Instance.getPlayer().GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerButtons");
        BoonSelectionPopup.SetActive(false);
        ChoiceMade.BoonSelected();
    }
    private BaseBoon GetBoon(List<BaseBoon> Boons)
    {
        if (BoonCollection == null || BoonCollection.AllBoons.Length == 0 || Boons.Count == 0) { return null; }
        int chosen = Random.Range(0, Boons.Count);
        return Boons[chosen];
    }

}
