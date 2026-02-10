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
        PlayerAbilities playerAbilities = GameManager.Instance.getPlayer().GetComponentInChildren<PlayerAbilities>();

        if (!playerAbilities.IsAbilitySlotTaken(PlayerAbilitySlot.Slot1))
        {
            Debug.Log("First Slot Used");
            //First Ability Slot is Free -> Place Ability
            //Place Fire Ability
            playerAbilities.AssignAbility(PlayerAbilitySlot.Slot1, ElementType.Fire, fireAbility.GetEffect(), fireAbility.BaseStats.Cooldown);
            //Place Ice Ability
            playerAbilities.AssignAbility(PlayerAbilitySlot.Slot1, ElementType.Ice, iceAbility.GetEffect(), iceAbility.BaseStats.Cooldown);
        }
        else if (!playerAbilities.IsAbilitySlotTaken(PlayerAbilitySlot.Slot2))
        {
            Debug.Log("Second Slot Used");
            //Second Ability Slot is Free -> Place Ability
            //Place Fire Ability
            playerAbilities.AssignAbility(PlayerAbilitySlot.Slot2, ElementType.Fire, fireAbility.GetEffect(), fireAbility.BaseStats.Cooldown);
            //Place Ice Ability
            playerAbilities.AssignAbility(PlayerAbilitySlot.Slot2, ElementType.Ice, iceAbility.GetEffect(), iceAbility.BaseStats.Cooldown);
        }

        if(abilityPickup != null) { abilityPickup.gameObject.SetActive(false); }
        SelectionPopup.SetActive(false);
        GM.ChangePlayerToPlayerActions();
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
        GM.ChangePlayerToUIActions();
        SelectionPopup.SetActive(true);
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
