using System;
using System.Collections;
using UnityEngine;

public class BaseEffectSpawn : MonoBehaviour
{
    //Pool Access
    BasePoolManager<PlayerEffectObjectType> PM => PlayerEffectPoolManager.Instance;

    //Object Details
    [Header("References")]
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected Animator anim;
    [SerializeField] protected AnimationClip EffectAnimation;
    private AnimatorOverrideController overrideAnimController;

    //Effect Details
    [Header("Anim Details")]
    [Tooltip("Time Between Spawning and Anim Start")]
    [SerializeField] protected float Delay = 0f;
    [Tooltip("Time Between Anim Start and Damage Start")]
    [SerializeField] protected float AnimWarmupDuration = 0f;
    [Tooltip("Time That Damage Collider Lasts")]
    [SerializeField] protected float DamageDuration = 0.1f;    

    //Events and Enums
    private enum EffectDamageType { Boon = 0, Ability = 1 }
    [SerializeField] private EffectDamageType damageType;

    public static event Action<PlayerEventContext> OnAbilityDamage;
    public static event Action<PlayerEventContext> OnAbilityKill;
    private AttackEventContext AbilityDamageContext = new();


    //Pool
    [SerializeField] protected PlayerEffectObjectType Pool;

    //Provided Stats -> Given From EffectLibrary
    protected float Damage = 1f;
    protected Vector2 AreaScaler = Vector2.one; //Makes the Effect Larger
    protected int EffectFreq = 1;               //How Many Times the Effect Goes off Per Triggered Event
    protected float EffectStatusDuration = 0f;  //Only Used By Boons that apply Statuses

    //Routine
    protected Coroutine LoopedEffectRoutine;

    //Ignore Enemy (In Case it Spawns on an Enemy and I want to Ignore Collisions on it)
    [HideInInspector] public GameObject ignoredEnemy;

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
        ignoredEnemy = null;
        if (PM != null)
        {
            PM.ReturnObjectToPool(Pool, gameObject);
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
    public virtual float GetDamage(BaseHealth EnemyHealth)
    {
        switch (damageType)
        {
            case EffectDamageType.Boon: break;
            case EffectDamageType.Ability:

                //Set up Damage Settings
                AbilityDamageContext.Setup(
                    PlayerController.PlayerAttackForm,
                    EnemyHealth.transform.position - gameObject.transform.position, 
                    EnemyHealth, hitbox.transform.position);

                //Trigger Events
                OnAbilityDamage?.Invoke(AbilityDamageContext);
                if (EnemyHealth.CurrentHealth <= Damage) { OnAbilityKill?.Invoke(AbilityDamageContext); }
                break;
        }

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
