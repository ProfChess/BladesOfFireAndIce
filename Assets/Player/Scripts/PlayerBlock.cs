using System.Collections;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public bool IsBlocking = false;
    //public bool IsBlocking { get; private set; } = false;
    private Vector2 BlockDirection = Vector2.zero;
    private Coroutine BlockingCoroutine;

    [Header("Blocking Stats")]
    [Header("Fire")]
    [Tooltip("Amount of Stamina Consumed Each Second When Holding Block in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHold;
    [Tooltip("Amount of Stamina Consumed When Hit is Blocked in Fire Stance")]
    [SerializeField] private float StaminaConsumedFireHit;
    [Tooltip("Percentage of Damage Blocked in Fire")]
    [SerializeField] private float BlockedDamagePercentageFire;
    [Header("Ice")]
    [Tooltip("Amount of Stamina Consumed Each Second When Holding Block in Ice Stance")]
    [SerializeField] private float StaminaConsumedIceHold;
    [Tooltip("Amount of Stamina Consumed When Hit is Blocked in Ice Stance")]
    [SerializeField] private float StaminaConsumedIceHit;
    [Tooltip("Percentage of Damage Blocked in Ice")]
    [SerializeField] private float BlockedDamagePercentageIce;

    [Header("References")]
    [SerializeField] private PlayerStaminaManager staminaManager;


    public void Block(Vector2 Direction, ElementType Element)
    {
        BlockDirection = Direction;
        IsBlocking = true;
        if (BlockingCoroutine == null)
        {
            if (Element == ElementType.Fire)
            {
                BlockingCoroutine = StartCoroutine(BlockRoutine(StaminaConsumedFireHold));
            }
            else if (Element == ElementType.Ice)
            {
                BlockingCoroutine = StartCoroutine(BlockRoutine(StaminaConsumedIceHold));
            }
        }
    }
    public void ReleaseBlock()
    {
        if (BlockingCoroutine != null)
        {
            StopCoroutine(BlockingCoroutine);
            BlockingCoroutine = null;
            IsBlocking = false;
        }
    }

    private IEnumerator BlockRoutine(float StaminaConsumedOnHold)
    {
        while (true)
        {
            staminaManager.DecreaseStamina(StaminaConsumedOnHold/10);
            yield return new WaitForSeconds(0.1f);
        }
    }


}
