using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //FSM
    private enum State{idle,running,jumping,falling,hurt};
    private State state = State.idle;
    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;
    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
  
    // Update is called once per frame
    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        VelocityState();
        anim.SetInteger("state",(int)state);
    }
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        
        //left�Bright movement
       if (hDirection < 0) 
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //Moveing Right
       else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

       //   jump
        if (Input.GetKeyDown(KeyCode.W) && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }


    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(state == State.falling)
            {
            Destroy(other.gameObject);
            Jump();
            }
            else
            {
                state = State.hurt;
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therefore I should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //Enemy is to my left therefore I should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void VelocityState()
    {
        if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if(Math.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if(Math.Abs(rb.velocity.x) > 2f)
        {
            //is running
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }
}
