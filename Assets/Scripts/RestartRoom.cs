using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * ASSIGN THIS SCRIPT TO A ROOM, AND SET THE ROOMS RESPAWN LOCATION
 */

public class RestartRoom : MonoBehaviour
{
    Subscription<DeathEvent> death_event_subscription;

    public float fadeDuration = 1f;
    public float waitDuration = 1f;

    public GameObject player;
    public Vector3 respawnLocation;

    private Camera mainCamera;
    private float alphaFadeValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathUpdated);

        mainCamera = Camera.main;  // Get the main camera (assuming this script is used in a 2D game)
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    void _OnDeathUpdated(DeathEvent e)
    {
        //move player back to given position when it has died and play the black screen
        StartCoroutine(CameraFadeAndPlayerResetCoroutine());   
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(death_event_subscription);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void OnTriggerStay2D(Collider2D other)
    {
        // if hit restart arrow
        if (other.gameObject.CompareTag("Player"))
        {

            PlayerMovement.instance.controlsFrozen = true;
            mainCamera.backgroundColor = Color.white;
            CameraController.instance.moveDistance = -22;
            CameraController.instance.move = true;

            player.transform.position = new Vector3(-4, 0, 0);
            PlayerMovement.instance.controlsFrozen = false;
        }
        
    }*/
    public IEnumerator CameraFadeAndPlayerResetCoroutine()
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

        //move player back to starting spot
        player.transform.position = respawnLocation;

        // Fade back in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            alphaFadeValue = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        //wait just half a second so player doesnt start immediately moving
        yield return new WaitForSeconds(0.5f);

        // Unfreeze player controls
        PlayerMovement.instance.controlsFrozen = false;

        // tell everyone that the player has respawned only after it has been moved from the spikes
        EventBus.Publish<RespawnEvent>(new RespawnEvent(false));
    }

    private void OnGUI()
    {
        // Draw full screen black rectangle with changing alpha value
        GUI.color = new Color(0, 0, 0, alphaFadeValue);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
    }

}

public class RespawnEvent
{
    public bool isDead = false;
    public RespawnEvent(bool _isDead) { isDead = _isDead; }
}
