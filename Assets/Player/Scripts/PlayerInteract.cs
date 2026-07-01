using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private BoxCollider2D playerInteractBox;
    [SerializeField] private float interactAnimWarmUpTime = 0.1f;
    private Coroutine interactRoutine;
    public void InteractWithObject()
    {
        if (interactRoutine != null) { return; }

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            playerInteractBox.bounds.center,
            playerInteractBox.bounds.size, 0f);

        InteractableObject closestObject = null;
        float closestDistance = float.MaxValue;
        foreach (Collider2D hit in hits)
        {
            InteractableObject interactable = hit.GetComponent<InteractableObject>();
            if (interactable == null) { continue; }

            float distance = (interactable.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance) { closestDistance = distance; closestObject = interactable; }
        }
        if(closestObject == null) { return; }
        interactRoutine = StartCoroutine(InteractionRoutine(closestObject));
    }
    private IEnumerator InteractionRoutine(InteractableObject interactedObject)
    {
        yield return GameTimeManager.WaitFor(interactAnimWarmUpTime);
        interactedObject.Interact();
        interactRoutine = null;
    }
}
