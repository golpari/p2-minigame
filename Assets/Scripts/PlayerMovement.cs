using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5;
    public float jumpPower = 10;

    public static PlayerMovement instance;

    public bool controlsFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 newVelocity = rb.velocity;

        //Horizontal
        newVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        //Vertical (jumping)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            newVelocity.y = jumpPower;
        }

        rb.velocity = newVelocity;
    }

    bool IsGrounded()
    {
        //use any available collider in the children (this will be different for Standing)
        Collider2D col = this.GetComponentInChildren<Collider2D>();

        if (col == null)
            return false;

        // Ray (actually just a direction) from the center of the collider down
        Vector2 direction = Vector2.down;

        // A bit smaller than the actual radius so it doesn't catch on walls
        float radius = col.bounds.extents.x - .05f;

        // A bit below the bottom
        float distance = col.bounds.extents.y + .05f;

        // Perform a CircleCast
        RaycastHit2D hit = Physics2D.CircleCast(col.bounds.center, radius, direction, distance);

        // If it hits something then return true
        if (hit.collider != null)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) // For instance, press 'T' to start transition to the right
        {
            CameraController.instance.move = true;
        }
    }
}
