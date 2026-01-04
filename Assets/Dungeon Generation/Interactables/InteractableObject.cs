using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractableObject : MonoBehaviour
{
    public virtual void Interact() { }
}
