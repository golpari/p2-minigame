using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlashingColors : MonoBehaviour
{
    public List<Color> colors = new List<Color>(); // Add colors in the Unity Inspector
    public float changeInterval = 2.0f; // Interval for color change

    private int currentIndex = 0;

    private void Start()
    {
        if (colors.Count <= 0)
        {
            Debug.LogWarning("No colors provided for background color changer.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(ChangeColor());
    }
    private IEnumerator ChangeColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeInterval);

            // Increment the index to use the next color
            currentIndex++;

            // Wrap the index if it exceeds the bounds of the list
            if (currentIndex >= colors.Count)
                currentIndex = 0;

            Camera.main.backgroundColor = colors[currentIndex];
        }
    }
}

