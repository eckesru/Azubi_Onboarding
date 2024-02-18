using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CloseDoorTrigger : MonoBehaviour
{

    [SerializeField] private string roomName;

    void Awake() {


    }

    private void OnTriggerEnter(Collider collider) {

        DoorController[] doors = FindObjectsOfType<DoorController>();

        // Durchlaeuft alle Tueren und schliesst sie, wenn der Raumname uebereinstimmt
        foreach (DoorController door in doors) {
            if (door.GetRoomName().Equals(roomName)) {
                door.CloseDoor();
            }
        }

    }
}
