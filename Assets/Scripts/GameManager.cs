using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

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
}
