using System.Collections;
using UnityEngine;

public class LingeringFlame : BaseLingeringAOE
{
    [SerializeField] private SpriteRenderer Visuals;
    private Coroutine FadeRoutine;
    private Color ogColor;
    private const float fadeDuration = 0.5f;

    private void Awake()
    {
        ogColor = Visuals.color;
    }
    public void BeginFading() { if (FadeRoutine == null) { FadeRoutine = StartCoroutine(FadeOut()); } }
    private IEnumerator FadeOut()
    {
        float timePassed = 0f;
        Color startingColor = Visuals.color;

        //Fade out 
        while (timePassed < fadeDuration)
        {
            timePassed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timePassed /  fadeDuration);
            Visuals.color = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);
            yield return null;
        }

        //return back to pool goes here
        Disappear();
        FadeRoutine = null;
    }
    protected override void Disappear()
    {
        base.Disappear();
        Visuals.color = ogColor;
    }

}
