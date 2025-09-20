using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SmallExplosion : BaseAttackDamage
{
    [Header("Stats")]
    [SerializeField] private float FadeInTime = 0.25f;
    [SerializeField] private float ExplosionDelayTime = 0.1f;

    [Header("References")]
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer ExplosionSprite;
    [SerializeField] private Animator animator;

    //Animation
    private readonly static int BoomTrig = Animator.StringToHash("BoomTrigger");

    //Routine
    private Coroutine BoomRoutine;


    public void StartExplosion(float Damage)
    {
        AttackDamage = Damage;

        if (BoomRoutine == null)
        {
            BoomRoutine = StartCoroutine(ExplosionRoutine());
        }
    }
    public void FinishObject()
    {
        if (BoomRoutine != null) { StopCoroutine(BoomRoutine); }
        BoomRoutine = null;
        gameObject.SetActive(false);
        BossPoolManager.Instance.ReturnObjectToPool(BossAttackPrefabType.SmallExplosions, gameObject);
    }


    //Collider
    public void ActivateCol()
    {
        col.enabled = true;
        GameObject FlameSpawn = BossPoolManager.Instance.getObjectFromPool(BossAttackPrefabType.LingeringFlames);
        Vector3 Offset = new Vector2(0, 0.25f);
        FlameSpawn.transform.position = gameObject.transform.position + Offset;
    }
    public void DeactivateCol()
    {
        col.enabled = false;
    }


    //Coroutines 
    private IEnumerator ExplosionRoutine()
    {
        yield return StartCoroutine(FadeInRoutine());
        yield return new WaitForSeconds(ExplosionDelayTime);
        animator.SetTrigger(BoomTrig);
    }
    private IEnumerator FadeInRoutine()
    {
        Color tempColor = ExplosionSprite.color;
        tempColor.a = 0;
        ExplosionSprite.color = tempColor;

        float timePassed = 0f;

        while (timePassed < FadeInTime)
        {
            timePassed += Time.deltaTime;
            tempColor.a = Mathf.Clamp01(timePassed / FadeInTime);
            ExplosionSprite.color = tempColor;
            yield return null;
        }
    }

}
