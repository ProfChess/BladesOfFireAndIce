using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShrinkingCircleDamage : BaseAttackDamage
{
    [Header("References")]
    [SerializeField] private GameObject ColliderlistParent;

    private List<BoxCollider2D> ColList = new List<BoxCollider2D>();
    private float RotationSpeed = 90f;
    private float EffectDuration = 5f;
    private bool ContinueRotating = false;
    private Vector2 StartingScale = Vector2.one;

    //Coroutines
    private Coroutine ShrinkRoutine;
    private Coroutine SpinRoutine;

    //Anim Curve
    public AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    //Start and Set Functions
    private void Awake()
    {
        ColList = ColliderlistParent.GetComponentsInChildren<BoxCollider2D>().ToList();
        StartingScale = gameObject.transform.localScale;
    }
    public void SetStats(float Speed, float Duration)
    {
        RotationSpeed = Speed;
        EffectDuration = Duration;
    }
    private void ResetStats() //Set everything back to base form
    {
        foreach (BoxCollider2D box in ColList)
        {
            box.gameObject.SetActive(true);
        }
        ContinueRotating = false;
        PoolManager.Instance.ReturnObjectToPool(EnemyType.CircleFlames, gameObject);
        transform.localScale = StartingScale;
        transform.rotation = Quaternion.identity;

        if (SpinRoutine != null)
        {
            StopCoroutine(SpinRoutine);
            SpinRoutine = null;
        }
        if (ShrinkRoutine != null)
        {
            StopCoroutine(ShrinkRoutine);
            ShrinkRoutine = null;
        }
    }
    public void BeginEffect()
    {
        if (ShrinkRoutine == null)
        {
            ContinueRotating = true;
            ShrinkRoutine = StartCoroutine(Shrink());
        }
    }
    
    
    private void TurnOffRandomPartOfCircle() //Deactivate Section of Circle to Create Opening
    {
        int Choice = Random.Range(0, ColList.Count);
        ColList[Choice].gameObject.SetActive(false);
        if (Choice == ColList.Count - 1)
        {
            ColList[0].gameObject.SetActive(false);
        }
        else
        {
            ColList[Choice + 1].gameObject.SetActive(false);
        }
    }



    //Enumerators for Controlling Continuous Movement
    private IEnumerator Shrink()
    {
        if (SpinRoutine == null) { SpinRoutine = StartCoroutine(Spin()); }

        TurnOffRandomPartOfCircle();

        Vector2 startScale = transform.localScale;
        Vector2 EndScale = Vector2.zero;

        float TimePassed = 0f;
        while (TimePassed < EffectDuration)
        {
            float t = TimePassed / EffectDuration;
            float curveT = shrinkCurve.Evaluate(t);

            transform.localScale = Vector3.Lerp(startScale, EndScale, curveT);
            TimePassed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = EndScale;
        ShrinkRoutine = null;

        ResetStats();
    }
    private IEnumerator Spin()
    {
        while (ContinueRotating)
        {
            transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);
            yield return null;
        }
        SpinRoutine = null;
    }
}
