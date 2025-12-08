using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class StaticObjectSorting : MonoBehaviour
{
    [SerializeField] private Transform FeetLevelObject;

    //Assign Sorting Order to be Negative of the Feet position (Lower = More In Front)
    private void Start()
    {
        var Visual = GetComponent<SpriteRenderer>();
        Visual.sortingOrder = Mathf.RoundToInt(-FeetLevelObject.position.y * 100);
    }
}
