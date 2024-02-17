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

    [SerializeField] private NPCController testnpc;

    private DoorController[] doors;

    private int points = 0;

    public bool gameRunning = false;
    public bool gamePaused = false;

    void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {

        // Zentriert den Cursor in der Mitte und blendet ihn aus
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Startet das Spiel
        //Time.timeScale = 1.0f;
        //gameRunning = true;

        doors = FindObjectsOfType<DoorController>();

        UnlockDoors("Flur 1");
        UnlockDoors("Empfang");

  //      string[] npc1text = new string[] {"Oh, du bist der neue. Richtig?", "Komm, ich zeig dir mal was.", "Das wird auch ganz lustig!"};
 //       testnpc.SetupNPC(npc1text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UnlockDoors(String roomName) {

        // Durchlaeuft alle Tueren und oeffnet sie, wenn der Raumname uebereinstimmt
        foreach (DoorController door in doors) {
            if (door.GetRoomName().Equals(roomName)) {
                door.locked = false;
            }
        }
    }

    public void AddPoints(int amount) {
        points += amount;
    }

}
