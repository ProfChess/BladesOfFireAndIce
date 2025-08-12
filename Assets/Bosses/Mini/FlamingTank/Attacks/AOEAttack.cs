using System.Collections;
using UnityEngine;

public class AOEAttack : BaseBossAttack
{
    [Header("References")]
    [SerializeField] private CircleCollider2D AttackCol;
    [SerializeField] private Animator BossAnim;

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
        yield return new WaitForSeconds(AnimWaitTime);
        AttackCol.enabled = true;
        yield return new WaitForSeconds(0.15f);
        AttackCol.enabled = false;

        //Null Coroutine
        AttackCoroutine = null;
    }

}
