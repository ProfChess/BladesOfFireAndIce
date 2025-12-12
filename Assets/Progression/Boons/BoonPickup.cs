using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoonPickup : InteractableObject
{
    public override void Interact()
    {
        base.Interact();
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.boonOptions != null)
            {
                GameManager.Instance.boonOptions.PlayerSelectBoonChoices();
            }
            else { Debug.Log("Game Manager's Boon Options is Null"); }
        }
        else { Debug.Log("Game Manager is Null"); }
    }


}
