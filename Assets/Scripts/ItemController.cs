using UnityEngine;

public class ItemController : MonoBehaviour, IInteractable
{

    [SerializeField] private bool keyItem = false;
    [SerializeField] private float _interactRange = 3.0f;
    [SerializeField] private string _itemName;
    public string itemName {get {return _itemName;} private set {_itemName = value;}}
    [SerializeField] private AudioClip collectItemClip;
    [SerializeField] private AudioClip collectKeyItemClip;
    public float interactRange {get {return _interactRange;} private set{_interactRange = value;}}
    private float speed = 8.0f;
    private bool pickedUp = false;
    private GameObject player;

    public delegate void KeyItemCollected();
    public static event KeyItemCollected OnKeyItemCollected;

    void Start()
    {
        player = GameObject.Find("Player");
    }

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
            gameManager.AddPoint(1, keyItem);

            PlaySound();

            Destroy(gameObject);
        }
    }

    private void PlaySound() {

        if(keyItem) {
            AudioSource.PlayClipAtPoint(collectKeyItemClip, transform.position, 0.5f);

            // Loest Event aus, welches im GameController verarbeitet aus
            OnKeyItemCollected();
        } else {
            AudioSource.PlayClipAtPoint(collectItemClip, transform.position, 0.5f);
        }
        
    }

    public void Interact() {

    if (!pickedUp) {
        pickedUp = true;
        transform.Find("ItemParticle").gameObject.SetActive(false);
    }
}
}