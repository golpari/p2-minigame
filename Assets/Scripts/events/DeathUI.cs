using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    Subscription<DeathEvent> death_event_subscription;

    void Start()
    {
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathUpdated);
    }

    void _OnDeathUpdated(DeathEvent e)
    {
        GetComponent<Text>().text = "DEATHS: " + e.deathCount;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(death_event_subscription);
    }
}
