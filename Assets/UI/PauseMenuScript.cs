using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject SettingsMenu;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MenuOpened(gameObject);
        }
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            Time.timeScale = 1f;
            GameTimeManager.SetPaused(false);
        }
    }

    //Buttons
    public void ResumeButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.uiManager.UnPauseGame();
        }
    }
    public void QuitButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.uiManager.QuitToHub();
        }
    }
    public void SettingsButton()
    {
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }
    public void BackButton()
    {
        SettingsMenu.SetActive(false);
        PauseMenu.SetActive(true);
    }

    
}
