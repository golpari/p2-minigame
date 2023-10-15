using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    Subscription<DeathEvent> death_event_subscription;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
            return;
        }

        //RegisterEvent
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // DO THE BLOODSPLATTER WHEN THE DEATH EVENT IS TRIGGERED
    void _OnDeathEvent(DeathEvent e)
    {
        StartCoroutine(BloodSplatter());
    }

    public IEnumerator BloodSplatter()
    {
       // while (true)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] != null)
                {
                    spriteRenderer.sprite = sprites[i];
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    Debug.LogWarning($"Sprite '{sprites[i]}' not found in the sprite sheet 'dropsplash' in the Resources folder.");
                }
            }

            spriteRenderer.sprite = null;
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(death_event_subscription);
    }
}
