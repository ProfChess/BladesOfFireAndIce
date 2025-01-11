using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controls;
    [SerializeField] private BoxCollider2D attackBox;
    private float offsetDistance = 1f;
    private float attackDuration;
    private void Start()
    {
        attackBox.enabled = false;
        attackDuration = controls.GetAttackDuration();
    }

    //Places Attack Box Collider in Direction Player is Moving
    public void BasicAttack()
    {
        //Position Calculations
        Vector2 attackDirection = controls.GetMoveVector() * offsetDistance;                    //Direction
        float rotateAngle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;  //Angle
        attackBox.transform.localPosition = attackDirection;
        attackBox.transform.localRotation = Quaternion.Euler(0, 0, rotateAngle);

        //Activate Collider
        attackBox.enabled = true;
        StartCoroutine(BaseAttackStay());
    }
    private IEnumerator BaseAttackStay() //Collider stays for attack duration
    {
        yield return new WaitForSeconds(attackDuration);
        attackBox.enabled = false;
    }
}
