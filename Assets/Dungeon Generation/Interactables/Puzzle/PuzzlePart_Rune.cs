using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePart_Rune : BasePuzzlePart
{
    [SerializeField] private ElementType Element;
    [SerializeField] private PuzzleChest_Rune runeChest;
    private int orderNum;
    public bool isPressed = false;

    //Check for Same Element as Rune for Sequence Application
    protected override void PartHit(BasePlayerDamage damageSource)
    {
        base.PartHit(damageSource);
        ElementType damageElement = damageSource.GetElement();
        if(damageElement == Element && !isCorrect)
        {
            if (runeChest.currentOrder == orderNum - 1)
            {
                isCorrect = true;
                runeChest.AddToOrder();
                Debug.Log(this.name + " Hit");
            }
            else
            {
                runeChest.ResetPuzzle();
            }
        }
    }
    public void SetOrderNum(int num)
    {
        orderNum = num;
        Debug.Log(this.name + "OrderNum: " + orderNum);
    }
}
