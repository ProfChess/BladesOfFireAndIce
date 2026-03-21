using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private void Update()
    {
        //Press N to Reload Scene
        if (Input.GetKeyDown(KeyCode.N))
        {
            GameManager.Instance.LoadScene("InteractTesting");
        }
    }

    //Saving and Loading The Player Save Data
    public void Save(PlayerSaveData data, string filePath)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }
    public PlayerSaveData Load(string filePath)
    {
        if (!File.Exists(filePath)) { return null; }
        
        string json = File.ReadAllText(filePath);
        PlayerSaveData data =  JsonUtility.FromJson<PlayerSaveData>(json);

        return data;
    }


    //Save Validation
    public void ValidateSave(PlayerSaveData data)
    {
        //Clamp XP and Stat Points
        data.xp = Mathf.Max(data.xp, 0);
        data.genericPoints = Mathf.Max(data.genericPoints, 0);
        data.currentLevel = Mathf.Max(data.currentLevel, 1);

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

}
public class PlayerSaveData
{
    //XP
    public int xp;
    public int genericPoints;
    public int currentLevel;

    //Stats
    public List<Save_StatValue> statNumbers = new List<Save_StatValue>()
    {
        new Save_StatValue{ stat = SaveStats.Vitality, value = 1 },
        new Save_StatValue{ stat = SaveStats.Endurance, value = 1 },
        new Save_StatValue{ stat = SaveStats.Strength, value = 1 },
        new Save_StatValue{ stat = SaveStats.Dexterity, value = 1 },
        new Save_StatValue{ stat = SaveStats.Luck, value = 1 },
    };

    //Blessings
    public List<Save_StatBonusStorage> statBlessings = new List<Save_StatBonusStorage>();
}
[Serializable]
public class Save_StatValue
{
    public SaveStats stat;
    public int value;
}
[Serializable]
public class Save_StatBonusStorage
{
    public SaveStats stat;
    public int chosenOption; // -1 = none, 0 or 1 = selected option
}
public enum SaveStats
{
    Vitality,
    Endurance,
    Strength,
    Dexterity,
    Luck
}
