using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer PlayerSprite;
    [SerializeField] private Vector2 SpriteFaceLeft = Vector2.zero;
    [SerializeField] private Vector2 SpriteFaceRight = Vector2.zero;

    private void Update()
    {
        gameObject.transform.localPosition = PlayerSprite.flipX ? SpriteFaceLeft : SpriteFaceRight;
    }
}
