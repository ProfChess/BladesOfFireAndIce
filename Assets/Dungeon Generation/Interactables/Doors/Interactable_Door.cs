using System.Collections;
using UnityEngine;

public class Interactable_Door : InteractableObject
{
    [SerializeField] private Collider2D blockCollider;
    [SerializeField] private Animator animController;
    private static readonly int DeactivateAnimTrigger = Animator.StringToHash("End");
    private bool isActive = true;

    public override void Interact()
    {
        DeactivateDoor();
    }
    public void DeactivateDoor()
    {
        if (!isActive) { return; }
        isActive = false;

        blockCollider.enabled = false;
        animController.SetTrigger(DeactivateAnimTrigger);
        CheckPositionAndNeightbours();
    }
    public void CheckPositionAndNeightbours()
    {
        if (DungeonInfo.Instance == null) { return; }

        Vector2Int MyPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        foreach (var direction in DirectionAccess.CardinalDir)
        {
            Interactable_Door obj = DungeonInfo.Instance.GetDoorFromPosition(MyPosition + direction);
            if (obj != null) { obj.DeactivateDoor(); }
        }
    }
}
