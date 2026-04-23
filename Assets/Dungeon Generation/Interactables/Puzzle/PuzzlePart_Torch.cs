using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePart_Torch : BasePuzzlePart
{
    [SerializeField] private GameObject Flame;
    [SerializeField] private GameObject RuneMark;
    private bool needslit = false;                  //Solution for Torch
    private bool lit = false;                       //Current State of The Torch
    protected override void PartHit(BasePlayerDamage damageSource)
    {
        base.PartHit(damageSource);

        //Ignite or Extinguish Flame Based on Element
        ElementType element = damageSource.GetElement();
        if (element == ElementType.Fire && !lit)
        {
            lit = true;
            Flame.SetActive(true);
        }
        if (element == ElementType.Ice && lit)
        {
            lit = false;
            Flame.SetActive(false);
        }
        EvaluatePart();
        Debug.Log("Hit");
    }
    public void EvaluatePart()
    {
        if (needslit == lit)
        {
            isCorrect = true;
        }
        else { isCorrect = false; }
    }
    public void SetUpPart(bool shouldLightTorch)
    {
        MoveMarkLocation();
        lit = Random.Range(0, 2) == 0 ? true : false;
        Flame.SetActive(lit);

        needslit = shouldLightTorch;
        RuneMark.SetActive(needslit);
    }
    private void MoveMarkLocation()
    {
        float RandomAngle = Random.Range(0f, 360f);
        float angleInRad = RandomAngle * Mathf.Deg2Rad;

        //Direction = (cos(angle), sin(angle))
        Vector2 Direction = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
        RuneMark.transform.position += (Vector3)Direction;
    }
}
