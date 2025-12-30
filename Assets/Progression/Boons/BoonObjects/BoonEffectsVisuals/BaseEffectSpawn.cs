using System.Collections;
using UnityEngine;

public class BaseEffectSpawn : MonoBehaviour
{
    //Object Details
    [Header("References")]
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected Animator anim;
    [SerializeField] protected AnimationClip EffectAnimation;
    private AnimatorOverrideController overrideAnimController;

    //Effect Details
    [Header("Anim Details")]
    [SerializeField] protected float Delay = 0f;               //Time between spawning and animation start
    [SerializeField] protected float AnimWarmupDuration = 0f;  //Time between anim start and damage activates
    [SerializeField] protected float DamageDuration = 0.1f;    //Time that damage collider lasts

    //Pool
    protected PlayerEffectObjectType Pool;

    //Provided Stats -> Given From EffectLibrary
    protected float Damage = 1f;
    protected Vector2 AreaScaler = Vector2.one; //Makes the Effect Larger
    protected int EffectFreq = 1;               //How Many Times the Effect Goes off Per Triggered Event
    protected float EffectStatusDuration = 0f;  //Only Used By Boons that apply Statuses

    protected Coroutine LoopedEffectRoutine;

    //Visuals
    protected static readonly int EffectAnimTrig = Animator.StringToHash("EffectTrigger");

    //This is called whenever the effect library needs to place and start and object
    public virtual void Spawn(Vector2 Location, float Dam)
    {
        Spawn(Location, Vector2.one, Dam);
    }
    public virtual void Spawn(Vector2 Location, Vector2 Scaler, float Dam, int Freq = 1, float Duration = 1f)
    {
        //Assign 
        Damage = Dam;
        AreaScaler = Scaler;
        EffectFreq = Freq;
        EffectStatusDuration = Duration;

        //Prep Beginning of Effect
        anim.gameObject.SetActive(false);
        gameObject.transform.position = Location;
        transform.localScale = AreaScaler;

        if (LoopedEffectRoutine == null)
        {
            LoopedEffectRoutine = StartCoroutine(EffectRoutine());
        }
    }

    //Cleans up the object and sends it back to the pool for reuse
    protected virtual void End()
    {
        LoopedEffectRoutine = null;
        if (hitbox != null) { hitbox.enabled = false; }
        gameObject.SetActive(false);
        if (PlayerEffectPoolManager.Instance != null)
        {
            PlayerEffectPoolManager.Instance.ReturnObjectToPool(Pool, gameObject);
        }
        else { Debug.Log("Could Not Return to: " + Pool.ToString() + " Pool"); }
    }

    protected virtual IEnumerator EffectRoutine()
    {
        yield return new WaitForSeconds(Delay);
        for (int i = 0; i < EffectFreq; i++)
        {
            yield return StartCoroutine(DamageEffect());
        }
        End();
    }

    protected virtual IEnumerator DamageEffect()
    {
        float timeToFinish = 1f - AnimWarmupDuration - DamageDuration;
        anim.gameObject.SetActive(true);
        anim.SetTrigger(EffectAnimTrig);
        yield return new WaitForSeconds(AnimWarmupDuration);
        hitbox.enabled = true;
        yield return new WaitForSeconds(DamageDuration);
        hitbox.enabled = false;
        yield return new WaitForSeconds(timeToFinish);
    }

    //Damage to Be Found by Enemies
    public virtual float GetDamage()
    {
        return Damage;
    }



    //Starting Settings
    protected void Awake()
    {
        overrideAnimController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = overrideAnimController;
    }

    protected void Start()
    {
        //Setup Animation Upon Beginning
        overrideAnimController["DefaultEffect"] = EffectAnimation;
    }
}
