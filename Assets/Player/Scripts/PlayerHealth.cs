using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private BoxCollider2D PlayerHitbox;
    [SerializeField] private Animator PlayerAnim;

    [SerializeField] private PlayerInput input;

    private bool isFalling = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Hazard Collision
        if (PlayerHitbox != null)
        {
            if (collision.CompareTag("Hazard-Hole") && !isFalling)
            {
                FallInHole(collision.transform.position);
            }
        }

        //Damage Collision
    }


    private void FallInHole(Vector3 HoleSpot)
    {
        isFalling = true;
        StartCoroutine(FallInHoleRoutine(HoleSpot));
    }
    //Hazard Hole Function
    private IEnumerator FallInHoleRoutine(Vector3 HoleSpot)
    {
        //Original State
        float fallTime = 0.5f;
        Vector3 OrigScale = transform.localScale;
        Vector3 StartPosition = transform.position;

        //Start Animation
        PlayerAnim.Play("PlayerFall");

        //Disable Input
        input.enabled = false;

        //Fall In Hole 
        for (float t = 0f; t < fallTime; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(StartPosition, HoleSpot, t / fallTime);   //Towards Center of Hole
            transform.localScale = Vector3.Lerp(OrigScale, Vector3.zero, t / fallTime); //Shrinks player to appear falling
            yield return null;
        }

        //Reset Player to previous state
        transform.position = new Vector3 (2, 2, 0);
        transform.localScale = OrigScale;
        isFalling = false;
        input.enabled = true;

        //WILL ADD IN FADE TO BLACK IN BETWEEN FALLING / RESPAWNING
    }
}
