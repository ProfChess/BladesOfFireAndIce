using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffectSpawn : MonoBehaviour
{
    //Object Details
    [Header("References")]
    [SerializeField] protected Collider2D hitbox;
    [SerializeField] protected Animator anim;

    //Effect Details
    [Header("Anim Details")]
    [SerializeField] protected float Delay = 0f;               //Time between spawning and animation start
    [SerializeField] protected float AnimWarmupDuration = 0f;  //Time between anim start and damage activates
    [SerializeField] protected float DamageDuration = 0.1f;    //Time that damage collider lasts

    //Pool
    protected PlayerEffectObjectType Pool;

    //Damage 
    protected float Damage = 1f;

    //Visuals
    protected Coroutine DamageRoutine = null;
    private static readonly int AnimTrig = Animator.StringToHash("AnimStart");


    //This is called whenever the effect library needs to place and start and object
    public virtual void Spawn(Vector2 Location, float Dam)
    {
        Damage = Dam;
        anim.gameObject.SetActive(false);
        gameObject.transform.position = Location;
        if (DamageRoutine == null) { DamageRoutine = StartCoroutine(DamageEffect()); }
    }

    //Cleans up the object and sends it back to the pool for reuse
    protected virtual void End()
    {
        DamageRoutine = null;
        if (hitbox != null) { hitbox.enabled = false; }
        gameObject.SetActive(false);
        if (PlayerEffectPoolManager.Instance != null)
        {
            PlayerEffectPoolManager.Instance.ReturnObjectToPool(Pool, gameObject);
        }
        else { Debug.Log("Could Not Return to: " + Pool.ToString() + " Pool"); }
    }

    //Goes through animation, playing effects and activating damage collider
    protected virtual IEnumerator DamageEffect()
    {
        float timeToFinish = 1f - AnimWarmupDuration - DamageDuration;
        yield return new WaitForSeconds(Delay);
        anim.gameObject.SetActive(true);
        anim.SetTrigger(AnimTrig);
        yield return new WaitForSeconds(AnimWarmupDuration);
        hitbox.enabled = true;
        yield return new WaitForSeconds(DamageDuration);
        hitbox.enabled = false;
        yield return new WaitForSeconds(timeToFinish);
        End();
    }
    public virtual float GetDamage()
    {
        return Damage;
    }
}
