using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public GameObject player;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera (assuming this script is used in a 2D game)
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement.instance.controlsFrozen = true;
            mainCamera.backgroundColor = Color.white;
            CameraController.instance.moveDistance = -22;
            CameraController.instance.move = true;

            player.transform.position = new Vector3(-4, 0, 0);
            PlayerMovement.instance.controlsFrozen = false;
        }
        
    }
}
