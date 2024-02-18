using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{

    [SerializeField] private float _interactRange = 4.0f;
    public float interactRange {get {return _interactRange;} private set {_interactRange = value;}}
    [SerializeField] private string _npcName;
    public string npcName {get {return _npcName;} private set {_npcName = value;}}
    [SerializeField] private string animationName;
    [SerializeField] private bool sitting = false;
    [SerializeField] private float distanceChatBubble = 0.5f;
    [SerializeField] private float rotationChatBubble = 180f;
    [SerializeField] private Transform head;
    private PlayerController player;

    private char gender;

    Animator animator;

    private string[] textLines;
    
    private int index;

    private bool active = false;

    private Transform chatBubble;

    private Vector3 lastHeadPosition;

    private bool interact = false;

    void Awake() {

        animator = GetComponent<Animator>();

        GetGender();

        player = FindObjectOfType<PlayerController>();

    }

    void Start()
    {
        animator.Play(animationName);
    }

    void Update()
    {
        interact = false;
    }

    void LateUpdate() {
        if (interact) MoveHead();
    }

    public void SetupNPC(string[] textLines) {
        this.textLines = textLines;
        index = 0;
        active = true;

    }

    public void Interact() {

        if (active && index < textLines.Length && chatBubble == null) {
                chatBubble = ChatBubble.CreateChatBubble(gameObject, textLines[index], sitting, distanceChatBubble, rotationChatBubble, gender);
                index++;
        }

        interact = true;
    }   

    private void OnTriggerEnter(Collider collider) {
        if (active && index == textLines.Length) {
            index = textLines.Length - 1;
        }
    }

    private void GetGender() {

        Transform[] childs = GetComponentsInChildren<Transform>();

        foreach(Transform child in childs) {
            if(child.name.Contains("Male", System.StringComparison.Ordinal)) {
                gender = 'm';
            } else if (child.name.Contains("Female", System.StringComparison.Ordinal)) {
                gender = 'w';
            }
        }

    }

    private void MoveHead() {
        
    // Begrenzt die maximale Drehung des Kopfs
    float maxRotationAngle = 30.0f;

    // Berechnet den Winkel zwischen der aktuellen Blickrichtung und der Zielrichtung
    Vector3 targetDir = player.cameraView.position - head.position;
    float angleToTarget = Vector3.Angle(head.forward, targetDir);

    // Wenn der Winkel nicht ueberschritten wurde, passe Blickrichtung an
    // Wenn der Winkel ueberschritten wurde, passe an letzte erlaubte Blickrichtung an
    if (!(angleToTarget > maxRotationAngle)) {
        head.LookAt(player.cameraView.position);
        lastHeadPosition = player.cameraView.position;
    } else {
        head.LookAt(lastHeadPosition);
    }

    }

}
