using System.Collections;
using UnityEngine;

public class PlayerMagicCircleAnim : BaseEffectVisual
{
    [SerializeField] private SpriteRenderer circleSprite;

    public Color fireColor = Color.red;
    public Color iceColor = Color.blue;

    [SerializeField] private float transitionDuration = 1f;

    private Coroutine switchingRoutine;
    private void Start()
    {
        circleSprite.color = PlayerSwitchElements.PlayerAttackForm == ElementType.Fire ? fireColor : iceColor;
    }
    public void SwitchToFire()
    {
        if (switchingRoutine == null)
        {
            switchingRoutine = StartCoroutine(SwapToColor(iceColor, fireColor, transitionDuration));
        }
    }
    public void SwitchToIce()
    {
        if (switchingRoutine == null)
        {
            switchingRoutine = StartCoroutine(SwapToColor(fireColor, iceColor, transitionDuration));
        }
    }

    //Coroutine
    private IEnumerator SwapToColor(Color start, Color end, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            circleSprite.color = Color.Lerp(start, end, time/duration);
            time += GameTimeManager.GameDeltaTime;
            yield return null;
        }

        circleSprite.color = end;
        switchingRoutine = null;    
    }
}
