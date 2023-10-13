using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // Movement speed
    public float jumpForce = 5f;       // Jumping force

    private bool isJumping = false;    // To check if player is already jumping
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
        if (!controlsFrozen)
        {
            // Left or right movement
            float moveX = Input.GetAxisRaw("Horizontal"); // -1 for left, 1 for right, 0 for idle
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    // Detecting ground using OnCollisionEnter2D and OnCollisionExit2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        else
            isJumping = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
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
