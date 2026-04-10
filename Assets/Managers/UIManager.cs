using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Player Hud
    public Image HealthBar;
    public Image StaminaBar;

    private PlayerHealth playerHealth;
    private PlayerStaminaManager playerStamina;

    private const float UIUpdateInterval = 0.1f;
    private float UIUpdateTimer = 0f;

    private void Update()
    {
        //Check if Player is Here
        if (playerHealth == null || playerStamina == null)
        {
            var player = GameManager.Instance.Player;
            if (player != null)
            {
                playerHealth = player.GetComponentInChildren<PlayerHealth>();
                playerStamina = player.GetComponentInChildren<PlayerStaminaManager>();
            }
        }

        //Update UI if Player is Here
        if (playerHealth != null && playerStamina != null)
        {
            UIUpdateTimer -= GameTimeManager.GameDeltaTime;
            if (UIUpdateTimer <= 0f)
            {
                UIUpdateTimer = UIUpdateInterval;

                //Update UI
                UpdateUIDisplay();
            }
        }
    }

    private void UpdateUIDisplay()
    {
        //HP UI
        float HealthPercentage = playerHealth.GetHealthPercentage();
        HealthBar.fillAmount = HealthPercentage;

        //Stamina UI
        float StaminaPercentage = playerStamina.GetStaminaPercentage();
        StaminaBar.fillAmount = StaminaPercentage;
    }


    //Pause Menu
    [SerializeField] private GameObject PauseMenuObject;

    public void PauseGame()
    {
        PauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        GameTimeManager.SetPaused(true);
    }
    public void UnPauseGame()
    {
        GameManager.Instance.CloseLatestMenu();
        Time.timeScale = 1f;
        GameTimeManager.SetPaused(false);
    }

}
