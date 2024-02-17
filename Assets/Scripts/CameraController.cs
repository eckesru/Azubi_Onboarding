using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 100.0f;

    private float xRotation;
    private float yRotation;
    private float mouseX;
    private float mouseY;

    void Start()
    {
        
    }

    void Update()
    {
        CalculateCameraRotation();
    }

    void FixedUpdate()
    {
        UpdateCameraRotation();
    }

    private void CalculateCameraRotation()
    {
        // Holen der Mausbewegung (X und Y-Achse) und Anpassung an Speed
        mouseX = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

        // Berechnung der Aenderung der Blickrichtung
        yRotation += mouseX;
        xRotation -= mouseY;
    }

    private void UpdateCameraRotation() {
        // Ausfuehren der Aenderung der Blickrichtung
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
