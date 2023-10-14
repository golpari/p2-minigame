using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Assign your player game object in the inspector
    public float fadeDuration = 1f;
    public float waitDuration = 1f;
    public float moveDistance = 5f; // Distance to move the camera

    [HideInInspector] public bool move = false;
    public static CameraController instance;

    private float alphaFadeValue = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (PlayerMovement.instance.controlsFrozen)
            return;

        /*// Test trigger (use any condition you want to start the transition)
        if (Input.GetKeyDown(KeyCode.T)) // For instance, press 'T' to start transition to the right
        {
            StartCoroutine(CameraTransitionCoroutine("right"));
        }
        if (Input.GetKeyDown(KeyCode.Y)) // Press 'Y' to start transition to the left
        {
            StartCoroutine(CameraTransitionCoroutine("left"));
        }*/

        if (move)
            StartCoroutine(CameraTransitionCoroutine("right"));
    }

    public IEnumerator CameraTransitionCoroutine(string direction)
    {
        // Freeze player controls
        PlayerMovement.instance.controlsFrozen = true;

        // Fade to black
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            alphaFadeValue = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        // Wait for a while
        yield return new WaitForSeconds(waitDuration);

        // Move the camera
        if (direction == "left")
            transform.position -= new Vector3(moveDistance, 0, 0);
        else if (direction == "right")
            transform.position += new Vector3(moveDistance, 0, 0);

        //move player over
        player.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y, player.transform.position.z);

        // Fade back in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            alphaFadeValue = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        move = false;
        moveDistance = 11;

        // Unfreeze player controls
        PlayerMovement.instance.controlsFrozen = false;
    }

    private void OnGUI()
    {
        // Draw full screen black rectangle with changing alpha value
        GUI.color = new Color(0, 0, 0, alphaFadeValue);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }
}

