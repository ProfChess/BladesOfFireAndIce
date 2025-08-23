using System.Collections;
using UnityEngine;

public abstract class BaseFinalBossAttack : BaseBossAttack
{
    [Header("Animation")]
    [SerializeField] protected AnimationClip AttackAnimClip;
    [SerializeField] protected Animator BossAnimator;
    protected static readonly int SpellTrigger = Animator.StringToHash("SpellCast_Trigger");


    protected Coroutine AttackRoutine;

    protected FinalBossTeleport BossTeleport;
    private void Awake()
    {
        Transform ParentObject = GetComponentInParent<Transform>();
        BossTeleport = ParentObject.GetComponentInParent<FinalBossTeleport>();
    }
    public override void StartAttack(BossAttackOption AttackOption)
    {
        //Attack Logic
        if (AttackRoutine == null)
        {
            if (BossRef is FinalBossControl finalBoss)
            {
                finalBoss.SwapAttackClip(AttackAnimClip);
            }
            AttackRoutine = StartCoroutine(SpellCastRoutine());
        }

        //Cooldown
        base.StartAttack(AttackOption);
    }
    protected abstract IEnumerator SpellCastRoutine();
}
