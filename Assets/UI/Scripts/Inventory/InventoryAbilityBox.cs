using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAbilityBox : InventoryItemBoxUI
{
    [SerializeField] private Button ButtonObject;
    private void OnEnable()
    {
        ButtonObject.interactable = StoredObject != null;
    }
}
