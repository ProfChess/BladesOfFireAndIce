using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite OpenedChest;
    [SerializeField] private Sprite UnopenedChest;

    public override void Interact()
    {
        if (sr.sprite == UnopenedChest) { sr.sprite = OpenedChest; }
    }
}
