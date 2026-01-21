using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PuzzleChest_Rune : BasePuzzleChest
{
    private List<PuzzlePart_Rune> puzzleParts = new();
    private List<PuzzlePart_Rune> puzzlePartCopy = new();
    public int currentOrder = 0;

    public override void SetupChest()
    {
        puzzleParts.AddRange(GetComponentsInChildren<PuzzlePart_Rune>());
        DecidePuzzleOrder();
    }
    private void Start()
    {
        puzzleParts.AddRange(GetComponentsInChildren<PuzzlePart_Rune>());
        DecidePuzzleOrder();
    }
    private void DecidePuzzleOrder()
    {
        puzzlePartCopy.Clear();
        puzzlePartCopy.AddRange(puzzleParts);

        for(int i = 0; i < puzzleParts.Count; i++)
        {
            int choice = Random.Range(0, puzzlePartCopy.Count);
            puzzlePartCopy[choice].SetOrderNum(i + 1);
            puzzlePartCopy.RemoveAt(choice);
        }
    }
    public void AddToOrder()
    {
        currentOrder++;
    }
    public void ResetPuzzle()
    {
        currentOrder = 0;
        foreach(var BasePuzzlePart in puzzleParts)
        {
            BasePuzzlePart.isCorrect = false;
        }
    }
}
