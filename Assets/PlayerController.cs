using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State {idle, run, jump, fall_jump}
    private State state = State.idle;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Left
        if (Input.GetAxis("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        // Right
        else if (Input.GetAxis("Horizontal") > 0)
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        else
        {
        }

        // Jump
        if ((Input.GetAxis("Vertical") > 0) && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, 7.5f);
            state = State.jump;
        }

        // Switch states
        VelocityState();
        anim.SetInteger("state", (int)state);
    }

    private void VelocityState()
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
