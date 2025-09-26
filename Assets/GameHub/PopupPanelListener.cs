using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanelListener : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.Instance.EndFound += TurnOn;
        GameManager.Instance.EndOptionIgnored += TurnOff;
    }
    private void OnDestroy()
    {
        GameManager.Instance.EndFound -= TurnOn;
        GameManager.Instance.EndOptionIgnored -= TurnOff;
    }
    private void TurnOn()
    {
        if (this != null && gameObject != null)
        {
            gameObject.SetActive(true);
        }
    }
    private void TurnOff()
    {
        if (this != null && gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}
