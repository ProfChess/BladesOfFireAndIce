using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HitFlash : MonoBehaviour
{
    //Hitflash Material
    [SerializeField] private Material hitFlashMat;
    private Material originalMat;
    [SerializeField] private float hitFlashTime = 0.05f;
    private Coroutine hitFlashCoroutine;
    private SpriteRenderer SR;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }

    //Called for Starting or Continuing a Flash
    public void Flash()
    {
        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }
        hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
    }
    //Routine for HitFlash Duration
    private IEnumerator HitFlashRoutine()
    {
        SR.material = hitFlashMat;
        yield return new WaitForSecondsRealtime(hitFlashTime);
        SR.material = originalMat;
    }


    //Reset upon Pooling
    private void OnEnable()
    {
        originalMat = SR.material;
    }
    private void OnDisable()
    {
        if (SR != null && originalMat != null)
        {
            SR.material = originalMat;
        }
    }
}
