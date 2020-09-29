using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    [SerializeField] Transform player;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    float agroRange;
    float movementSpeed;
    private float leftCap;
    private float rightCap;

    private bool facingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        agroRange = 9f;
        movementSpeed = 5f;
        leftCap = transform.position.x - 3;
        rightCap = transform.position.x + 3;
    }

    private bool isGrounded()
    {
        RaycastHit2D rc = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, layerMask);
        return rc.collider != null;
    }

    void HandleMovement()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if(distToPlayer <= agroRange)
        {
            AgroMovement();
        }
        else
        {
            IdleMovement();
        }
    }

    private void IdleMovement()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (isGrounded())
                {
                    rigidBody.velocity = new Vector2(-movementSpeed, transform.position.y);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (isGrounded())
                {
                    rigidBody.velocity = new Vector2(movementSpeed, transform.position.y);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private void AgroMovement()
    {
        if(transform.position.x < player.position.x)
        {
            rigidBody.velocity = new Vector2(movementSpeed, rigidBody.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            rigidBody.velocity = new Vector2(-movementSpeed, rigidBody.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
}
