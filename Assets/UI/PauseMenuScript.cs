using System.Collections;
using System.Collections.Generic;
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
}
