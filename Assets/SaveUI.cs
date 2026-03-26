using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    public void TestSave()
    {
        GameManager.Instance.saveManager.SaveGame(GameManager.Instance.Player);
    }
    public void TestLoad()
    {
        GameManager.Instance.saveManager.Load();
    }
}
