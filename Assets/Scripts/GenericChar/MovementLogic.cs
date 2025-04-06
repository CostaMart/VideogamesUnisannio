
using System.Numerics;
using Unity.Cinemachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class MovementLogic : MonoBehaviour
{

    private Animator anim;
    public CinemachineCamera camera;
    private int jumpsAvailable = 1;
    private bool aiming = false;


    private Rigidbody rb;
    private Collider col;

    // movement event manager 
    [SerializeField] private CharStats playerSettings;
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
        jumpsAvailable = playerSettings.MaxJumps;

        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        Application.targetFrameRate = 120;
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
            Quaternion n = Quaternion.LookRotation(q);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, n, playerSettings.RotationSpeed * Time.deltaTime));

        }

        if (direction != Vector3.zero)
        {
            if (!aiming)
            {
                // rotazione basata sulla direzione di movimento, se non si mira
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, playerSettings.RotationSpeed * Time.deltaTime));
            }


            rb.MovePosition(transform.position + direction.normalized * playerSettings.MoveSpeed * Time.deltaTime);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("terrain"))
            jumpsAvailable = playerSettings.MaxJumps;
    }

    public void Jump()
    {
        Vector3 direction = Vector3.zero;

        direction += camera.transform.forward * moveDirection.y;
        direction += camera.transform.right * moveDirection.x;

        if (jumpsAvailable <= 0) return;


        rb.linearVelocity = new Vector3(direction.x * playerSettings.jumpSpeedx, playerSettings.jumpSpeedy, direction.z * playerSettings.jumpSpeedz);
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
