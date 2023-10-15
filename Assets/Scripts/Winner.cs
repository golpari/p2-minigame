using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public AudioClip winAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Play sound effect at the location of the main camera
            if (winAudio != null)
                AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);
        }
    }
}
