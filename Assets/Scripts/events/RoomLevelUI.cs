using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomLevelUI : MonoBehaviour
{
    Subscription<NextRoomEvent> nextRoom_event_subscription;

    void Start()
    {
        nextRoom_event_subscription = EventBus.Subscribe<NextRoomEvent>(_OnRoomUpdated);
    }

    void _OnRoomUpdated(NextRoomEvent e)
    {
        GetComponent<Text>().text = "LEVEL: " + e.roomNumber;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(nextRoom_event_subscription);
    }
}
