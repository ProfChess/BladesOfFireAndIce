using System.Collections;
using System.Collections.Generic;
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

    [Header("Attack VFX")]
    [SerializeField] private Animator RightEffect;
    [SerializeField] private Animator LeftEffect;
    private static readonly int EffectTrigger = Animator.StringToHash("Effect Trigger");

    private GameObject Player;
    private Coroutine AttackRoutine;

    //Flame Spawn Locations
    private static readonly Vector3[] RightFlameLocations =
    {
        new Vector2(2,0),
        new Vector2(1,1),
        new Vector2(1,-1),
    };

    private static readonly Vector3[] LeftFlameLocations =
    {
        new Vector2(-2,0),
        new Vector2(-1,1),
        new Vector2(-1,-1),
    };

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
                AttackRoutine = 
                    StartCoroutine(ActivateEachCol(RightCol, LeftCol, 
                    RightFlameLocations, LeftFlameLocations, 
                    RightEffect, LeftEffect));
            }
        }
        else
        {
            if (AttackRoutine == null)
            {
                BossSprite.flipX = true;
                BossAnim.SetTrigger(BlastTrigger);
                AttackRoutine = 
                    StartCoroutine(ActivateEachCol(LeftCol, RightCol, 
                    LeftFlameLocations, RightFlameLocations,
                    LeftEffect, RightEffect));
            }
        }
        base.StartAttack(AttackOption);
    }

    private IEnumerator ActivateEachCol(
        Collider2D firstCol, Collider2D secondCol, 
        Vector3[] FirstFlames, Vector3[] SecondFlames, 
        Animator FirstAnim, Animator SecondAnim)
    {
        FirstAnim.SetTrigger(EffectTrigger);                    //First VFX
        yield return new WaitForSeconds(AnimWaitTime);          //Wait for right moment in anim
        StartCoroutine(ColliderLive(firstCol, 0.12f));          //Spawns Collider
        SpawnFlames(FirstFlames);                               //Spawns Flames 
        yield return new WaitForSeconds(1.0f - AnimWaitTime);   //Waits Until Anim is completed
        BossSprite.flipX = !BossSprite.flipX;                   //Flips direction
        BossAnim.SetTrigger(BlastTrigger);                      //Starts next attack
        SecondAnim.SetTrigger(EffectTrigger);                   //Second VFX
        yield return new WaitForSeconds(AnimWaitTime);          //Wait for right moment in anim
        StartCoroutine(ColliderLive(secondCol, 0.12f));         //Spawns Collider
        SpawnFlames(SecondFlames);                              //Spawns Flames 

        AttackRoutine = null;
    }
    private IEnumerator ColliderLive(Collider2D col, float Time) //Turns collider on and off
    {
        col.enabled = true;
        yield return new WaitForSeconds(Time);
        col.enabled = false;
    }

    private void SpawnFlames(Vector3[] FlameLocations)
    {
        for (int i = 0; i < FlameLocations.Length; i++)
        {
            GameObject FlameObj = PoolManager.Instance.getObjectFromPool(EnemyType.Flames);
            FlameObj.transform.position = gameObject.transform.position + FlameLocations[i];
        }
    }
}
