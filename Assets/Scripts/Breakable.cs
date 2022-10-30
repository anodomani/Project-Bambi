using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    bool broken;
    Collider2D c2D;
    SpriteRenderer spriteRenderer;

    void Start(){
        c2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Break(){
        c2D.enabled = false;
        spriteRenderer.color = Color.grey;
    }
}
