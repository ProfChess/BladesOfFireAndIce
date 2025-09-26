using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    [SerializeField] private GameObject NewRunPopup;

    public void StartRunFunc()
    {
        if (GameManager.Instance != null) { GameManager.Instance.BeginNewLevel(); }
    }
    public void CancelNewRun()
    {
        NewRunPopup.SetActive(false);
    }
}
