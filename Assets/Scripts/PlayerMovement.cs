using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementStatus
    {
        standing,
        walking,
        jumping
    }

    public float pulseDuration = 1f;  // Duration for one full pulse (shrink and unshrink)
    public float shrinkFactor = 0.02f;   // How much you want to shrink the player's height
    private Vector3 originalScale;
    private Vector3 shrunkenScale;
    private float timer = 0.0f;


    private Rigidbody2D rb;
    private bool justJumped = false;

    public float moveSpeed = 5;
    public float jumpPower = 10;

    public MovementStatus status = MovementStatus.standing;

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

    private void Start()
    {
        originalScale = transform.localScale;
        shrunkenScale = new Vector3(originalScale.x, originalScale.y * shrinkFactor, originalScale.z);
        StartCoroutine(PlayerAnimations());
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

        //FOR ANIMATIONS
        // if stickman is just standing
        if (rb.velocity == Vector2.zero)
        {
            status = MovementStatus.standing;
        }
        else if (rb.velocity.y != 0)
        {
            status = MovementStatus.jumping;
        }
        else
        {
            status = MovementStatus.walking;
        }
    }

    private void Update()
    {
        // Vertical (jumping)
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !justJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x / 2, jumpPower);
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

    IEnumerator PlayerAnimations()
    {
        while (true)
        {
            if (status == MovementStatus.standing)
            {
                timer += Time.deltaTime;

                if (timer >= pulseDuration)
                {
                    // Toggle between the two sizes
                    if (transform.localScale == originalScale)
                    {
                        SetScale(shrunkenScale);
                    }
                    else
                    {
                        SetScale(originalScale);
                    }
                    timer = 0.0f; // Reset the timer
                }
            }
            else
            {
                transform.localScale = originalScale; // Reset to original scale if not standing
                yield return null;
            }
        }
    }

    private void SetScale(Vector3 newScale)
    {
        float deltaHeight = (originalScale.y - newScale.y) / 2;
        transform.position += Vector3.up * deltaHeight;
        transform.localScale = newScale;
    }
}
