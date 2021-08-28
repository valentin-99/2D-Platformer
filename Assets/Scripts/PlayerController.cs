using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State { idle, run, jump, fallJump, hurt }
    private State state = State.idle;

    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask groundMargins;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float hurtForce;
    [SerializeField] private Text scoreCounter;
    
    private int cherries;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        speed = 6f;
        jumpForce = 14f;
        hurtForce = 6f;
        cherries = 0;
        scoreCounter.text = "0";
    }

    private void Update()
    {
        if (state != State.hurt)
        {
            Move();
        }
        AnimationSwitch();
        // Sets the animation state
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Collision between player and cherries
        if (col.tag == "Collectable")
        {
            Destroy(col.gameObject);
            cherries++;
            scoreCounter.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (state == State.fallJump)
            {
                Destroy(col.gameObject);
                Jump();
            }
            else
            {
                state = State.hurt;
                // Collided object is in front of the player
                if (col.gameObject.transform.position.x > transform.position.x)
                {
                    // move player back
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                // Collided object is behind the player
                else
                {
                    // move player in front
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    // Decide movement
    private void Move()
    {
        // Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // Right
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        // Jump
        if ((Input.GetAxis("Vertical") > 0) && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }

    // Decide animation
    private void AnimationSwitch()
    {
        // If player jumps he starts falling to the ground
        if ((state == State.jump) || (coll.IsTouchingLayers(groundMargins)))
        {
            if (rb.velocity.y < .1f)
            {
                state = State.fallJump;
            }
        }

        // If player is falling to the ground and he's touching it then switch to idle
        else if (state == State.fallJump)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }

        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        // If player is walking
        else if (Mathf.Abs(rb.velocity.x) > 3)
        {
            state = State.run;
        }

        else
        {
            state = State.idle;
        }
    }
}
