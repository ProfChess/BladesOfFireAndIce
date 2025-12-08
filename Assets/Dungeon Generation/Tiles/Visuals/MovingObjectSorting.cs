using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSorting : MonoBehaviour
{
    [SerializeField] private SpriteRenderer Visual;
    [SerializeField] private Transform FeetLevelObject;
    private Vector3 lastPosition;

    private void Awake()
    {
        //Assign Sorting Order to be Negative of the Feet position (Lower = More In Front)
        Visual.sortingOrder = Mathf.RoundToInt(-FeetLevelObject.position.y * 100);
    }

    //If Object Moves, Update Sorting Order
    private void LateUpdate()
    {
        if (FeetLevelObject.position != lastPosition)
        {
            Visual.sortingOrder = Mathf.RoundToInt(-FeetLevelObject.position.y * 100);
            lastPosition = transform.position;
        }
    }
}
