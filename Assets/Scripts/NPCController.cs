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
    [SerializeField] bool lookAtPlayer = false;
    [SerializeField] private Transform head;
    private bool keyDialogue = false;
    private bool keyDialogueTriggered;
    private PlayerController player;
    private GameManager gameManager;
    private char gender;
    Animator animator;
    private string[] textLines;
    private int index;
    private bool active = false;
    private Transform chatBubble;
    private bool interact = false;

    public delegate void KeyDialogueFinished();
    public static event KeyDialogueFinished OnKeyDialogueFinished;

    void Awake() {

        animator = GetComponent<Animator>();

        GetGender();

        player = FindObjectOfType<PlayerController>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    void Start()
    {
        animator.Play(animationName);
    }

    void LateUpdate() {
        if (interact && lookAtPlayer) MoveHead();

        interact = false;
    }

    public void SetupNPC(string[] textLines, bool keyDialogue) {
        this.textLines = textLines;
        this.keyDialogue = keyDialogue;
        keyDialogueTriggered = false;
        index = 0;
        active = true;

    }

    public void Interact() {

        if (active) {

            // Stoesst die naechste ChatBubble an, wenn keine mehr vorhanden ist
            if (index < textLines.Length && chatBubble == null) {
                chatBubble = ChatBubble.CreateChatBubble(gameObject, textLines[index], sitting, distanceChatBubble, rotationChatBubble, gender);
                index++;
            }

            // Trigger fuer den naechsten keyPoint, wenn der letzte Dialog erreicht ist
            if(index == textLines.Length && keyDialogue && !keyDialogueTriggered) {
                keyDialogueTriggered = true;

                gameManager.AddPoint(0, keyDialogue);
            
                OnKeyDialogueFinished();
            }

            interact = true;

        }
    }   

    private void OnTriggerEnter(Collider collider) {
        if (active && index == textLines.Length) {
            index = textLines.Length - 1;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if(chatBubble != null && index != 0) {
            index--;
            Destroy(chatBubble.gameObject);
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
        float maxRotationAngle = 60.0f;

        // Berechnet den Winkel zwischen der aktuellen Blickrichtung und der Zielrichtung
        Vector3 targetDir = player.cameraView.position - head.position;
        float angleToTarget = Vector3.Angle(targetDir, head.forward);

        // Wenn der Winkel nicht ueberschritten wurde, passe Blickrichtung an
        if (angleToTarget <= maxRotationAngle) {
            head.LookAt(player.cameraView.position);
        }
    }
}
