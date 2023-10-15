using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BackgroundColorChanger : MonoBehaviour
{
    public AudioClip paintAudio;
    public Color newBackgroundColor = Color.blue;  // Default color is blue, but you can change this in the Inspector

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;  // Get the main camera (assuming this script is used in a 2D game)
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }

        // Ensure this object's collider is set as a trigger
        if (!GetComponent<Collider2D>().isTrigger)
        {
            Debug.LogWarning("Collider is not set as a trigger. Setting it to trigger now.");
            GetComponent<Collider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mainCamera != null)
        {
            mainCamera.backgroundColor = newBackgroundColor;
            // Play sound effect at the location of the main camera
            if (paintAudio != null)
                AudioSource.PlayClipAtPoint(paintAudio, Camera.main.transform.position);
        }
    }
}

