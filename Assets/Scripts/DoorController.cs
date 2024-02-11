using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] public bool locked = true;
    [SerializeField] private bool openInside = true;
    private bool opened = false;
    private Transform door;
    private float closedY;
    private String roomName;
    private Animator animator;

    void Awake() {

        door = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        roomName = transform.parent.parent.name;
        closedY = (float) Math.Round(door.gameObject.transform.eulerAngles.y);

    }

    void Start() {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DoorIsLocked() {

    // TODO: LockedSound

    }

    private void OpenDoor() {

        // Spiel dynamisch die entsprechende Tuer-Animation anhand closedY und openY ab
        animator.Play("DoorOpen_" + closedY + "_" + GetOpenY());
        
        // TODO: OpenedSound

        opened = true;

    }

    public void Interact() {

        if (!opened) {
            if (locked) {
                DoorIsLocked();
            } else {
                OpenDoor();
            }
        }

    }

    public String GetRoomName() {
        return roomName;
    }

    private float GetOpenY() {
        // Berechnet Rotation-Y am Ende der Tueroffnung
        return (float) Math.Round(closedY + (openInside ? 90.0f : -90.0f));
    }
}