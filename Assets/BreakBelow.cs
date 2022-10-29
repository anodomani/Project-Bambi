using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBelow : MonoBehaviour
{
    Collider2D c2D;

    void Start(){
        c2D = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other){
        //check if grounded
        c2D.enabled = false;
        Collider2D[] slam = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.5f), 0);
        c2D.enabled = true;
        foreach(Collider2D i in slam){
            if(i.TryGetComponent(out Breakable breakable)){
                breakable.Break();
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.5f));
    }
}
