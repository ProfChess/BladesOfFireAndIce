using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDescriptionUI : MonoBehaviour
{
    [Header("DetailsTab")]
    [SerializeField] private GameObject DetailsTab;
    [SerializeField] private ArrowTextScroll DetailsTextScroll;
    //Name
    [SerializeField] private TextMeshProUGUI NameLabel;
    [SerializeField] private TextMeshProUGUI NameText;
    //Type
    [SerializeField] private TextMeshProUGUI TypeLabel;
    [SerializeField] private TextMeshProUGUI TypeText;
    //Level
    [SerializeField] private TextMeshProUGUI LevelLabel;
    [SerializeField] private TextMeshProUGUI LevelText;
    //Description
    [SerializeField] private TextMeshProUGUI DescLabel;
    [SerializeField] private TextMeshProUGUI DescText;

    [Header("StatsTab")]
    [SerializeField] private GameObject StatsTab;
    [SerializeField] private ItemStatAccess StatEntryPrefab;
    [SerializeField] private RectTransform StatEntriesParentObject;
    [SerializeField] private List<ItemStatAccess> ReserveStatEntries;

    //Assigning Text Details
    public void AssignTextFromItem(string name, string type, string Desc, int Level = 0)
    {
        NameText.text = name; NameLabel.gameObject.SetActive(true);
        TypeText.text = type; TypeLabel.gameObject.SetActive(true);
        DescText.text = Desc; DescLabel.gameObject.SetActive(true);
        if (Level != 0) { LevelLabel.gameObject.SetActive(true); LevelText.gameObject.SetActive(true); LevelText.text = Level.ToString(); }
        else {  LevelLabel.gameObject.SetActive(false); LevelText.gameObject.SetActive(false); }

        //Update Arrows
        DetailsTextScroll.UpdateArrowScroll();
    }
    private void OnEnable()
    {
        NameLabel.gameObject.SetActive(false);
        TypeLabel.gameObject.SetActive(false);
        LevelLabel.gameObject.SetActive(false);
        DescLabel.gameObject.SetActive(false);
    }

    //Assigning Stats
    public void AssignStatsFromItem(List<StatDisplayEntry> ListOfRelevantStats)
    {
        ClearStatList();
        //Add Every Relevant Stat into the UI
        foreach (StatDisplayEntry statGroup in ListOfRelevantStats)
        {
            ItemStatAccess statUI = GetAvailableStatEntry();

            statUI.AssignStatInfo(statGroup.DisplayInfo.Name, statGroup.Value, statGroup.IsPercentage);
            statUI.gameObject.SetActive(true);
        }
    }
    //Returns First Emptry Entry if One Exists, Otherwise Creates a New One
    private ItemStatAccess GetAvailableStatEntry()
    {
        foreach (ItemStatAccess availableEntry in ReserveStatEntries)
        {
            if (availableEntry.isEmpty()) { return availableEntry; }
        }

        ItemStatAccess newEntry = Instantiate(StatEntryPrefab, StatEntriesParentObject).GetComponent<ItemStatAccess>();

        ReserveStatEntries.Add(newEntry);

        return newEntry;
    }
    private void ClearStatList()
    {
        foreach (var stat in ReserveStatEntries) { stat.ClearText(); }
    }

    //Toggles
    public void ToggleDetailTab()
    {
        DetailsTab.SetActive(true);
        StatsTab.SetActive(false);
    }
    public void ToggleStatMenu()
    {
        StatsTab.SetActive(true);
        DetailsTab.SetActive(false);
    }
}
public struct StatDisplayEntry
{
    public StatDisplayInfo DisplayInfo;
    public float Value;
    public bool IsPercentage;
}
public struct StatDisplayInfo
{
    public string Name;
    public string Description;
}
