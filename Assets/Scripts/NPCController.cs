using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{

    [SerializeField] private float _interactRange = 4.0f;
    public float interactRange {get {return _interactRange;} private set{_interactRange = value;}}

    private string[] textLines;
    
    private int index;

    private bool active = false;

    private Transform chatBubble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupNPC(string[] textLines) {
        this.textLines = textLines;
        index = 0;
        active = true;

    }

    public void Interact() {

        if (active && chatBubble == null && index < textLines.Length) {
                chatBubble = ChatBubble.CreateChatBubble(this.gameObject, textLines[index]);
                index++;
        }
    }   

    private void OnTriggerEnter(Collider collider) {
        if (index == textLines.Length) {
            index = textLines.Length - 1;
        }
    }
}
