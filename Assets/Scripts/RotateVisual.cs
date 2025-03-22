using Unity.Cinemachine;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class MouseRotateCamera : MonoBehaviour
{

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Vector3 initialPos;
    private Vector3 zoomVector;

    private InputAction aimRotation;
    private InputAction zoomAction;
    private bool aiming = false;
    private bool zoomed = false;
    private Vector2 delta;

    // ragdolling camera management
    [Tooltip("se viene passato un riferimento al ragdoller la camera gestirà anche gli eventi di attivazione della ragdoll, iniziando a seguirla nel momento in cui viene attivata")]
    [SerializeField] private Ragdoller ragdoller;
    private Vector3 initialLocalPos;
    private GameObject ragdollRef;
    private bool ragdolling = false;

    [Tooltip("necessario per controllare la camera")]
    public PlayerInput playerInput;
    [Header("Camera Settings")]
    [Tooltip("Questo componente gestisce le impostazioni della cinemachinecamera, questo componente è perciò richiesto. In particolare assume che la camera segua un empy gameObject posizionato sul personaggio")]
    public CinemachineThirdPersonFollow followCamera;
    [Tooltip("Zoom della telecamera quando si entra in modalità mira")]
    public CinemachineCamera cineCam;

    [Tooltip("riferimento alla cinecamera")]
    public float zoom;

    [Tooltip("sensibilitià alla rotazione")]
    public float sensitivity = 0.1f; // Sensibilità della rotazione

    [Tooltip("FOV della telecamera quando si entra in modalità mira")]
    public float aimingFov = 40f;

    [Tooltip("FOV della telecamera quando non si è in modalità mira")]
    public float defaultFov = 60f;

    void Awake()
    {
        if (ragdoller != null)
            ragdoller.onRagdolling += OnRagdolling;
        initialLocalPos = transform.localPosition;
    }

    void Start()
    {
        transform.localPosition = initialLocalPos;
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

        if (aiming && !zoomed)
        {
            zoomed = true;

            followCamera.CameraDistance = followCamera.CameraDistance - zoom;
            cineCam.Lens.FieldOfView = aimingFov;
        }
        else if (zoomed && !aiming)
        {
            cineCam.Lens.FieldOfView = defaultFov;
            zoomed = false;
            followCamera.CameraDistance = followCamera.CameraDistance + zoom;
        }

        // Calcola la direzione di rotazione in base al movimento del mouse
        rotationY += delta.x * sensitivity;  // Ruota sull'asse Y (orizzontale)
        rotationX -= delta.y * sensitivity;  // Ruota sull'asse X (verticale)

        // Limita la rotazione X per evitare rotazioni strane
        rotationX = Mathf.Clamp(rotationX, -50f, 90f);

        // Ruota la telecamera in modo fluido (più lento se il mouse si sposta meno)
        if (!ragdolling)
        {
            // Se non siamo in ragdoll, usa la rotazione X e Y specificate
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
        else
        {
            // Se siamo in ragdoll, mantieni l'oggetto orientato verso l'alto lungo l'asse Y
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
            transform.position = ragdollRef.transform.position;
        }
    }
    public void OnRagdolling(bool isRagdolling, GameObject ragdollRef)
    {
        if (!isRagdolling)
            transform.localPosition = initialLocalPos;

        this.ragdollRef = ragdollRef;
        ragdolling = isRagdolling;
    }
}


