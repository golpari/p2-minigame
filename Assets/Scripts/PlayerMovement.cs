using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool justJumped = false;

    public float moveSpeed = 5;
    public float jumpPower = 10;

    [Header("Ground Check Settings")]
    [SerializeField] private LayerMask groundLayerMask;

    public static PlayerMovement instance;
    public bool controlsFrozen = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (instance == null)
        {
            instance = this;
        }
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = rb.velocity;

        // Horizontal
        if (IsGrounded())
            newVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
        else
            newVelocity.x = Input.GetAxis("Horizontal") * moveSpeed / 2;

        rb.velocity = newVelocity;

        // Reset the justJumped flag if grounded
        if (IsGrounded())
        {
            justJumped = false;
        }
    }

    private void Update()
    {
        // Vertical (jumping)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !justJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x/2, jumpPower);
            justJumped = true;
        }
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
        RaycastHit2D hit = Physics2D.CircleCast(col.bounds.center, radius, direction, distance, groundLayerMask);

        // Visualization: Draw the CircleCast as a line with circles at the start and end
        Debug.DrawLine(col.bounds.center, col.bounds.center + (Vector3)direction * distance, Color.red);
        DebugDrawCircle(col.bounds.center, radius, Color.red);
        DebugDrawCircle(col.bounds.center + (Vector3)direction * distance, radius, Color.red);

        // If it hits something then return true
        if (hit.collider != null)
            return true;

        return false;
    }

    // Helper function to draw a circle using Debug.DrawLine()
    void DebugDrawCircle(Vector3 position, float radius, Color color)
    {
        int segments = 20; // Adjust as needed for more or less detail
        Vector3 prevPos = position + new Vector3(radius, 0, 0);
        for (int i = 0; i < segments + 1; i++)
        {
            float angle = (float)i / (float)segments * 360 * Mathf.Deg2Rad;
            Vector3 newPos = position + new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
            Debug.DrawLine(prevPos, newPos, color);
            prevPos = newPos;
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow")) // For instance, press 'T' to start transition to the right
        {
            CameraController.instance.move = true;
        }
    }*/
}
