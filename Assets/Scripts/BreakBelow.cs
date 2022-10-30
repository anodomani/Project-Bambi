using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBelow : MonoBehaviour
{
    GM gM;
    Collider2D c2D;
    public GameObject landParticle;

    void Start(){
        gM = GM.inst;
        c2D = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other){
        //check if grounded
        c2D.enabled = false;
        Collider2D[] slam = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.5f), 0);
        c2D.enabled = true;
        foreach(Collider2D i in slam){
            if(i.TryGetComponent(out Breakable breakable)){
                if(slam.Length > 0 && landParticle != null){Instantiate(landParticle, new Vector2(transform.position.x, transform.position.y - transform.localScale.y/2), Quaternion.identity);}
                gM.ScreenShake(2, 100);
                breakable.Break();
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.5f));
    }
}
