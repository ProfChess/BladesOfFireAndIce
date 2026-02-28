using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicCircleAnim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer circleSprite;

    public Color fireColor = Color.red;
    public Color iceColor = Color.blue;

    [SerializeField] private float transitionDuration = 1f;

    private Coroutine switchingRoutine;
    private void Start()
    {
        circleSprite.color = PlayerController.PlayerAttackForm == ElementType.Fire ? fireColor : iceColor;
    }
    public void SetSpriteFlip(bool x)
    {
        Vector3 startPos = gameObject.transform.localPosition;
        startPos.x = x ? 0.15f : -0.15f;
        gameObject.transform.localPosition = startPos;
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
