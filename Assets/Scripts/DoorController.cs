using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] public bool locked = true;
    [SerializeField] private bool openInside = true;
    [SerializeField] private float _interactRange = 1.0f;
    public float interactRange { get {return _interactRange;} private set {_interactRange = value;} }
    private bool opened = false;
    private Transform door;
    private float closedY;
    private String _roomName;
    public String roomName {get {return _roomName;} private set {_roomName = value;}}
    private Animator animator;

    void Awake() {

        door = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        roomName = transform.parent.parent.name;
        closedY = (float) Math.Round(door.gameObject.transform.eulerAngles.y);

    }

    void Start() {

    }

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

    public float GetInteractRange() {
        return interactRange;
    }

    private float GetOpenY() {
        // Berechnet Rotation-Y am Ende der Tueroffnung
        return (float) Math.Round(closedY + (openInside ? 90.0f : -90.0f));
    }
}