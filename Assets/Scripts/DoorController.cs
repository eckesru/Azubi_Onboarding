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
    [SerializeField] private bool sharedInteractTrigger = false;
    [SerializeField] private bool openOnlyOnce = false;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closedClip;
    public float interactRange { get {return _interactRange;} private set {_interactRange = value;} }
    private bool _opened = false;
    public bool opened { get {return _opened;} private set {_opened = value;} }
    private bool closedClipTrigger = false;
    private bool finalLock = false;
    private Transform door;
    private float closedY;
    private string _roomName;
    public string roomName {get {return _roomName;} private set {_roomName = value;}}
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

        if(closedClipTrigger) {
            AudioSource.PlayClipAtPoint(closedClip, transform.position);
            closedClipTrigger = false;
        }

    }

    private void OpenDoor() {

        // Spiel dynamisch die entsprechende Tuer-Animation anhand closedY und openY ab
        animator.Play("DoorOpen_" + closedY + "_" + GetOpenY());
        
        AudioSource.PlayClipAtPoint(openClip, transform.position);

        opened = true;

        OpenSharedDoors();

    }

    public void Interact() {

        if (!opened || finalLock) {
            if (locked) {
                DoorIsLocked();
            } else {
                OpenDoor();
            }
        }

    }

    public string GetRoomName() {
        return roomName;
    }

    public float GetInteractRange() {
        return interactRange;
    }

    private float GetOpenY() {
        // Berechnet Rotation-Y am Ende der Tueroffnung
        return (float) Math.Round(closedY + (openInside ? 90.0f : -90.0f));
    }

    private void OnTriggerEnter(Collider collider) {
        closedClipTrigger = true;
    }

    private void OpenSharedDoors() {

        if (sharedInteractTrigger) {
            GameObject parent = transform.parent.gameObject;
            DoorController[] doors = parent.GetComponentsInChildren<DoorController>();

            foreach(DoorController door in doors) {
            door.Interact();
            }
        }

    }

    public void CloseDoor() {

        animator.Play("DoorOpen_" + GetOpenY() + "_" + closedY);

        AudioSource.PlayClipAtPoint(closedClip, transform.position);

        if(openOnlyOnce) {
            locked = true;
            finalLock = true;
        } else {
            opened = false;
        }
    }
}