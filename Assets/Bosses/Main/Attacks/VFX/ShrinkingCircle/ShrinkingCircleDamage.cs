using System.Collections;
using UnityEngine;

public class ShrinkingCircleDamage : BaseAttackDamage
{
    private BoxCollider2D[] ColList;
    [Header("Other Stats")]
    [SerializeField] private float RotationSpeed = 3f;
    [SerializeField] private float EffectDuration = 5f;
    private bool ContinueRotating = true;

    //Coroutines
    private Coroutine ShrinkRoutine;
    private Coroutine SpinRoutine;

    private void Start()
    {
        Transform TempObj = gameObject.GetComponentInChildren<Transform>();
        ColList = TempObj.GetComponentsInChildren<BoxCollider2D>();
        if (ShrinkRoutine == null)
        {
            ShrinkRoutine = StartCoroutine(Shrink());
        }
    }
    private void TurnOffRandomPartOfCircle()
    {
        int Choice = Random.Range(0, ColList.Length);
        ColList[Choice].gameObject.SetActive(false);
    }

    private IEnumerator Shrink()
    {
        if (SpinRoutine == null) { ContinueRotating = true; SpinRoutine = StartCoroutine(Spin()); }

        TurnOffRandomPartOfCircle();

        Vector2 startScale = transform.localScale;
        Vector2 EndScale = Vector2.zero;

        float TimePassed = 0f;
        while (TimePassed < EffectDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, EndScale, TimePassed / EffectDuration);
            TimePassed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = EndScale;
        ShrinkRoutine = null;
        ContinueRotating = false;

        Debug.Log("Done");
    }
    private IEnumerator Spin()
    {
        while (ContinueRotating)
        {
            transform.Rotate(0f, 0f, 90f * Time.deltaTime);
            yield return null;
        }
        SpinRoutine = null;
    }
}
