using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * ASSIGN THIS SCRIPT TO THE CAMERA.
 * WHEN A ROOM CHANGE EVENT IS TRIGGERED
 * THE CAMERA WILL PAN AND THE PLAYER WILL BE TELEPORTED
 */

public class CameraController : MonoBehaviour
{
    public AudioClip roomchangeAudio;
    public GameObject player; // Assign your player game object in the inspector
    public float fadeDuration = 1f;
    public float waitDuration = 1f;
    public float moveDistance = 11f; // Distance to move the camera

    private float alphaFadeValue = 0;
    
    Subscription<NextRoomEvent> nextRoom_event_subscription;

    void Start()
    {
        nextRoom_event_subscription = EventBus.Subscribe<NextRoomEvent>(_OnRoomEvent);
    }

    private void Update()
    {
     
            
    }

    void _OnRoomEvent(NextRoomEvent e)
    {
        StartCoroutine(CameraTransitionCoroutine("right"));
        // Play sound effect at the location of the main camera
        if (roomchangeAudio != null)
            AudioSource.PlayClipAtPoint(roomchangeAudio, Camera.main.transform.position);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(nextRoom_event_subscription);
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

        Camera.main.backgroundColor = Color.white; //reset bg color

        //move player over
        player.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y, player.transform.position.z);

        // Fade back in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            alphaFadeValue = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

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
