using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkMovementSpeed = 70.0f;
    [SerializeField] private float runMovementSpeed = 120.0f;
    [SerializeField] public Transform cameraView;
    [SerializeField] private Transform playerObject;

    private float movementSpeed;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    // private float slopeAngle = 40.0f;
    private RaycastHit slopeHit;

    private float playerHeight;

    private LayerMask groundLayerMask;
    private LayerMask slopeLayerMask;

    [SerializeField] private float dragForce = 10.0f;
    [SerializeField] private float groundCheckDistance = 0.3f;

    Collider[] collidersInRange;
    [SerializeField] private float interactRange;

    [SerializeField] private GameManager gameManager;

    private Rigidbody rb;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();

        groundLayerMask = LayerMask.GetMask("Ground");
        slopeLayerMask = LayerMask.GetMask("Slope");

        playerHeight = cameraView.position.y;
    }

    void Start()
    {

    }

    void Update()
    {

        CheckInteractions();
        
    }

    private void FixedUpdate()
    {

        HandleMovementSpeed();

        HandleMovementOnSlope();

        HandlePlayerMovement();

        HandlePlayerRotation();

        PreventPlayerClimbing();

    }

    private void LateUpdate() {

    }

    private bool OnSlope() { 

        // Pruefung, ob Player sich auf einem Slope befindet
        return Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight, slopeLayerMask);

        // Alternative Version ohne LayerMask, Winkelberechnung ist beim rueckwaertsgehen allerdings inkorrekt
        //float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        //return angle < slopeAngle && angle != 0;

    }

    private Vector3 GetSlopeDirection()
    {
        // Holt die Richtung des Slopes.
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void HandleMovementSpeed()
    {
        // Anpassen von movementSpeed bei LeftShift zum Rennen
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = runMovementSpeed;
        }
        else
        {
            movementSpeed = walkMovementSpeed;
        }
    }

    private void HandleMovementOnSlope()
    {
        // Handhabung von der Bewegung des Spielers auf einem Slope
        // Schwerkraft wird auf Slopes deaktiviert, um Rutschen zu verhindern
        if (OnSlope())
        {
            // Verlangsamung des Spielers auf Slopes
            movementSpeed = movementSpeed * 0.6f;

            // Anpassung der Bewegungskraft in die Richtung des Slopes
            rb.AddForce(GetSlopeDirection() * movementSpeed, ForceMode.Force);

            rb.useGravity = false;

            if (rb.velocity.y < 0)
            {
                // Beim Absteigen von Slopes wird der Spieler leicht heruntergedrueckt
                rb.AddForce(Vector3.down * 40.0f, ForceMode.Force);
            }
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void HandlePlayerMovement()
    {
        // Generelle Handhabung der Bewegung des Spielers

        // Holen der Eingabe durch WASD/Pfeiltasten
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Berechnet die Bewegung anhand der Blickrichtung der Kamera
        direction = cameraView.forward * vertical + cameraView.right * horizontal;

        // Verhindert Veraenderung der Hoehe (Fliegen), wenn die Kamera nach oben/unten schaut
        direction.y = 0;

        // Fuehrt die Bewegung in die Blickrichtung der Kamera aus
        rb.AddForce(direction.normalized * movementSpeed, ForceMode.Force);

        // Fixiert die Ziehkraft des Spielers auf einen fixen Wert (Verhindert Rutschen auf dem Boden)
        rb.drag = dragForce;
    }

    private void HandlePlayerRotation()
    {
        // Passt die Blickrichtung des PlayerObjects analog an die Kamerabewegung an.
        transform.rotation = Quaternion.Euler(0f, cameraView.eulerAngles.y, 0f);
    }


    private void PreventPlayerClimbing()
    {
            if (!(OnGround() || OnSlope())) {
                // Drueckt denn Spieler auf den Boden, wenn er diesen unerwuenscht verlaesst
                rb.AddForce(Vector3.down * 200.0f, ForceMode.Force);
        }
    }



private bool OnGround()
{
    return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayerMask);
}


private void CheckInteractions() {
    // Definiert die Größe und die Mitte der Box, die verwendet werden soll
    Vector3 boxSize = new Vector3(interactRange, 2.8f, interactRange); // Stelle die Größe entsprechend deinen Bedürfnissen ein
    Vector3 boxCenter = transform.position + Vector3.up * boxSize.y / 2; // Zentriert die Box in einer angemessenen Höhe relativ zum Spieler

    // Prüft, welche Collider von GameObjects sich innerhalb der Box befinden
    collidersInRange = Physics.OverlapBox(boxCenter, boxSize / 2, transform.rotation);

    foreach (Collider collider in collidersInRange) {
        // Überprüft, ob das Objekt eine IInteractable-Komponente hat und innerhalb der interactRange liegt
        if (collider.TryGetComponent<IInteractable>(out var interactable)) {

            // Ermittelt die Distanz zwischen Spieler und Objekt
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            // Führt die Interact()-Methode des Objekts aus, wenn es innerhalb der Reichweite ist
            if (interactable.interactRange >= distance) {
                interactable.Interact();
            }
        }
    }
}

}
