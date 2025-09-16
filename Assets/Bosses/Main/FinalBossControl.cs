using UnityEngine;

public class FinalBossControl : BaseFinalBoss
{
    [Header("Animation")]
    [SerializeField] private SpriteRenderer BossSprite;
    [SerializeField] private Animator BossAnim;
    private AnimatorOverrideController overrideController;
    [SerializeField] private AnimationClip SpellPlaceHolder;

    //Anims
    private static readonly int StunTrigger = Animator.StringToHash("StunTrigger");
    private static readonly int StopStunTrigger = Animator.StringToHash("StunStopTrigger");

    private void Awake()
    {
        overrideController = new AnimatorOverrideController(BossAnim.runtimeAnimatorController);
        BossAnim.runtimeAnimatorController = overrideController;
    }
    public void SwapAnimClip(AnimationClip OriginalClip, AnimationClip NewClip)
    {
        overrideController[OriginalClip] = NewClip;
    }
    public void SwapAttackClip(AnimationClip Clip)
    {
        overrideController[SpellPlaceHolder] = Clip;    
    }
    protected override void MoveUpdate()
    {
        //Add Extra Movement Here if Needed
    }


    //Stun Specifics
    protected override void BossStunExtras()
    {
        BossAnim.SetTrigger(StunTrigger);
        BossSprite.color = Color.grey;
    }
    protected override void StopStunExtras()
    {
        BossAnim.SetTrigger(StopStunTrigger);
        BossSprite.color = Color.white;
    }

}
