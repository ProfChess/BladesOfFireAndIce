using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
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
}
