using System.Collections;
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

    //Assigning Text
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
