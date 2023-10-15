using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ASSIGN THIS SCRIPT TO THE ARROW THAT WILL TRIGGER THE MOVE TO THE NEXT ROOM
 */

public class NextRoom : MonoBehaviour
{
    static int currentRoom = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentRoom++;
            EventBus.Publish<NextRoomEvent>(new NextRoomEvent(currentRoom));
        }

    }
}

public class NextRoomEvent
{
    public int roomNumber = 1;
    public NextRoomEvent(int _roomNumber) { roomNumber = _roomNumber; }
}
