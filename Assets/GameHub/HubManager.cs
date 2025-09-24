using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
    [SerializeField] private GameObject NewRunPopup;

    public void StartRunFunc()
    {
        SceneManager.LoadScene("MainTestLevel");
    }
    public void CancelNewRun()
    {
        NewRunPopup.SetActive(false);
    }
}
