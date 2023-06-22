using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Collider2D collider2d;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider2d = GetComponent<Collider2D>();
    }

    public void SetActive(bool isActive)
    {
        sprite.enabled = isActive;
        collider2d.enabled = isActive;
    }
}
