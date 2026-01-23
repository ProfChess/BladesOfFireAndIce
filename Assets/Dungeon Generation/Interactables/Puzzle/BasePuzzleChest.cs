using System.Collections.Generic;
using UnityEngine;

public class BasePuzzleChest : Chest
{
    protected bool isLocked = true;
    [SerializeField] protected List<BasePuzzlePart> puzzlePartList = new();

    public override void Interact()
    {
        canOpen();
        if (isLocked)
        {
            DisplayPuzzleInfo();
            return;
        }


        //Spawn Loot as Normal
        base.Interact();
    }

    //Checks all Parts in list to see if chest can be opened
    public void canOpen()
    {
        foreach (BasePuzzlePart part in puzzlePartList)
        {
            if (!part.isCorrect) return;
        }
        isLocked = false;
    }
    protected virtual void DisplayPuzzleInfo()
    {

    }
    public virtual void SetupChest()
    {

    }
}
