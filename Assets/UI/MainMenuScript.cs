using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public void StartButtonFunc()
    {
        SceneManager.LoadScene("MainTestLevel");
    }
    public void QuitButtonFunc()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
