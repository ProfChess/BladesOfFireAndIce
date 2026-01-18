using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public abstract class LootBase : InteractableObject
{
    //Movement Specifics for Arc Motion
    private bool isMoving = false;              //Flag for controlling movement
    private Vector2 targetPos = Vector2.zero;   //Target to move towards
    private Vector2 velocity = Vector2.zero;    //movement vector
    private const float gravity = 12f;          //downward force
    private const float arcMulti = 1.35f;       //affects height of arc
    private const float totalTime = 1f;         //total time for movement -> changing will affect speed of movement
    private float elapsedTime = 0f;             //tracker for time passed
    public void beginMove(Vector2 start, Vector2 Tar)
    {
        //Disable collider
        gameObject.GetComponent<Collider2D>().enabled = false;

        //Start at chest location
        gameObject.transform.position = start;

        //Formula for Arc Movement
        Vector2 delta = Tar - start;
        velocity.x = delta.x / totalTime;
        velocity.y = (delta.y + 0.5f * gravity * totalTime * totalTime * arcMulti) / totalTime;
        targetPos = Tar;

        //Start Moving
        isMoving = true;
        elapsedTime = 0f;
    }
    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            
            //Decend according to gravity and arcmultiplier
            velocity.y -= gravity * arcMulti * Time.deltaTime;
            transform.position += (Vector3)velocity * Time.deltaTime;
            
            //Track time, disable movement and reenable collider once enough time has passed
            if (elapsedTime >= totalTime)
            {
                transform.position = targetPos;
                isMoving = false;
                gameObject.GetComponent<Collider2D>().enabled = true;
            }
        }
    }
}
