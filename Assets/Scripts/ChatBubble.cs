using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class ChatBubble : MonoBehaviour
{

    [SerializeField] private AudioClip blipSoundMale;
    [SerializeField] private AudioClip blipSoundFemale;

    private AudioClip blipSound;

    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;
    private string text;
    private char gender;
    private float timer = 0f;
    private float timePerCharacter;
    private float bubbleDieTime;
    private int characterIndex = 0;
    private bool writeText = false;
    private int chatBubbleBreakpoint = 35;
    private int lastWhiteSpaceCharacterPosition = 0;
    private bool whiteScreenFixed = false;
    private int whiteScreenFixedPosition;
    private Animator animator;


    // Hilfsfunktion zur automatischen Generierung von ChatBubbles
    // Optionale Argument enthalten Standardwerte, um die ChatBubble direkt vor einem stehenden NPC erscheinen zu lassen
    public static Transform CreateChatBubble(GameObject gameObject, string text, bool sitting, float distance,  float rotation, char gender) {

        // Instanzierung des GameObjects als Child des GameObjects, basierend auf dem ChatBubble-Prefab
        Transform chatBubbleTransform = Instantiate(GameAssets.Instance.chatBubble, gameObject.transform);

        // Auf Basis des MeshRenderers wird die Groesse des GameObjects bestimmt und die ChatBubble entsprechend platziert
        SkinnedMeshRenderer renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        float heightOffset; // Da eine Animation nicht den MeshRenderer veraendert, muss die Hoehe der ChatBubble im Falle einer sitzenden Animation anpasst werden
        heightOffset = sitting ? 0.8f : 0.5f;
        Vector3 headPosition = new Vector3(renderer.bounds.center.x, renderer.bounds.max.y - heightOffset, renderer.bounds.center.z);
        chatBubbleTransform.position = headPosition;

        // Verschiebung aller Childs um einen Offset, so dass die Chatbubble vor dem Koerper erscheint, unabhaengig von der Rotation des GameObjects
        Transform[] children = chatBubbleTransform.GetComponentsInChildren<Transform>();
        foreach(Transform child in children) {
            child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, child.localPosition.z + distance);
        }

        // Drehung der ChatBubble um 180°, da sie standardmaessig verkehrt herum generiert wird
        chatBubbleTransform.rotation = Quaternion.Euler(chatBubbleTransform.eulerAngles.x ,chatBubbleTransform.eulerAngles.y - rotation, chatBubbleTransform.eulerAngles.z);

        // Ausfuehren der Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text, gender);

        return chatBubbleTransform;

    }

    void Awake()
    {

        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        animator = GetComponent<Animator>();

    }

    void Start()
    {

        blipSound = gender.Equals('m') ? blipSoundMale : blipSoundFemale;

    }

    void Update()
    {

        SpeedUpChatBubble();
        WriteText();
    }

        // Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
    private void Setup (string text, char gender) {

        //textMeshPro.SetText(text);
        this.text = text;
        this.gender = gender;

        // Da die Textanzeige nicht zuverlaessig aktualisiert wird, muss dies erzwungen werden
        textMeshPro.ForceMeshUpdate();

        // Anpassung der Greoße des Hintergrunds an die Groeße des Texts + Padding
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(4f, 2f);
        backgroundSpriteRenderer.size = textSize + padding;

        writeText = true;

        // Anpassung der Position des Hintergrunds + Offset, damit der Text mittig platziert ist (Nicht erforderlich)
        // Vector3 offset = new Vector3(0f, 0f, 0f);
        // backgroundSpriteRenderer.transform.localPosition += offset;
    }

    private void WriteText() {
        if (writeText) {
            UpdateChatBubbleBackground();
            timer -= Time.deltaTime;
            while (timer <= 0f) {

                // Naechsten Buchstaben anzeigen
                timer += timePerCharacter;
                characterIndex++;

                string currentText = text.Substring(0, characterIndex);

                char currentChar = text.ToCharArray()[currentText.Length - 1];

                // Speichert die letzte bekannte Position eines Whitespaces
                if (Char.IsWhiteSpace(currentText[characterIndex - 1])) {
                    lastWhiteSpaceCharacterPosition = characterIndex;
                }

                // Fuegt einen Zeilenumbruch in die Textposition des letzten Whitespace ein, wenn die maximale Zeichenlaenge ueberschritten wurde
                if (characterIndex > chatBubbleBreakpoint && lastWhiteSpaceCharacterPosition > chatBubbleBreakpoint)  {
                    if (!whiteScreenFixed) {
                        whiteScreenFixedPosition = lastWhiteSpaceCharacterPosition;
                        whiteScreenFixed = true;
                    } else {
                        currentText = currentText.Insert(whiteScreenFixedPosition, "\n");
                    }
                    }

                textMeshPro.text = currentText;
                
                PlayCharacterSound(currentChar);

                if(characterIndex >= text.Length) {
                    // Sobald der komplette Text geschrieben wurde
                    writeText = false;
                    // Startet eine Coroutine, welche waehrend der Ausfuehrung ein oder mehrere return yields verarbeiten kann
                    StartCoroutine(DestroyChatBubble());
                }
            }
        }
    }

    // Anpassung der Greoße des Hintergrunds an die aktuelle Textgroeße zzgl. Padding
    private void UpdateChatBubbleBackground() {

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(4f, 2f);
        backgroundSpriteRenderer.size = textSize + padding;

    }

    // Wartet n Sekunden, und zerstoert dannach das ChatBubble-GameObject nach n Sekunden, damit die Animation in Ruhe abgespielt wird
    private IEnumerator DestroyChatBubble() {
        yield return new WaitForSeconds(bubbleDieTime);
        animator.Play("ChatBubbleExit");
        Destroy(transform.gameObject, 1f);
    }

    private void PlayCharacterSound(char nextCharacter) {

        if(!Char.IsWhiteSpace(nextCharacter))
        AudioSource.PlayClipAtPoint(blipSound, transform.position, 0.1f);
    }

    private void SpeedUpChatBubble() {

        // Beschleunigt die ChatBubble, wenn die linke Maustaste gedrueckt gehalten wird
        if(Input.GetKey(KeyCode.Mouse0)) {
            timePerCharacter = 0.05f;
            bubbleDieTime = 0.5f;
        } else {
            timePerCharacter = 0.1f;
            bubbleDieTime = 1f;
        }

    }

}
