using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePart_Torch : BasePuzzlePart
{
    [SerializeField] private GameObject runeMark;
    private bool needslit = false;
    private bool lit = false;
    protected override void PartHit(BasePlayerDamage damageSource)
    {
        base.PartHit(damageSource);

        //Ignite or Extinguish Flame Based on Element
        ElementType element = damageSource.GetElement();
        if (element == ElementType.Fire && !lit)
        {
            lit = true;
        }
        if (element == ElementType.Ice && lit)
        {
            lit = false;
        }
        EvaluatePart();
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
        needslit = shouldLightTorch;
        if (needslit) { runeMark.SetActive(true); }
        else { runeMark.SetActive(false); }
    }
    private void MoveMarkLocation()
    {
        float RandomAngle = Random.Range(0f, 360f);
        float angleInRad = RandomAngle * Mathf.Deg2Rad;

        //Direction = (cos(angle), sin(angle))
        Vector2 Direction = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
        runeMark.transform.position += (Vector3)Direction;
    }
}
