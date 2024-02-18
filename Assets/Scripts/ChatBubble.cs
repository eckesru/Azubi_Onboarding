using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class ChatBubble : MonoBehaviour
{

    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;
    private string text;
    private float timer = 0f;
    private float timePerCharacter = 0.15f;
    private int characterIndex = 0;
    private bool writeText = false;
    private Animator animator;

    // Hilfsfunktion zur automatischen Generierung von ChatBubbles
    // Optionale Argument enthalten Standardwerte, um die ChatBubble direkt vor einem stehenden NPC erscheinen zu lassen
    public static Transform CreateChatBubble(GameObject gameObject, string text, bool sitting = false, float distance = 0.5f,  float rotation = 180) {

        // Instanzierung des GameObjects als Child des GameObjects, basierend auf dem ChatBubble-Prefab
        Transform chatBubbleTransform = Instantiate(GameAssets.Instance.chatBubble, gameObject.transform);

        // Auf Basis des MeshRenderers wird die Groesse des GameObjects bestimmt und die ChatBubble entsprechend platziert
        SkinnedMeshRenderer renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        float heightOffset; // Da eine Animation nicht den MeshRenderer veraendert, muss die Hoehe der ChatBubble im Falle einer sitzenden Animation anpasst werden
        heightOffset = sitting ? 1.0f : 0.7f;
        Vector3 headPosition = new Vector3(renderer.bounds.center.x, renderer.bounds.max.y - heightOffset, renderer.bounds.center.z);
        chatBubbleTransform.position = headPosition;

        // Verschiebung aller Childs um einen Offset, so dass die Chatbubble leicht rechts neben dem Kopf erscheint, unabhaengig von der Rotation des GameObjects
        Transform[] children = chatBubbleTransform.GetComponentsInChildren<Transform>();
        foreach(Transform child in children) {
            child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, child.localPosition.z + distance);
        }

        // Drehung der ChatBubble um 180°, da sie standardmaessig verkehrt herum generiert wird
        chatBubbleTransform.rotation = Quaternion.Euler(chatBubbleTransform.eulerAngles.x ,chatBubbleTransform.eulerAngles.y - rotation, chatBubbleTransform.eulerAngles.z);

        // Ausfuehren der Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);

        return chatBubbleTransform;

    }

    void Awake() {

        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();

        animator = GetComponent<Animator>();
    }

    void Start()
    {


    }

    void Update()
    {


        WriteText();
    }

        // Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
    private void Setup (string text) {

        //textMeshPro.SetText(text);
        this.text = text;

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
                textMeshPro.text = text.Substring(0, characterIndex);

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
        yield return new WaitForSeconds(3);
        animator.Play("ChatBubbleExit");
        Destroy(transform.gameObject, 1f);
    }

}
