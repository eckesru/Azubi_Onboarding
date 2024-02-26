using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private void ActivateItems(string itemName) {

        // Durchlaeuft alle Items und aktiviert sie, wenn der Name uebereinstimmt
        foreach (ItemController item in items) {
            if (item.itemName.Equals(itemName)) {
                item.gameObject.SetActive(true);
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
                ActivateNPC("Aussenbereich", GetDialogue("Aussenbereich0"), true);
            break;

            case 1:
                UnlockDoors("Flur 1");
                UnlockDoors("Empfang");
                ActivateNPC("Empfang", GetDialogue("Empfang1"), true);
            break;

            case 2:
                ActivateItems("Mitarbeiterausweis");
            break;

            case 3:
                ActivateNPC("Empfang", GetDialogue("Empfang3"), true);
            break;

            case 4:
                ActivateItems("Standard");
                UnlockDoors("Sanitaerraum 1");
                UnlockDoors("Treppenhaus");
                UnlockDoors("Flur 2");
                UnlockDoors("Vertriebsabteilung");
                UnlockDoors("Meetingraum 3");
                UnlockDoors("Sanitaerraum 2");
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabteilung4"), true);
            break;

            case 5:
                UnlockDoors("Kueche");
                ActivateItems("Kaffee");
                ActivateNPC("Kueche", GetDialogue("Kueche"));
            break;

            case 6:
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabsteilung6"), true);
            break;

            case 7:
                ActivateNPC("Empfang", GetDialogue("Empfang7"), true);
            break;

            case 8:
                UnlockDoors("Meetingraum 1");
                ActivateNPC("Meetingraum 1", GetDialogue("Meetingraum"));
                UnlockDoors("Meetingraum 2");
                ActivateItems("Schluessel");
            break;

            case 9:
                ActivateNPC("Empfang", GetDialogue("Empfang9"));
                UnlockDoors("Poststelle");
                ActivateNPC("Poststelle", GetDialogue("Poststelle"));
                ActivateItems("Post");
            break;

            case 10:
                ActivateNPC("Empfang", GetDialogue("Empfang10"));
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabsteilung10"), true);
            break;

            case 11:
                ActivateNPC("IT-Abteilung", GetDialogue("IT-Abteilung11"), true);
                UnlockDoors("IT-Abteilung");
            break;

            case 12:
                UnlockDoors("Serverraum");
                ActivateItems("Laptop");
            break;
        
            case 13:
                ActivateNPC("IT-Abteilung", GetDialogue("IT-Abteilung13"));
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabteilung13"), true);
            break;
                        
            case 14:
                UnlockDoors("Pausenraum");
                ActivateNPC("Pausenraum", GetDialogue("Pausenraum14"), true);
            break;
                        
            case 15:
                ActivateItems("Sandwich");
                ActivateNPC("Pausenraum", GetDialogue("Pausenraum15"));
            break;

            case 16:
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabteilung16"), true);
            break;

            case 17:
                UnlockDoors("Geschaeftsfuehrer");
                ActivateNPC("Geschaeftsfuehrer", GetDialogue("Geschaeftsfuehrer17"), true);
            break;

            case 18:
                UnlockDoors("Lagerraum");
                ActivateNPC("Lagerraum", GetDialogue("Lagerraum18"), true);
            break;

            case 19:
                ActivateItems("Golfschlaeger");
            break;

            case 20:
                ActivateNPC("Lagerraum", GetDialogue("Lagerraum20"));
                ActivateNPC("Geschaeftsfuehrer", GetDialogue("Geschaeftsfuehrer20"), true);
            break;

            case 21:
                ActivateNPC("Vertriebsabteilung", GetDialogue("Vertriebsabteilung21"), true);
            break;

            case 22:
                StartCoroutine(GameEnding());
            break;
        }
    }

    private void InitializeGameScript() {

        AddDialogue("GameStart", "Herzlich Willkommen bei der Fleet GmbH" + "\n\n" + "Wir wünschen dir viel Spaß bei dem Azubi-Onbarding!");
        AddDialogue("Aussenbereich0", "Guten Morgen!", "Du bist der neue Auszubildende, nicht wahr?", "Melde dich bitte drinnen an der Rezeption.");
        AddDialogue("Empfang1", "Hallo! Du hast heute deinen ersten Tag?", "Herzlich Willkomen bei der Fleet GmbH!", "Nimm dir bitte zunächst einen Mitarbeiterausweis von dem Tisch links.");
        AddDialogue("Empfang3", "Super! Trage den Mitarbeiterausweis bitte immer bei dir.", "Manchmal wirst du weitere Gegenstände im Büro finden können.", "Bitte sammle diese Gegenstände ein!", "Melde dich nun am besten bei deinem Ausbilder in der Vertriebsabteilung.", "Die Vertriebsabteilung findest du oben, Treppen hoch und dann links.");
        AddDialogue("Vertriebsabteilung4", "Willkommen! Da bist du ja endlich.", "Ich werde dein Ausbilder sein.", "Wie du sicher schon weißt, verkaufen wir hier Fahrzeuge.", "Wie genau das funktioniert, wirst du die nächsten Wochen von mir lernen." , "Zunächst habe ich aber eine Bitte an dich.", "Hol mir bitte einen Kaffe aus der Kueche.");
        AddDialogue("Kueche", "Dieses Chilli basierte auf einem Familienrezept...", "Oh Nein...");
        AddDialogue("Vertriebsabsteilung6", "Danke für den Kaffee.", "Nun habe ich eine kleine Aufgabe für dich.", "Jeden Tag bekommen wir neue Post von unseren Kunden.", "Bitte hole die Post bei der Poststelle unten an der Rezeption ab.");
        AddDialogue("Empfang7", "Du suchst die Poststelle?", "Die Poststelle findest du links, allerdings habe ich den Schlüssel verlegt.", "Ich glaube ich habe ihn in einem der beiden Meetingräume vergessen.", "Suche den Schlüssel in den beiden Meetingräumen gegenüber.");
        AddDialogue("Meetingraum", "Ich habe gleich einen wichtigen Kundentermin.");
        AddDialogue("Empfang9", "Klasse! Die Poststelle findest du links.");
        AddDialogue("Poststelle", "Hi! Ich bin für die tägliche Postbearbeitung zuständig.", "Jeden Tag erhalten wir unzählige Briefe.", "Darunter befinden sich Rechnungen, Verträge, Bestellungen und vieles weitere...", "Meine Aufgabe ist dabei, die Post den richtigen Kollegen zuzuordnen.", "Hab noch einen schönen Tag!");
        AddDialogue("Empfang10", "Wenn du Aufgaben brauchst, frage einfach deinen Ausbilder.");
        AddDialogue("Vertriebsabsteilung10", "Gut gemacht.", "Als nächstes brauchst du einen Laptop.", "Bitte hole deinen Laptop bei der IT-Abteilung, ganz hinten durch im Flur.");
        AddDialogue("IT-Abteilung11", "Hey! Willkommen in der IT-Abteilung!", "Wir kümmern uns um die Technik in der Firma.", "Du brauchst einen Laptop?", "Deinen Laptop findest du im Serverraum, rechts durch die Türe.");
        AddDialogue("IT-Abteilung13", "Wenn du Hilfe mit dem Laptop brauchst, musst du ein Ticket erstellen.");
        AddDialogue("Vertriebsabteilung13", "Super, das wichtigste wäre damit erledigt.", "Wie wäre es mit einer kleinen Pause?", "Es ist sehr wichtig, sich die Zeit für eine Pause zu nehmen.", "Schnapp dir ein Sandwich aus dem Pausenraum, gegenüber von der IT-Abteilung!");
        AddDialogue("Pausenraum14", "Neuling. Du möchtest ein Sandwich?", "In Ordnung, lass mir dir aber zuerst etwas zum Thema Datenschutz erzählen.", "Ich als Datenschutzbeauftragter überwache strengstens die Einhaltung der Datenschutzgesetze.", "Die Kenntnis der DSGVO ist das A und O jedes guten Mitarbeiters.", "Der Schutz von personenbezogenen Daten hat oberste Priorität.", "Kapiert? Dann darfst du dir jetzt ein Sandwich schnappen.");
        AddDialogue("Pausenraum15", "Niemals den Datenschutz vergessen, klar?");
        AddDialogue("Vertriebsabteilung16", "Mit einem vollen Magen lässt sich gleich besser arbeiten, was?", "Als letzte Aufgabe solltest du dich dem Manager vorstellen.", "Das Büro des Managers findest du direkt gegenüber.");
        AddDialogue("Geschaeftsfuehrer17","Ah, Richtig, du bist der neue Lehrling!", "Ich bin der Manager der Fleet GmbH.", "Wenn du dich gut anstellst, kannst du auch mal so werden wie ich.", "Deshalb habe ich auch eine sehr wichtige Aufgabe für dich.", "Ich brauche unbedingt einen neuen Golfschläger aus dem Lager.", "Das Lager findest du im Erdgeschoss, hinter der braunen Tür.");
        AddDialogue("Lagerraum18", "Neuer, lass mich dir einen Ratschlag geben.", "Kaffee kochen ist die wichtigste Fähigkeit jedes Auszubildenden.", "Wenn du das gut machst, kommst du ganz groß raus.", "Golfschläger? Die findest du hinten durch.");
        AddDialogue("Lagerraum20", "Nur nichts überstürzen, Neuer.");
        AddDialogue("Geschaeftsfuehrer20", "Genau diesen Golfschläger habe ich gebraucht.", "Gute Arbeiter sind mir sehr wichtig. Also streng dich immer an!", "Ich habe jetzt einen wichtigen Termin, du musst also gehen.");
        AddDialogue("Vertriebsabteilung21", "Hast dich dem Manager vorgestellt?", "Der Manager kann etwas streng sein, er ist aber eigentlich ein lieber Kerl.", "Damit hast du alle Aufgaben abgeschlossen.", "Du hast nun die wichtigsten Kollegen im Büro kennengelernt. Klasse!","Du solltest jetzt Feierabend machen, den hast du dir verdient.", "Bis morgen!");
    }

    private IEnumerator GameEnding() {

        gameRunning = false;

        // Kurze Verzoegerung des Spielendes
        yield return new WaitForSecondsRealtime(2);

        fadePanel.SetActive(true);
        
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
        GameLoop();

        playerController.transform.Find("Main Camera").rotation = new Quaternion(0,0,0,0);

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
                UpdateFoundItemsLabel();
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

    private void UpdateFoundItemsLabel() {
    	AddDialogue("ItemFoundLabel", points + " / " + itemAmount + " Gegenstände");

        TextMeshProUGUI pauseTMP = pausePanel.transform.Find("ItemLabel").GetComponent<TextMeshProUGUI>();

        pauseTMP.SetText(GetDialogue("ItemFoundLabel")[0]);
        pauseTMP.ForceMeshUpdate();
    }
}
