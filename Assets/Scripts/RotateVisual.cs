using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotateCamera : MonoBehaviour
{

    public float sensitivity = 0.1f; // Sensibilità della rotazione
    public Animator animator;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 initialPos;
    public float zoom;
    private Vector3 zoomVector;
    private Vector3 newPos;

    public PlayerInput playerInput;
    private InputAction aimRotation;
    private InputAction zoomAction;
    private bool aiming = false;
    private Vector2 delta;
    void Start()
    {
        aimRotation = playerInput.actions["Look"];
        zoomAction = playerInput.actions["Aim"];
        zoomAction.performed += ctx => { aiming = true; };
        zoomAction.canceled += ctx => { aiming = false; };
        // Blocca il cursore al centro dello schermo e lo rende invisibile
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialPos = transform.localPosition;
        zoomVector = Vector3.one * zoom;
        zoomVector.y = 0;
        zoomVector.x = 0;
    }

    void Update()
    {
        delta = aimRotation.ReadValue<Vector2>();
        if (aiming && transform.localPosition.z != (initialPos + zoomVector).z)
        {
            newPos = transform.localPosition;
            newPos.z = Vector3.Lerp(transform.localPosition, initialPos + zoomVector, Time.fixedDeltaTime * 20).z;
            transform.localPosition = newPos;
        }

        else if (transform.localPosition.z != initialPos.z)
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, Time.deltaTime * 5);


        // Calcola la direzione di rotazione in base al movimento del mouse
        rotationY += delta.x * sensitivity;  // Ruota sull'asse Y (orizzontale)
        rotationX -= delta.y * sensitivity;  // Ruota sull'asse X (verticale)

        // Limita la rotazione X per evitare rotazioni strane
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Ruota la telecamera in modo fluido (più lento se il mouse si sposta meno)
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}


