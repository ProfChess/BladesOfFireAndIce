using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelection : MonoBehaviour
{
    [SerializeField] private GameObject SelectionPopup;
    [SerializeField] AbilityStorage Storage;
    [SerializeField] private List<AbilityChoiceUI> UIOptions = new();
    private AbilityPickup abilityPickup;
    GameManager GM => GameManager.Instance;

    public void AbilitySelected(Ability fireAbility, Ability iceAbility)
    {
        GM.runData.AddAbilityPair(fireAbility, iceAbility);

        if(abilityPickup != null) { abilityPickup.gameObject.SetActive(false); }
        GM.CloseLatestMenu();
    }

    public List<AbilityPair> CreateListofPairs(AbilityPickup pickup)
    {
        abilityPickup = pickup;

        //Make List
        Storage.GeneratePairs();
        
        List<AbilityPair> Pairs = new List<AbilityPair>(Storage.AbilityPairs);
        List<AbilityPair> ChosenOptions = new();
        for (int i = 0; i < UIOptions.Count; i++)
        {
            AbilityPair Choice = Pairs[Random.Range(0, Pairs.Count)];
            ChosenOptions.Add(Choice);
            Pairs.Remove(Choice);
        }

        return ChosenOptions;
    }

    public void DisplayAbilityInformation(List<AbilityPair> abilitiesToDisplay)
    {
        if (abilitiesToDisplay.Count == 0) return;

        for (int i = 0; i < abilitiesToDisplay.Count; i++)
        {
            UIOptions[i].AssignVisuals(abilitiesToDisplay[i].Fire, abilitiesToDisplay[i].Ice);
            UIOptions[i].gameObject.SetActive(true);
        }
        SelectionPopup.SetActive(true);
        GM.MenuOpened(SelectionPopup);
    }

}
