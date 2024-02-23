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
    private int keyPoints = 0;

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

        InitializeGameScript();
        ActivateNPC("Aussenbereich", GetDialogue("Test"));
        ActivateNPC("Empfang", GetDialogue("Test"));
        ActivateNPC("Poststelle", GetDialogue("Test"));
        ActivateNPC("Lagerraum", GetDialogue("Test"));
        ActivateNPC("Meetingraum 1", GetDialogue("Test"));
        ActivateNPC("Geschaeftsfuehrer", GetDialogue("Test"));
        ActivateNPC("Vertriebsabteilung", GetDialogue("Test"));
        ActivateNPC("Kueche", GetDialogue("Test"));
        ActivateNPC("IT-Abteilung", GetDialogue("Test"));
        ActivateNPC("Pausenraum", GetDialogue("Test"));
        ActivateNPC("Pausenraum MA1", GetDialogue("Test"));
              
    }

    void Update()
    {
        
        HandlePauseMode();
        
    }

    private void ActivateNPC(string npcName, string[] textLines, bool keyDialogue = false) {

        // Durchlaeuft alle NPC und aktiviert sie, wenn der Name uebereinstimmt
        foreach (NPCController npc in npcs) {
            if (npc.npcName.Equals(npcName)) {
                npc.SetupNPC(textLines, keyDialogue);
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

    public void AddPoint(int amount, bool keyItem) {
        points += amount;

        if (keyItem) {
            keyPoints += 1;
        }

    }

    void OnEnable() {
        // Bei Ausloesen des Events, rufe die GameLoop-Methode auf (Abonnieren)
        ItemController.OnKeyItemCollected += GameLoop;
        NPCController.OnKeyDialogueFinished += GameLoop;
    }

    void OnDisable() {  
        // Bei Zerstoerung des GameObjects mit dem Event, rufe nicht mehr die GameLoop-Methode auf (Abbestellen)
        ItemController.OnKeyItemCollected -= GameLoop;
        NPCController.OnKeyDialogueFinished -= GameLoop;
    }

    private void GameLoop() {

        switch(keyPoints) {
            case 0:
            break;
            case 1:
            break;
            case 2:
            break;
            case 3:
            break;
            case 4:
            break;
            case 5:
            break;
            case 6:
            break;
            case 7:
            break;
            case 8:
            break;
            case 9:
            break;
            case 10:
            break;
            case 11:
            break;
            case 12:
            break;
        }
    }

    private void InitializeGameScript() {

        AddDialogue("GameStart", "Herzlich Willkommen bei der Fleet GmbH" + "\n\n" + "Wir wünschen dir viel Spaß bei dem Azubi-Onbarding!");
        AddDialogue("Aussenbereich0", "Guten Morgen!", "Du bist der neue Auszubildende, nicht wahr?", "Melde dich bitte drinnen an der Rezeption");
        AddDialogue("Empfang0", "Hallo! Du hast heute deinen ersten Tag?", "Herzlich Willkomen bei der Fleet GmbH!", "Nimm dir bitte zunächst eine Mitarbeiterkarte von dem Tisch links");
        AddDialogue("Empfang1", "Super! Trage die Mitarbeiterkarte bitte immer bei dir.", "Manchmal wirst du weitere Gegenstände auf dem Boden finden können.", "Bitte sammle diese Gegenstände ein!", "Melde dich nun am besten bei deinem Ausbilder oben in der Vertriebsabteilung", "Die Vertriebsabteilung findest du oben, die Treppen hoch und dann links.");
        AddDialogue("Vertriebsabteilung2", "Willkommen! Da bist du ja endlich.", "Ich werde dein Ausbilder sein.", "Wie du sicher schon weißt, verkaufen wir hier Fahrzeuge.", "Wie genau das funktioniert, wirst du die nächsten Wochen von mir lernen." , "Zünächst habe ich aber eine Bitte an dich.", "Hol mir bitte einen Kaffe aus der Kueche.");
        AddDialogue("Kueche", "Oh Nein...", "Dieses Chilli war ein Familienrezept...");
        AddDialogue("Vertriebsabsteilung3", "Danke für den Kaffee.", "Nun habe ich eine kleine Aufgabe für dich.", "Jeden Tag bekommen wir neue Post von unseren Kunden.", "Bitte hole die Post bei der Poststelle unten an der Rezeption ab.");
        AddDialogue("Empfang4", "Du suchst die Poststelle?", "Die Poststelle findest du links, allerdings habe ich den Schlüssel verlegt.", "Ich glaube ich habe den Schlüssel in einem der beiden Meetingräume vergessen", "Suche den Schlüssel in den Meetingräumen gegenüber.");
        AddDialogue("Empfang5", "Klasse! Die Poststelle findest du links.");
        AddDialogue("Vertriebsabteilung6", "Gut gemacht.", "Als nächstes brauchst du einen Laptop.", "Bitte hole deinen Laptop bei der IT-Abteilung ab, ganz hinten durch im Flur.");
        AddDialogue("IT-Abteilung7", "Hey! Willkommen in der IT-Abteilung!", "Wir kümmern uns um die Technik in der Firma.", "Du brauchst einen Laptop?", "Laptops findest du durch die Tür rechts im Serverraum.");
        AddDialogue("IT-Abteilung8", "Wenn du Hilfe mit dem Laptop brauchst, musst du ein Ticket erstellen.");
        AddDialogue("Vertriebsabteilung8", "Super, das wichtigste wäre damit erledigt.", "Als nächstes solltest du dich dem Manager vorstellen.", "Der Manager befindet sich direkt gegenüber");
    }

    private void GameEnding() {
        
        AddDialogue("GameFinish", "Herzlichen Glückwunsch!" + "\n" + "Du hast das Azubi-Onboarding erfolgreich abgeschlossen" + "\n\n" + "Gegenstände: " + GetFinalPercentage() + "% gefunden");

        fadeController.FadeIn(GetDialogue("GameFinish"), 10);
        
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
