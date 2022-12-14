using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    Collider2D c2D;
    Rigidbody2D rb2D;
    Collider2D leftCheck, rightCheck;
    public float moveSpeed;
    public bool moving;
    Vector2Int movementStartingPosition;
    // Start is called before the first frame update
    void Start()
    {
        c2D = GetComponent<Collider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move if hit by player
        if(leftCheck != null && rightCheck == null){
            if(!moving && leftCheck.tag == "Entity" && leftCheck.TryGetComponent(out Player player)){
                if(Mathf.Abs(player.moveSpeedCurrent) > 0 && player.grounded && player.grounded != this.gameObject){
                    moving = true;
                    StartCoroutine(Move(1));
                    movementStartingPosition = Vector2Int.RoundToInt(transform.position);
                }
            }
        }else if(rightCheck != null && leftCheck == null){
            if(!moving && rightCheck.tag == "Entity" && rightCheck.TryGetComponent(out Player player)){
                if(Mathf.Abs(player.moveSpeedCurrent) > 0 && player.grounded && player.grounded != this.gameObject){
                    moving = true;
                    StartCoroutine(Move(-1));
                    movementStartingPosition = Vector2Int.RoundToInt(transform.position);
                }
            }
        }
        if(!moving){
            //rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.gravityScale = 6;
            transform.position = Vector2.Lerp(transform.position, Vector2Int.RoundToInt(transform.position), 0.25f);
        }//else{rb2D.bodyType = RigidbodyType2D.Dynamic;}
        leftCheck = null;
        rightCheck = null;
    }
    void OnCollisionEnter2D(Collision2D other){
        //check if hit on Left or Right
        c2D.enabled = false;
        leftCheck = Physics2D.OverlapBox(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.2f), new Vector2(0.25f, 0.5f), 0);
        rightCheck = Physics2D.OverlapBox(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.2f), new Vector2(0.25f, 0.5f), 0);
        c2D.enabled = true;
    }
    
    public IEnumerator Move(int moveTargetMod){
        int timeRemaining = 25;
        while(moving){
            yield return new WaitForFixedUpdate();
            if(Mathf.Abs(transform.position.x - (movementStartingPosition.x + moveTargetMod)) < 0.1f || timeRemaining < 1){
                moving = false;
            } else{
                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(movementStartingPosition.x + moveTargetMod, transform.position.y), moveSpeed);
                Vector2 velocityCurrent = rb2D.velocity;
                transform.position = Vector2.SmoothDamp(transform.position, new Vector2(movementStartingPosition.x + moveTargetMod, transform.position.y), ref velocityCurrent, moveSpeed, Mathf.Infinity);
            }
            if(leftCheck && leftCheck.tag == "Ground" && moving || rightCheck && rightCheck.tag == "Ground" && moving){
                moving = false;
            }
            timeRemaining--;
        }
    }

    public IEnumerator Thump(Vector2 moveTargetMod){
        if(!moving){
            int timeRemaining = 15;
            moving = true;
            movementStartingPosition = Vector2Int.RoundToInt(transform.position);
            while(moving){
                yield return new WaitForFixedUpdate();
                c2D.enabled = false;
                Collider2D ceilingCheck = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(0.75f, 0.2f), 0);
                c2D.enabled = true;
                if(Vector2.Distance(transform.position, movementStartingPosition + moveTargetMod) < 0.1f || ceilingCheck != null || timeRemaining < 1){
                    moving = false;
                }else{
                    rb2D.gravityScale = 0;
                    //transform.position = Vector2.MoveTowards(transform.position, new Vector2(movementStartingPosition.x + moveTargetMod, transform.position.y), moveSpeed);
                    Vector2 velocityCurrent = rb2D.velocity;
                    transform.position = Vector2.SmoothDamp(transform.position, new Vector2(transform.position.x, movementStartingPosition.y + moveTargetMod.y), ref velocityCurrent, moveSpeed / 2, Mathf.Infinity);
                    if(moveTargetMod.x > 0 && !rightCheck || moveTargetMod.x < 0 && !leftCheck){
                        transform.position = Vector2.SmoothDamp(transform.position, new Vector2(movementStartingPosition.x + moveTargetMod.x, transform.position.y), ref velocityCurrent, moveSpeed / 2, Mathf.Infinity);
                    }
                }
                timeRemaining--;
                print("time remaining: " + timeRemaining);
            }
        }
    }

    void OnDrawGizmos(){
        if(moving){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x - 0.5f, transform.position.y + 0.2f), new Vector2(0.25f, 0.5f));
        Gizmos.DrawWireCube(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.2f), new Vector2(0.25f, 0.5f));
    }
}
