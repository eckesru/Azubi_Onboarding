using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{

    [SerializeField] private bool keyItem = false;
    [SerializeField] private float _interactRange = 3.0f;
    public float interactRange {get {return _interactRange;} private set{_interactRange = value;}}
    private float speed = 8.0f;
    private bool pickedUp = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp) {
            // Holt die Position des Spieler und erhöht den Y-Wert etwas, damit das Item etwa in die Haende des Spielers fliegt
            Vector3 playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 0.8f, player.transform.position.z);
            // Bewegt das Item in Richtung des Spielers
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
            CheckDistanceAndDestroy();
}
    }

    private void CheckDistanceAndDestroy() {

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance < 1.0f) {

            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.AddPoints(1);
            Destroy(gameObject);
        }
    }

    public void Interact() {

    if (!pickedUp) {
        pickedUp = true;
        transform.Find("ItemParticle").gameObject.SetActive(false);
    }
}
}