using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform preRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActiveRoom(true);
                preRoom.GetComponent<Room>().ActiveRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(preRoom);
                preRoom.GetComponent<Room>().ActiveRoom(true);
                nextRoom.GetComponent<Room>().ActiveRoom(false);
            }
        }
    }
}
