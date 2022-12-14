using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    audioManager audioManager;
    Rigidbody2D rb2D;
    Collider2D c2D;
    Collider2D ceilingCheck;
    Collider2D[] groundCheckAll;
    SpriteRenderer spriteRenderer;
    Animator animator;
    public float moveSpeed;
    public float moveSpeedCurrent;
    public float jumpSpeed;
    public Vector2 maxJumpDistance;
    public bool jumping;
    public GameObject grounded;
    public GameObject carrying;
    public int playerIndex, sliding;
    public Vector2Int lastGroundedPosition;
    public GameObject landParticle;
    public float slideMod;
    public string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioManager = audioManager.Instance;
    }

    void Update(){
        switch(playerIndex){
            //player controls for bambi(0) && the hare(1)
            case 0:
                moveSpeedCurrent = (moveSpeed * Input.GetAxisRaw("Horizontal"));
                if(Input.GetButtonDown("Jump") && grounded && carrying == null ){
                    audioManager.Play("jump");
                    jumping = true;
                }
                if(!grounded || Mathf.Abs(lastGroundedPosition.y - (int)transform.position.y) > maxJumpDistance.y){
                    jumping = false;
                    slideMod = 0;
                }
            break;
            case 1:
                moveSpeedCurrent = (moveSpeed * Input.GetAxisRaw("Horizontal2"));
                if(Input.GetButtonDown("Jump2") && grounded && carrying == null){
                    audioManager.Play("jump");
                    jumping = true;
                }
                if(!grounded || Mathf.Abs(lastGroundedPosition.y - (int)transform.position.y) > maxJumpDistance.y){
                    jumping = false;
                    slideMod = 0;
                }
            break;
        }
        Animate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //player abilities
        switch(playerIndex){
            case 1:
                if(!grounded && rb2D.velocity.y < -2f && sliding == 0){
                    //print("groundPound");
                    c2D.enabled = false;
                    Collider2D[] slamDown = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.5f), 0);
                    Collider2D[] slamSides = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(2f, 2f), 0);
                    c2D.enabled = true;
                    foreach(Collider2D i in slamDown){
                        if(i.tag != "ignoreSlam"){
                            audioManager.Play("groundPound");
                            if(!grounded && landParticle != null){Instantiate(landParticle, new Vector2(transform.position.x, transform.position.y - transform.localScale.y/2), Quaternion.identity);}
                        }
                        if(i.TryGetComponent(out Breakable breakable)){
                            breakable.Break();
                            transform.position = new Vector2(transform.position.x, (int)transform.position.y);
                            jumping = true;
                        }
                    }
                    if(slamDown.Length > 0){
                        GM.inst.ScreenShake(2, 100);
                        foreach(Collider2D i in slamSides){
                            if(i.TryGetComponent(out Push push)){
                                StartCoroutine(push.Thump(new Vector2((i.transform.position.x - transform.position.x) * 0.75f, 1.75f)));
                            }
                            if(i.TryGetComponent(out Breakable breakable)){
                                breakable.Break();
                                transform.position = new Vector2(transform.position.x, (int)transform.position.y);
                            }
                        }
                    }
                }
            break;
        }

        //check if grounded and sliding
        c2D.enabled = false;
        groundCheckAll = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.1f), 0);
        Collider2D[] groundCheckSlide = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.75f), new Vector2(0.2f, 0.5f), 0);
        c2D.enabled = true;
        foreach(Collider2D i in groundCheckAll){
            if(i.tag == "Ground" || i.tag == "Nut" || i.tag == "Slope" || i.tag == "SlopeRight"){
                grounded = i.gameObject;
                lastGroundedPosition = Vector2Int.RoundToInt(transform.position);
                rb2D.gravityScale = 7f;
            } else if(i.tag == "Entity" && i.TryGetComponent(out Player player)){
                if(player.grounded == true){
                    grounded = i.gameObject;
                    lastGroundedPosition = Vector2Int.RoundToInt(transform.position);
                    rb2D.gravityScale = 7f;
                }
            }
        }
        foreach(Collider2D i in groundCheckSlide){
            if(i.tag == "Slope"){
                jumping = false;
                sliding = 1;
                rb2D.gravityScale = 20f;
                break;
            }else if(i.tag == "SlopeRight"){
                jumping = false;
                sliding = 2;
                rb2D.gravityScale = 20f;
                break;
            }else {
                sliding = 0;
            }
        }
        if(groundCheckAll.Length < 1){
            grounded = null; 
            rb2D.gravityScale = 2.5f; 
            sliding = 0;
        }
        //stop jump when hitting ceiling
        c2D.enabled = false;
        ceilingCheck = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(0.5f, 0.25f), 0);
        c2D.enabled = true;
        if(ceilingCheck != null && (ceilingCheck.tag == "Entity" || ceilingCheck.tag == "Ground")){
            jumping = false;
            if(ceilingCheck.tag == "Entity"){
                if(carrying){carrying.transform.parent = null;}
                carrying = ceilingCheck.gameObject;
                carrying.transform.parent = this.transform;
            }
        }else{
            if(carrying){carrying.transform.parent = null;}
            carrying = null;
        }
        //set velocity while jumping
        if(jumping){rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);}
        //move horizontal
        /*
        if(sliding && Mathf.Abs(slideMod) < moveSpeed){slideMod += 0.5f;}
        else if(slideMod > 0 && (!sliding)){slideMod -= 0.5f;}
        if(Mathf.Abs(slideMod) < 0.1f){slideMod = 0;}*/
        //if(Mathf.Abs(slideMod) >= moveSpeed){moveSpeedCurrent = 0;}
        if(sliding == 1){slideMod = -3;}
        else if(sliding == 2){slideMod = 3f;}
        
        if(slideMod == 0 && sliding == 0){
            rb2D.velocity = new Vector2(moveSpeedCurrent + slideMod, rb2D.velocity.y);
            //audioManager.Play("walk");
        }
        else{
            rb2D.velocity = new Vector2(slideMod, rb2D.velocity.y);
        }

        if(slideMod != 0 && sliding == 0){slideMod = Mathf.Lerp(slideMod, 0, 0.1f);}
        if(Mathf.Abs(slideMod) < 0.2f){slideMod = 0;}
        //limit horizontal jump distance
        if(Mathf.Abs(lastGroundedPosition.x - transform.position.x) > maxJumpDistance.x){
            print("falling now");
            rb2D.velocity = new Vector2(Mathf.Lerp(rb2D.velocity.x, 0, 0.7f), rb2D.velocity.y);
        }
    }

    void Animate(){
        string newAnimation = "Stand";
        if (Mathf.Abs(moveSpeedCurrent) > 0.01f){
            spriteRenderer.flipX = moveSpeedCurrent < 0f;
            newAnimation = "Walk";
        }else{
            newAnimation = "Stand";
        }
        if(!grounded){
            newAnimation = "Jump";
        }else if(sliding != 0){
            newAnimation = "Slide";
        }
        if(currentAnimation != newAnimation){
            currentAnimation = newAnimation;
            animator.Play(currentAnimation);
        }
    }

    void OnDrawGizmos(){
        if(grounded){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.75f, 0.1f));
        if(sliding > 0){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.75f), new Vector2(0.2f, 0.5f));
        if(jumping){Gizmos.color = Color.green;}else{Gizmos.color = Color.red;}
        Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y + 0.5f), new Vector2(0.5f, 0.25f));
        if(playerIndex == 1 && Application.isPlaying){
            if(!grounded && rb2D.velocity.y < 0){Gizmos.color = Color.yellow;}else{Gizmos.color = Color.gray;}
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.5f, 0.2f));
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y), new Vector2(2f, 0.25f));
        }
    }
}
