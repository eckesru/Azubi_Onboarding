using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChatBubble : MonoBehaviour
{

    private SpriteRenderer backgroundSpriteRenderer;
    private TextMeshPro textMeshPro;


    // Hilfsfunktion zur automatischen Generierung von ChatBubbles
    public static void CreateChatBubble(GameObject gameObject, bool sitting, String text) {

        // Instanzierung des GameObjects als Child des GameObjects, basierend auf dem ChatBubble-Prefab
        Transform chatBubbleTransform = Instantiate(GameAssets.Instance.chatBubble, gameObject.transform);

        // Auf Basis des MeshRenderers wird die Groesse des GameObjects bestimmt und die ChatBubble entsprechend platziert
        SkinnedMeshRenderer renderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        float heightOffset; // Da eine Animation nicht den MeshRenderer veraendert, muss die Hoehe der ChatBubble im Falle einer sitzenden Animation anpasst werden
        heightOffset = sitting ? 0.6f : 0.2f;
        Vector3 headPosition = new Vector3(renderer.bounds.center.x, renderer.bounds.max.y - heightOffset, renderer.bounds.center.z);
        chatBubbleTransform.position = headPosition;

        // Verschiebung aller Childs um einen Offset, so dass die Chatbubble leicht rechts neben dem Kopf erscheint, unabhaengig von der Rotation des GameObjects
        Transform[] children = chatBubbleTransform.GetComponentsInChildren<Transform>();
        foreach(Transform child in children) {
            child.localPosition = new Vector3(child.localPosition.x - 0.3f, child.localPosition.y, child.localPosition.z);
        }

        // Drehung der ChatBubble um 180°, da sie standardmaessig verkehrt herum generiert wird
        chatBubbleTransform.rotation = Quaternion.Euler(chatBubbleTransform.eulerAngles.x ,chatBubbleTransform.eulerAngles.y - 180, chatBubbleTransform.eulerAngles.z);

        // Ausfuehren der Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);

        // Automatische Zerstoerung der ChatBubble nach n Sekunden
        Destroy(chatBubbleTransform.gameObject, 6f);
    }

    void Awake() {
        backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }


        // Standardmethode zur Anpassung der Groeße und des Textes der ChatBubble
    private void Setup (String text) {

        textMeshPro.SetText(text);

        // Da die Textanzeige nicht zuverlaessig aktualisiert wird, muss dies erzwungen werden
        textMeshPro.ForceMeshUpdate();

        // Anpassung der Greoße des Hintergrunds an die Groeße des Texts + Padding
        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(4f, 1f);
        backgroundSpriteRenderer.size = textSize + padding;

        // Anpassung der Position des Hintergrunds + Offset, damit der Text mittig platziert ist
        Vector3 offset = new Vector3(-2f, 0f, 0f);
        backgroundSpriteRenderer.transform.localPosition = new Vector3(backgroundSpriteRenderer.size.x / 2f, 0f) + offset;
    }
}
