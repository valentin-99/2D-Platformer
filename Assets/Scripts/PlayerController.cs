using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    private enum State {idle, run, jump, fall_jump}
    private State state = State.idle;
    
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed;
    [SerializeField] private float jump_force;

    public int cherries;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        speed = 6f;
        jump_force = 14f;
        cherries = 0;
    }

    private void Update()
    {
        Move();
        AnimationSwitch();
        // Sets the animation state
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries++;
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
            rb.velocity = new Vector2(rb.velocity.x, jump_force);
            state = State.jump;
        }
    }

    // Decide animation
    private void AnimationSwitch()
    {
        // If player jumps he starts falling to the ground
        if (state == State.jump)
        {   
            if (rb.velocity.y < .1f)
            {
                state = State.fall_jump;
            }   
        }

        // If player is falling to the ground and he's touching it then switch to idle
        else if (state == State.fall_jump)
        {
            if (coll.IsTouchingLayers(ground))
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
