using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkMovementSpeed = 70.0f;
    [SerializeField] private float runMovementSpeed = 120.0f;
    [SerializeField] private float dragForce = 10.0f;
    [SerializeField] public Transform cameraView;
    [SerializeField] private Transform playerObject;

    private float movementSpeed;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    private float slopeAngle = 40.0f;
    private RaycastHit slopeHit;

    private float playerHeight;


    [SerializeField] private GameManager gameManager;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerHeight = cameraView.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Zentriert den Cursor in der Mitte und blendet ihn aus
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

        HandleMovementSpeed();

        HandleMovementOnSlope();

        HandlePlayerMovement();

        HandlePlayerRotation();

    }

    private bool OnSlope() { 

        // Pruefung, ob Player sich auf einem Slope befindet
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < slopeAngle && angle != 0;
        }
        return false;
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
                // Beim Absteigen von Slopes wird der Spieler heruntergedrueckt
                rb.AddForce(Vector3.down * 100.0f, ForceMode.Force);
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

        public void HandlePlayerRotation()
    {
        // Passt die Rotation des Spielers analog an die Kamerabewegung an.
         playerObject.rotation = cameraView.rotation;
    }
}
