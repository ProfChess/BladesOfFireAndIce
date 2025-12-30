using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseProjectileEffectSpawn : BaseEffectSpawn
{
    //Get Direction to Mouse
    [SerializeField] private Rigidbody2D rb;
    private Vector2 MoveDirection = Vector2.zero;
    private float ProjSpeed = 1f;
    private float ProjDuration = 2f;
    private bool isMoving = false;

    //Visuals
    protected static readonly int EffectEndTrig = Animator.StringToHash("EffectTrigger");

    private Vector2 GetMouseDirection()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 PlayerLocation = GameManager.Instance.getPlayer().transform.position;
        return (mouseWorldPos - PlayerLocation).normalized;
    }

    public void Spawn(Vector2 Location, Vector2 Scaler, float Dam, float StatDuration = 1, float TravelDuration = 2f, float Speed = 1f)
    {
        //Assign 
        Damage = Dam;
        AreaScaler = Scaler;
        EffectStatusDuration = StatDuration;
        MoveDirection = GetMouseDirection();
        ProjSpeed = Speed;
        ProjDuration = TravelDuration;
        isMoving = false;

        //Prep Beginning of Effect
        anim.gameObject.SetActive(false);
        gameObject.transform.position = Location;
        transform.localScale = AreaScaler;

        if (LoopedEffectRoutine == null)
        {
            LoopedEffectRoutine = StartCoroutine(EffectRoutine());
        }
    }
    protected override IEnumerator EffectRoutine()
    {
        yield return new WaitForSeconds(Delay);
        yield return StartCoroutine(DamageEffect());
        End();
    }

    protected override IEnumerator DamageEffect()
    {
        float timeToFinish = ProjDuration - AnimWarmupDuration;
        anim.gameObject.SetActive(true);
        anim.SetTrigger(EffectAnimTrig);
        yield return new WaitForSeconds(AnimWarmupDuration);
        isMoving = true;
        hitbox.enabled = true;
        yield return new WaitForSeconds(timeToFinish);
        hitbox.enabled = false;
        isMoving = false;
        anim.SetTrigger(EffectEndTrig);
    }


    //Moving
    private void FixedUpdate()
    {
        if (isMoving) { rb.velocity = MoveDirection * ProjSpeed; }
        else { rb.velocity = Vector2.zero; }
    }


}
