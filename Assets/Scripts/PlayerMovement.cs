using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // Movement speed
    public float jumpForce = 5f;       // Jumping force

    private bool canJumpUp = false;    // To check if player is already jumping
    private bool canWallJump = false;
    private bool canWalk = false;
    [HideInInspector] public bool controlsFrozen = false;
    public static PlayerMovement instance;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!controlsFrozen) {

            // Left or right movement
            float moveX = Input.GetAxisRaw("Horizontal"); // -1 for left, 1 for right, 0 for idle
            if (canWalk)
            {
                rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
            }

            // if user presses space and you can jump up, then jump up. doesnt matter if can walljump as this takes precedent
            if (Input.GetKeyDown(KeyCode.Space) && canJumpUp)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                canJumpUp = false;

            }
           
        }
    }

    // Detecting ground using OnCollisionEnter2D and OnCollisionExit2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if you touch the ground you can jump up again
        if (collision.gameObject.CompareTag("Ground"))
        {
            canWalk = true;
            canJumpUp = true;
            canWallJump = false;
        }
        //if you touch the wall you can wall jump again
        else if (collision.gameObject.CompareTag("Wall"))
        {
            canJumpUp = false;
            canWallJump = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if you stay on the ground you can jump up again
        if (collision.gameObject.CompareTag("Ground"))
        {
            canWalk = true;
            canJumpUp = true;
            canWallJump = false;
        }
        //if you stay on the wall you can wall jump again
        else if (collision.gameObject.CompareTag("Wall"))
        {
            canJumpUp = false;
            canWallJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // if you exit a collision with the ground then you are jumping VERTICALLY, and you cant jump again
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJumpUp = false;
        }
        //if you exit a collision with a WALL then you are jumping HORIZONTALLY, and you cant jump again
        else if (collision.gameObject.CompareTag("Wall"))
        {
            canWallJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) // For instance, press 'T' to start transition to the right
        {
            CameraController.instance.move = true;
        }
    }
}
