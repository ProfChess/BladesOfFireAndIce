using System.Collections;
using UnityEngine;

public class FireDoubleBlast : BaseBossAttack
{
    [Header("Colliders")]
    [SerializeField] private Collider2D RightCol;
    [SerializeField] private Collider2D LeftCol;
    private const float AnimWaitTime = 0.5f;

    [Header("Sprite and Animation")]
    [SerializeField] private SpriteRenderer BossSprite;
    [SerializeField] private Animator BossAnim;
    private static readonly int BlastTrigger = Animator.StringToHash("BlastTrigger");
    
    
    private GameObject Player;
    private Coroutine AttackRoutine;
    protected override void Start()
    {
        base.Start();
        Player = GameManager.Instance.getPlayer();
    }
    public override void StartAttack(BossAttackOption AttackOption)
    {
        //Determines if player is to the right or left
        bool PlayerToRightofBoss = Player.transform.position.x >= gameObject.transform.position.x;

        //Attack in direction of player, then opposite direction
        if (PlayerToRightofBoss)
        {
            if (AttackRoutine == null)
            {
                BossSprite.flipX = false;
                BossAnim.SetTrigger(BlastTrigger);
                AttackRoutine = StartCoroutine(ActivateEachCol(RightCol, LeftCol));
            }
        }
        else
        {
            if (AttackRoutine == null)
            {
                BossSprite.flipX = true;
                BossAnim.SetTrigger(BlastTrigger);
                AttackRoutine = StartCoroutine(ActivateEachCol(LeftCol, RightCol));
            }
        }
        base.StartAttack(AttackOption);
    }

    private IEnumerator ActivateEachCol(Collider2D firstCol, Collider2D secondCol)
    {
        yield return new WaitForSeconds(AnimWaitTime);          //Wait for right moment in anim
        StartCoroutine(ColliderLive(firstCol, 0.12f));          //Spawns Collider
        yield return new WaitForSeconds(1.0f - AnimWaitTime);   //Waits Until Anim is completed
        BossSprite.flipX = !BossSprite.flipX;                   //Flips direction
        BossAnim.SetTrigger(BlastTrigger);                      //Starts next attack
        yield return new WaitForSeconds(AnimWaitTime);          //Wait for right moment in anim
        StartCoroutine(ColliderLive(secondCol, 0.12f));         //Spawns Collider
        AttackRoutine = null;
    }
    private IEnumerator ColliderLive(Collider2D col, float Time) //Turns collider on and off
    {
        col.enabled = true;
        yield return new WaitForSeconds(Time);
        col.enabled = false;
    }
}
