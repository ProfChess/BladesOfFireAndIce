using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossControl : BaseFinalBoss
{
    [Header("Animation")]
    [SerializeField] private Animator BossAnim;
    private AnimatorOverrideController overrideController;
    [SerializeField] private AnimationClip SpellPlaceHolder;
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

}
