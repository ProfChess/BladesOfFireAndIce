using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStarterScript : MonoBehaviour
{
    [SerializeField] private BoxCollider2D EffectBox;
    [SerializeField] private GameObject Popup;

    private void ActivatePopup() { Popup.SetActive(true); }
    private void DeactivatePopup() {  Popup.SetActive(false); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { ActivatePopup(); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {  DeactivatePopup(); } 
    }
}
