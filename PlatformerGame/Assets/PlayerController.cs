using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;
    private bool canDoubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private bool isGrounded()
    {
        RaycastHit2D rc = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, layerMask);
        return rc.collider != null;
    }

    private void HandleMovement()
    {
        float movementSpeed = 10f;
        float horizontalDirection = Input.GetAxis("Horizontal");
        if (horizontalDirection < 0)
        {
            rigidBody.velocity = new Vector2(-movementSpeed, rigidBody.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            if (horizontalDirection > 0)
            {
                rigidBody.velocity = new Vector2(movementSpeed, rigidBody.velocity.y);
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                //rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
    }

    private void HandleJump()
    {
        if (isGrounded())
        {
            canDoubleJump = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded())
            {
                float jumpSpeed = 17f;
                rigidBody.velocity = Vector2.up * jumpSpeed;
            }
            else
            {
                if (canDoubleJump)
                {
                    float jumpSpeed = 17f;
                    rigidBody.velocity = Vector2.up * jumpSpeed;
                    canDoubleJump = false;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "NPCs")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
    }
}
