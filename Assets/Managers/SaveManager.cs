using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class SaveManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private void Update()
    {
        //Press N to Reload Scene
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.LoadScene("InteractTesting");
        }
    }
    string SaveFilePath = "Empty";
    private void Start()
    {
        SaveFilePath = Path.Combine(Application.persistentDataPath, "Saves", "playerSave.json");
        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Saves"));
        Debug.Log(SaveFilePath);
    }


    //Saving and Loading The Player Save Data
    private void Save(PlayerSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json);
    }
    public PlayerSaveData Load()
    {
        if (!File.Exists(SaveFilePath)) { return new PlayerSaveData(); }
        
        string json = File.ReadAllText(SaveFilePath);
        PlayerSaveData data =  JsonUtility.FromJson<PlayerSaveData>(json);

        ValidateSave(data);

        return data;
    }

    //Save Validation
    public void ValidateSave(PlayerSaveData data)
    {
        //Clamp XP and Stat Points
        data.xp = Mathf.Max(data.xp, 0);

        //Recalculate Points
        int totalPoints = GM.xpManager.CalculateTotalPointsFromXP(data.xp);
        int spentPoints = 0;
        foreach (var stat in data.statNumbers)
        {
            spentPoints += (stat.value - 1);
        }
        data.genericPoints = Mathf.Max(0, totalPoints - spentPoints);

        //Clamp Stats
        foreach (var stat in data.statNumbers)
        {
            stat.value = Mathf.Clamp(stat.value, 1, 10);
        }

        //Validate Blessings
        foreach (var blessing in data.statBlessings)
        {
            if (blessing.chosenOption != 0 && blessing.chosenOption != 1)
            {
                blessing.chosenOption = -1;
            }
        }
    }

    //Create Save
    public void SaveGame(PlayerController player)
    {
        PlayerSaveData saveData = new PlayerSaveData();

        //Fill in All Info
        saveData.xp = GM.xpManager.StoreSavedXP();
        saveData.genericPoints = GM.statManager.GetPointsAvailable();
        saveData.statNumbers = GM.statManager.GetStatPoints();
        saveData.statBlessings = GM.statManager.GetSelectedBlessings();

        Save(saveData);
    }
   
}
public class PlayerSaveData
{
    //XP
    public int xp;
    public int genericPoints;

    //Stats
    public List<Save_StatValue> statNumbers = new List<Save_StatValue>();

    //Blessings
    public List<Save_StatBonusStorage> statBlessings = new List<Save_StatBonusStorage>();
}
[Serializable]
public class Save_StatValue
{
    public SaveStats stat;
    public int value = 1;
}
[Serializable]
public class Save_StatBonusStorage
{
    public SaveStats stat;
    public int chosenOption = -1; // -1 = none, 0 or 1 = selected option
}
public enum SaveStats
{
    Vitality,
    Endurance,
    Strength,
    Dexterity,
    Luck
}
