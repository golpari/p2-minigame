using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public enum MovementDirection
    {
        None,
        Horizontal,
        Vertical
    }

    [Header("Movement Settings")]
    public MovementDirection direction = MovementDirection.None;
    public float speed = 1.0f;
    public float distance = 10.0f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        switch (direction)
        {
            case MovementDirection.Horizontal:
                transform.position = startPosition + new Vector3(Mathf.Sin(Time.time * speed) * distance, 0, 0);
                break;
            case MovementDirection.Vertical:
                transform.position = startPosition + new Vector3(0, Mathf.Sin(Time.time * speed) * distance, 0);
                break;
            case MovementDirection.None:
            default:
                // No movement
                break;
        }
    }


    // TO MAKE THE PLAYER MOVE WHEN IT JUMPS ON A PLATFORM THAT OSCILLATES
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

