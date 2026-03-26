using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadStation : InteractableObject
{
    [SerializeField] private SaveUI saveUI;
    // Start is called before the first frame update
    public override void Interact()
    {
        saveUI.gameObject.SetActive(true);
        GameManager.Instance.MenuOpened(saveUI.gameObject);
    }
}
