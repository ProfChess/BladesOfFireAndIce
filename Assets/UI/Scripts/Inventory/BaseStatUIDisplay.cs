using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStatUIDisplay : MonoBehaviour
{
    [Header("List of Stats")]
    [SerializeField] protected float StatEntryFontSize;
    [SerializeField] protected ItemStatAccess StatEntryPrefab;
    [SerializeField] protected RectTransform StatEntriesParentObject;
    [SerializeField] protected List<ItemStatAccess> ReserveStatEntries;

    [Header("Tooltip Placement")]
    [SerializeField] private RectTransform TooltipTransformLocation;
    [SerializeField] private bool TooltipOnRight = false;
    public RectTransform GetTooltipLocation() { return TooltipTransformLocation; }
    public bool IsToolTipRight => TooltipOnRight;

    //Assigning Stats
    public void AssignStatsFromItem(List<StatDisplayEntry> ListOfRelevantStats)
    {
        ClearStatList();
        //Add Every Relevant Stat into the UI
        foreach (StatDisplayEntry statGroup in ListOfRelevantStats)
        {
            ItemStatAccess statUI = GetAvailableStatEntry();

            statUI.AssignStatInfo(statGroup.DisplayInfo, statGroup.NewValue, statGroup.IsPercentage, statGroup.OldValue);
            statUI.gameObject.SetActive(true);
            statUI.SetFontSize(StatEntryFontSize);
        }
    }
    //Returns First Emptry Entry if One Exists, Otherwise Creates a New One
    protected ItemStatAccess GetAvailableStatEntry()
    {
        foreach (ItemStatAccess availableEntry in ReserveStatEntries)
        {
            if (availableEntry.isEmpty()) { return availableEntry; }
        }

        ItemStatAccess newEntry = Instantiate(StatEntryPrefab, StatEntriesParentObject).GetComponent<ItemStatAccess>();

        ReserveStatEntries.Add(newEntry);

        return newEntry;
    }
    protected void ClearStatList()
    {
        if (ReserveStatEntries.Count <= 0) { return; }
        foreach (var stat in ReserveStatEntries) { stat.ClearText(); stat.gameObject.SetActive(false); }
    }
}
