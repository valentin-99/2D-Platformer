using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected BoxCollider2D boxCol;
    protected Collider2D coll;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void EnemyDeath()
    {
        anim.SetTrigger("death");
    }

    public void UnableEnemyCollider()
    {
        boxCol.enabled = false;
    }

    private void EnemyDestroy()
    {
        Destroy(this.gameObject);
    }
}
