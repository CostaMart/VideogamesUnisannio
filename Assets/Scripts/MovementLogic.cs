
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class CharMovementLogic : MonoBehaviour
{

    private Animator anim;
    public CinemachineCamera camera;
    public float JumpSpeed = 5f;
    public int maxJumps = 1;
    private int jumpsAvailable = 1;
    private bool aiming = false;

    public float moveSpeed = 5f;


    private Rigidbody rb;
    private Collider col;

    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    private float usedRotationSp;
    private float aimingRotation;


    // movement event manager 
    [SerializeField] private ControlEventManager controlEventManager;
    private Vector3 moveDirection = Vector3.zero;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // connect to movement event manager
        controlEventManager.AddListenerMove(Move);
        controlEventManager.AddListenerJump(Jump);
        controlEventManager.AddListenerAiming((value) => Aiming = value);
    }
    void Start()
    {

        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        jumpsAvailable = maxJumps;
        rb = GetComponent<Rigidbody>();

        Application.targetFrameRate = 60;
        usedRotationSp = rotationSpeed;
        aimingRotation = rotationSpeed + 10;
    }


    private void Update()
    {

        Vector3 direction = Vector3.zero;



        // se non è attivo il rigidBody principale, non possiamo muoverci
        if (rb.isKinematic == true)
            return;


        // Determina la direzione del movimento rispetto alla telecamera
        direction += camera.transform.forward * moveDirection.y;
        direction += camera.transform.right * moveDirection.x;

        // Annulla la componente verticale per evitare movimenti indesiderati
        direction.y = 0;


        if (aiming)
        {
            // rotazione basata sulla direzione della telecamera, se stiamo mirando
            Vector3 q = camera.transform.forward;
            q.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, q, aimingRotation * Time.deltaTime);
        }

        if (direction != Vector3.zero)
        {
            if (!aiming)
            {
                // rotazione basata sulla direzione di movimento, se non si mira
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, usedRotationSp);
            }

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
            jumpsAvailable = maxJumps;
    }

    public void Jump()
    {
        if (jumpsAvailable <= 0) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpSpeed, rb.linearVelocity.z);
        jumpsAvailable--;
    }

    public void Move(Vector2 direction)
    {
        moveDirection = new Vector3(direction.x, direction.y, 0);
    }

    public bool Aiming
    {
        get { return aiming; }
        set { aiming = value; }
    }
}
