using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb2D;
    Collider2D c2D;
    Collider2D groundCheck, ceilingCheck;
    public float moveSpeed;
    public float moveSpeedCurrent;
    public float jumpSpeed;
    public Vector2 maxJumpDistance;
    public bool grounded, jumping;
    public GameObject carrying;
    public int playerIndex;
    public Vector2Int lastGroundedPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
    }

    void Update(){
        switch(playerIndex){
            //player controls for bambi(0) && the hare(1)
            case 0:
                moveSpeedCurrent = moveSpeed * Input.GetAxis("Horizontal");
                if(Input.GetButtonDown("Jump") && grounded && carrying == null ){
                    jumping = true;
                }
                if(!grounded || Mathf.Abs(lastGroundedPosition.y - (int)transform.position.y) > maxJumpDistance.y){
                    jumping = false;
                }
            break;
            case 1:
                moveSpeedCurrent = moveSpeed * Input.GetAxis("Horizontal2");
                if(Input.GetButtonDown("Jump2") && grounded && carrying == null){
                    jumping = true;
                }
                if(!grounded || Mathf.Abs(lastGroundedPosition.y - (int)transform.position.y) > maxJumpDistance.y){
                    jumping = false;
                }
                if(!grounded && rb2D.velocity.y < -2f){
                    //print("groundPound");
                    Collider2D[] slamDown = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.5f), 0);
                    Collider2D[] slamSides = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(2f, 0.5f), 0);
                    foreach(Collider2D i in slamDown){
                        if(i.TryGetComponent(out Breakable breakable)){
                            breakable.Break();
                        }
                    }
                    if(slamDown.Length > 1){
                        foreach(Collider2D i in slamSides){
                            if(i.TryGetComponent(out Push push)){
                                push.Move((int)(i.transform.position.x - transform.position.x));
                            }
                            if(i.TryGetComponent(out Breakable breakable)){
                                breakable.Break();
                            }
                        }
                    }
                }
            break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if grounded
        c2D.enabled = false;
        groundCheck = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.25f), 0);
        c2D.enabled = true;
        if(groundCheck != null){
            if(groundCheck.tag == "Ground" || groundCheck.tag == "Entity"){
                grounded = true;
                lastGroundedPosition = Vector2Int.RoundToInt(transform.position);
            }
        }else{
            grounded = false;
        }
        //stop jump when hitting ceiling
        c2D.enabled = false;
        ceilingCheck = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(0.5f, 0.25f), 0);
        c2D.enabled = true;
        if(ceilingCheck != null && (ceilingCheck.tag == "Entity" || ceilingCheck.tag == "Ground")){
            jumping = false;
            if(ceilingCheck.tag == "Entity"){
                carrying = ceilingCheck.gameObject;
                carrying.transform.parent = this.transform;
            }
        }
        else{carrying = null;}
        //stick to ground while grounded
        if(grounded){rb2D.gravityScale = 7f;}else{rb2D.gravityScale = 2.5f;}
        //set velocity while jumping
        if(jumping){rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);}
        //move horizontal
        rb2D.velocity = new Vector2(moveSpeedCurrent, rb2D.velocity.y);
        //limit horizontal jump distance
        //print("current jump distance: " + (int)(Mathf.Abs(lastGroundedPosition.x - transform.position.x)*10));
        if(Mathf.Abs(lastGroundedPosition.x - transform.position.x) > maxJumpDistance.x){
            rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x, 0, 0.5f), rb2D.velocity.y);
        }
    }

    void OnDrawGizmos(){
        if(grounded){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.25f));
        if(jumping){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(0.5f, 0.25f));
        if(playerIndex == 1 && Application.isPlaying){
            if(!grounded && rb2D.velocity.y < 0){Gizmos.color = Color.yellow;}else{Gizmos.color = Color.gray;}
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.5f));
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y), new Vector2(2f, 0.5f));
        }
    }
}
