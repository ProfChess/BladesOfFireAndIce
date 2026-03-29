using UnityEngine;

public class StatAllocationCenter : InteractableObject
{
    [SerializeField] private GameObject StatMenu;

    public override void Interact()
    {
        base.Interact();
        StatMenu.SetActive(true);
        GameManager.Instance.ChangePlayerToUIActions();
    }

}
