using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Player Hud
    [Header("Player Hud")]
    public Image HealthBar;
    public Image StaminaBar;

    private PlayerHealth playerHealth;
    private PlayerStaminaManager playerStamina;

    private const float UIUpdateInterval = 0.1f;
    private float UIUpdateTimer = 0f;

    [Header("Inventory")]
    public Inventory InventoryUIObject;

    [Header("Tooltip")]
    public TooltipStat Tooltip;

    [Header("Shop")]
    public ShopUI ShopUIObject;

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
    [Header("Pause")]
    [SerializeField] private GameObject PauseMenuObject;

    public void PauseGame()
    {
        PauseMenuObject.SetActive(true);
        StopGame();
    }
    public void UnPauseGame()
    {
        GameManager.Instance.CloseLatestMenu();
        ContinueGame();
    }
    public void QuitToHub()
    {
        UnPauseGame();
        SceneManager.LoadScene("MainHubScene");
    }
    private void StopGame()
    {
        Time.timeScale = 0f;
        GameTimeManager.SetPaused(true);
    }
    private void ContinueGame()
    {
        Time.timeScale = 1f;
        GameTimeManager.SetPaused(false);
    }

    //Inventory Display
    public void OpenInventory()
    {
        InventoryUIObject.gameObject.SetActive(true);
        StopGame();
    }

    //Display tooltip
    public void ShowTooltip(ItemDisplayInfo info, RectTransform transformParent, bool showTooltipOnRight)
    {
        Tooltip.DisplayInformation(info);

        RectTransform tooltipRect = Tooltip.GetComponent<RectTransform>();

        tooltipRect.pivot = showTooltipOnRight ? new Vector2(0,1) : new Vector2(1,1);
        tooltipRect.position = transformParent.position;

        Tooltip.gameObject.SetActive(true);
    }
    public void CloseTooltip()
    {
        Tooltip.gameObject.SetActive(false);
    }

    //Shop
    //Shop
    public void InputUIPopup_Shop(List<ShopOption> options) { ShopUIObject.PopulateShopOptions(options); }
    public void ActivateUIPopup_Shop() { ShopUIObject.gameObject.SetActive(true); GameManager.Instance.MenuOpened(ShopUIObject.gameObject); }
    public void MakeShopDecision(ShopOption ChosenShopItem)
    {
        GameManager.Instance.runData.AddShopCurrency(-ChosenShopItem.itemCost);
        ShopUIObject.ReEvalutateShop();
        ShopUIObject.shopItemDetails.ClearSelection();
        if (ChosenShopItem != null)
        {
            ChosenShopItem.item.BonusCollected();
        }
    }

}
