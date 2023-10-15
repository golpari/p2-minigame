using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ASSIGN THIS SCRIPT TO WHATEVER WILL KILL THE PLAYER
 */

public class KillPlayer : MonoBehaviour
{
    static int totalDeaths = 0;
    bool isDead = false;

    Subscription<RespawnEvent> respawn_event_subscription;

    void Start()
    {
        respawn_event_subscription = EventBus.Subscribe<RespawnEvent>(_OnRespawnUpdated);
    }

    void _OnRespawnUpdated(RespawnEvent e)
    {
        if (!isDead) { return; }
        isDead = e.isDead;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(respawn_event_subscription);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike") && !isDead)
        {
            totalDeaths++;
            isDead = true;
            EventBus.Publish<DeathEvent>(new DeathEvent(totalDeaths));
        }
            
    }   
}

public class DeathEvent
{
    public int deathCount = 0;
    public DeathEvent(int deaths) { deathCount = deaths; }

    public override string ToString()
    {
        return "DEATHS: " + deathCount;
    }
}
