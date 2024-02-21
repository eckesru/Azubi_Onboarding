using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private FadeController fadeController;

    private NPCController[] npcs;
    private DoorController[] doors;

    private int points = 0;
    private int keyItemsCollected = 0;

    public bool gameRunning = false;
    public bool gamePaused = false;

    private Dictionary<string, string[]> npcDialogues = new Dictionary<string, string[]>();

    void Awake() {

    }

    void Start()
    {
        InitializeGameScript();

        fadeController.FadeOut(GetDialogue("GameStart"), 5);

        // Zentriert den Cursor in der Mitte und blendet ihn aus
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Startet das Spiel
        //Time.timeScale = 1.0f;
        //gameRunning = true;

        npcs = FindObjectsOfType<NPCController>();
        doors = FindObjectsOfType<DoorController>();

        UnlockDoors("Flur 1");
        UnlockDoors("Empfang");

        ActivateNPC("Test NPC", GetDialogue("Test"));
    }

    void Update()
    {

    }

    private void ActivateNPC(string npcName, string[] textLines) {

        // Durchlaeuft alle NPC und aktiviert sie, wenn der Name uebereinstimmt
        foreach (NPCController npc in npcs) {
            if (npc.npcName.Equals(npcName)) {
                npc.SetupNPC(textLines);
            }
        }

    }

    private void UnlockDoors(string roomName) {

        // Durchlaeuft alle Tueren und oeffnet sie, wenn der Raumname uebereinstimmt
        foreach (DoorController door in doors) {
            if (door.GetRoomName().Equals(roomName)) {
                door.locked = false;
            }
        }
    }

    public void AddPoint(bool keyItem) {
        points++;

        if (keyItem) {
            keyItemsCollected += 1;
        }

    }

    void OnEnable() {
        // Bei Ausloesen des Events, rufe die GameLoop-Methode auf (Abonnieren)
        ItemController.OnKeyItemCollected += GameLoop;
    }

    void OnDisable() {  
        // Bei Zerstoerung des GameObjects mit dem Event, rufe nicht mehr die GameLoop-Methode auf (Abbestellen)
        ItemController.OnKeyItemCollected -= GameLoop;
    }

    private void GameLoop() {



    }

    // Hilfsmethode zum Hinzufuegen von Dialogen
    // Mithilfe des params-Modifizierers muessen lediglich String angegeben werden, welche dann automatisch in ein Array verpackt werden
    private void AddDialogue(string dialogueName, params string[] dialogues)
    {
        npcDialogues[dialogueName] = dialogues;
    }

    private string[] GetDialogue(string dialogueName) {
            return npcDialogues[dialogueName];
    }


    private void InitializeGameScript() {

        AddDialogue("GameStart", "Herzlich Willkommen bei der Fleet GmbH" + "\n\n" + "Wir wünschen dir viel Spaß beim Azubi-Onbarding-Programm!");
        AddDialogue("Test", "Test1  ...", "Test2  .......");

    }

}
