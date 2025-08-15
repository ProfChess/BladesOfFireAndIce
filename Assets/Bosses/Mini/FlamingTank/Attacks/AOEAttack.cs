using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : BaseBossAttack
{
    [Header("References")]
    [SerializeField] private CircleCollider2D AttackCol;
    [SerializeField] private Animator BossAnim;
    private bool listeningForPlayer = false;

    //Flame Spawn Locations
    private static readonly Vector3[] FlameLocationOffsets =
    {
        new Vector2(0,2),
        new Vector2(2,0),
        new Vector2(0,-2),
        new Vector2(-2,0),
    };

    //Animation 
    private Coroutine AttackCoroutine;
    private const float AnimWaitTime = 0.45f;
    public override void StartAttack(BossAttackOption AttackOption)
    {
        BossAnim.SetTrigger("AOETrigger");
        if (AttackCoroutine == null) { AttackCoroutine = StartCoroutine(AttackAppearance()); }
        base.StartAttack(AttackOption);
    }
    private IEnumerator AttackAppearance()
    {
        listeningForPlayer = true;
        yield return new WaitForSeconds(AnimWaitTime);
        AttackCol.enabled = true;
        yield return new WaitForSeconds(0.15f);
        AttackCol.enabled = false;
        listeningForPlayer = false;

        //Null Coroutine
        AttackCoroutine = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (listeningForPlayer)
            {
                SpawnFlames();
                listeningForPlayer = false;
            }
        }
    }
    private void SpawnFlames()
    {
        for (int i = 0; i < FlameLocationOffsets.Length; i++)
        {
            GameObject FlameObj = PoolManager.Instance.getObjectFromPool(EnemyType.Flames);
            FlameObj.transform.position = gameObject.transform.position + FlameLocationOffsets[i];
        }
    }

}
