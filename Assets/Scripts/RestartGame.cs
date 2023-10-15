using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float waitDuration = 1f;
    public float moveDistance = 11f; // Distance to move the camera

    private float alphaFadeValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RestartGameCameraFade());
        }
    }

    public IEnumerator RestartGameCameraFade()
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

        // Restart game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
