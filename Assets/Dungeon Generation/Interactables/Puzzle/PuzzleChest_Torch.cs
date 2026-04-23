using System.Collections.Generic;
using UnityEngine;

public class PuzzleChest_Torch : BasePuzzleChest
{
    private List<PuzzlePart_Torch> puzzleParts = new();

    private void Start()
    {
        SetupChest();
    }

    public override void SetupChest()
    {
        base.SetupChest();
        puzzleParts.AddRange(GetComponentsInChildren<PuzzlePart_Torch>());
        PartSetup();
    }
    private void PartSetup()
    {
        foreach (PuzzlePart_Torch part in puzzleParts)
        {
            int choice = Random.Range(0, 2);
            part.SetUpPart(choice == 0);
            part.EvaluatePart();
            Debug.Log(part.name + " NeedsLit = " + choice);
        }
    }
}
