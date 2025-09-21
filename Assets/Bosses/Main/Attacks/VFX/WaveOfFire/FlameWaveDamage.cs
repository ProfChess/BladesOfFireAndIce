using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWaveDamage : BaseAttackDamage
{
    [Header("References")]
    [SerializeField] private Collider2D hitbox;

    private float Speed = 1f;
    private float Duration = 1;

    //Coroutine
    private Coroutine WaveMoveRoutine;

    public void BeginMovement(Vector2 Direction, float MoveSpeed, float MoveDuration)
    {
        Speed = MoveSpeed;
        Duration = MoveDuration;

        //Rotation
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        //Start Routine
        if (WaveMoveRoutine == null)
        {
            WaveMoveRoutine = StartCoroutine(WaveMove());
        }
    }
    private void StopMovement()
    {
        gameObject.SetActive(false);
        BossPoolManager.Instance.ReturnObjectToPool(BossAttackPrefabType.FlameWaves, gameObject);
    }
    
    private IEnumerator WaveMove()
    {
        Debug.Log("Coroutine Started");
        float timepassed = 0f;
        while (timepassed < Duration)
        {
            timepassed += Time.deltaTime;
            transform.Translate(Vector2.up * Speed * Time.deltaTime);
            yield return null;
        }

        StopMovement();
        WaveMoveRoutine = null;
    }
}
