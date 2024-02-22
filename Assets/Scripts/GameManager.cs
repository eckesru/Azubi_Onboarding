using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private FadeController fadeController;

    private NPCController[] npcs;
    private DoorController[] doors;
    private ItemController[] items;

    private int points = 0;
    private float itemAmount = 0;
    private int keyItemsCollected = 0;

    public bool gameRunning = false;
    public bool gamePaused = false;

    private Dictionary<string, string[]> npcDialogues = new Dictionary<string, string[]>();

    void Awake() {

        npcs = FindObjectsOfType<NPCController>();
        doors = FindObjectsOfType<DoorController>();
        items = FindObjectsOfType<ItemController>(true);

        itemAmount = CountItems();

    }

    void Start()
    {

        Time.timeScale = 0.0f;
        AudioListener.pause = true;

        menuPanel.SetActive(true);
        pausePanel.SetActive(false);
        fadePanel.SetActive(true);

        UnlockDoors("Flur 1");
        UnlockDoors("Empfang");

       // ActivateNPC("Test NPC", GetDialogue("Test"));
    }

    void Update()
    {
        
        HandlePauseMode();

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

        AddDialogue("GameStart", "Herzlich Willkommen bei der Fleet GmbH" + "\n\n" + "Wir wünschen dir viel Spaß bei dem Azubi-Onbarding!");
        AddDialogue("Test", "Test1  ...", "Test2  .......");

    }

    private void GameEnding() {
        
        AddDialogue("GameFinish", "Herzlichen Glückwunsch!" + "\n" + "Du hast das Azubi-Onboarding erfolgreich abgeschlossen" + "\n\n" + "Gegenstände: " + GetFinalPercentage() + "% gefunden");

        fadeController.FadeIn(GetDialogue("GameFinish"), 10);
        
    }

    public void OnStartButtonClick()
    {
        // Zentriert den Cursor in der Mitte und blendet ihn aus
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        fadePanel.SetActive(true);
        menuPanel.SetActive(false);

        InitializeGameScript();

        StartCoroutine(HandleFadeOut(5));

    }

    private IEnumerator HandleFadeOut(int sleepTime) {

        fadeController.FadeOut(GetDialogue("GameStart"), sleepTime);

        yield return new WaitForSecondsRealtime(sleepTime + fadeController.fadeDuration);
        
        gameRunning = true;

    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnMenuButtonClick()
    {
        // Startet die Szene neu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnContinueButtonClick()
    {
        AudioListener.pause = false;
        fadeController.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        gamePaused = false;
        gameRunning = true;
        Time.timeScale = 1.0f;
        fadePanel.SetActive(true);
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandlePauseMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused && gameRunning)
            {
                AudioListener.pause = true;
                fadeController.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
                gamePaused = true;
                gameRunning = false;
                Time.timeScale = 0.0f;
                fadePanel.SetActive(false);
                pausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
            else if (!gameRunning && gamePaused)
            {
                OnContinueButtonClick();
            }
        }
    }

    private float CountItems() {

        int itemAmount= 0;
        foreach (ItemController item in items) {
            itemAmount++;
        }

        return itemAmount;

    }

    private double GetFinalPercentage() {
        return Math.Round(points / itemAmount * 100, 0);
    }
}
