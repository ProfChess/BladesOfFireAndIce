using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardBounce : MonoBehaviour
{
    private float HSpeed = 1.5f;
    private float initialUpForce = 3f;
    private float gravity = 12f;
    private float smallBounceForce = 1f;
    private float groundY = -3f;
    
    public float fadeSpeed = 2f;
    public float fadeDelay = 0.4f;

    private Vector2 vel;
    private bool grounded = false;
    private bool smallBounceFinished = false;
    private float fadeTimer;
    
    [SerializeField] private SpriteRenderer sr;

    public void StartBounce(float HorSpeed, float UpForce, float downForce,
        float miniBounceForce, float YTrigger, float SpeedOfFade, float DelayOfFade)
    {
        HSpeed = HorSpeed;
        initialUpForce = UpForce;
        gravity = downForce;
        smallBounceForce = miniBounceForce;
        groundY = YTrigger;
        fadeSpeed = SpeedOfFade;
        fadeDelay = DelayOfFade;

        float dir = Random.Range(-1f, 1f);

        vel = new Vector2(dir * HSpeed, initialUpForce);
        grounded = false;
        smallBounceFinished = false;

        //reset fade
        var c = sr.color;
        c.a = 1f;
        sr.color = c;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        //Main Bounce
        if (!grounded)
        {
            vel.y -= gravity * Time.deltaTime;
            pos += (Vector3)vel * Time.deltaTime;

            if (pos.y <= groundY)
            {
                pos.y = groundY;
                grounded = true;

                //trigger smaller bounce
                vel.y = smallBounceForce;
            }

            transform.position = pos;
            return;
        }

        //Small Bounce 
        if (!smallBounceFinished)
        {
            vel.y -= gravity * Time.deltaTime;
            pos += (Vector3)vel * Time.deltaTime;

            if (pos.y <= groundY)
            {
                pos.y = groundY;
                smallBounceFinished = true;
                fadeTimer = fadeDelay;
                vel = Vector2.zero;
            }

            transform.position = pos;
            return;
        }

        //Fade after finished
        fadeTimer -= Time.deltaTime;
        if (fadeTimer <= 0)
        {
            Color c = sr.color;
            c.a -= fadeSpeed * Time.deltaTime;
            sr.color = c;
            if (c.a <= 0f) { gameObject.SetActive(false); }
        }
    }
}
