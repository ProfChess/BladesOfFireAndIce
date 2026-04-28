using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDescriptionUI : MonoBehaviour
{
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

    //Assigning Text
    public void AssignTextFromItem(string name, string type, string Desc, int Level = 0)
    {
        NameText.text = name; 
        TypeText.text = type;
        DescText.text = Desc; 
        if (Level != 0) { LevelLabel.gameObject.SetActive(true); LevelText.gameObject.SetActive(true); LevelText.text = Level.ToString(); }
        else {  LevelLabel.gameObject.SetActive(false); LevelText.gameObject.SetActive(false); }
    }

}
