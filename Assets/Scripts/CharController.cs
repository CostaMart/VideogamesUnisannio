using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Constraints;
using Palmmedia.ReportGenerator.Core;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class CharMovement : MonoBehaviour
{

    // input actions
    public PlayerInput playerInput;
    private InputAction aim;
    private InputAction move;
    private InputAction reload;
    private InputAction jump;
    // ------------------------------------
    private Animator anim;
    public CinemachineCamera camera;
    public float JumpSpeed = 5f;
    public int maxJumps = 1;
    private int jumpsAvailable = 1;
    private bool aiming = false;
    private bool lastWas = false;
    public float moveSpeed = 5f;


    private Rigidbody rb;
    private Collider col;

    // ragdolling managamenet
    public float speedLimitBeforeRagdolling = 10f;
    private GameObject ragdollReference;

    private bool ragdolling = false;

    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    private float usedRotationSp;
    private float aimingRotation;

    // Gestione Event System

    [Tooltip("Evento chiamato quando il personaggio entra in modalità ragdoll")]
    public UnityEvent<bool, GameObject> onRagdolling;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        jumpsAvailable = maxJumps;
        rb = GetComponent<Rigidbody>();
        Ragdolling(false);

        // input system setup
        move = playerInput.actions["Move"];
        aim = playerInput.actions["Aim"];
        reload = playerInput.actions["Reload"];
        jump = playerInput.actions["Jump"];
        aim.performed += ctx => { aiming = true; };
        aim.canceled += ctx => { aiming = false; };
        jump.performed += ctx => { Jump(); };


        Application.targetFrameRate = 60;
        usedRotationSp = rotationSpeed;
        aimingRotation = rotationSpeed + 10;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {

        Vector2 directionInput = move.ReadValue<Vector2>();
        Vector3 direction = Vector3.zero;

        if (!ragdolling && (Math.Abs(rb.linearVelocity.y) > speedLimitBeforeRagdolling || Math.Abs(rb.linearVelocity.x) > speedLimitBeforeRagdolling || Math.Abs(rb.linearVelocity.z) > speedLimitBeforeRagdolling))
        {
            Ragdolling(true);
            ragdolling = true;
        }


        if (ragdolling)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Ragdolling(false);
                ragdolling = false;
            }

            // se siamo in modalità ragdoll non possiamo muoverci
            return;
        }

        // Determina la direzione del movimento rispetto alla telecamera
        direction += camera.transform.forward * directionInput.y;
        direction += camera.transform.right * directionInput.x;

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

    void Jump()
    {
        if (jumpsAvailable <= 0) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpSpeed, rb.linearVelocity.z);
        jumpsAvailable--;
    }

    /*Activate or deactivate ragdolling*/
    void Ragdolling(bool ragdolling)
    {

        Rigidbody[] r = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody otherRigid in r)
        {
            if (rb != otherRigid)
            {
                otherRigid.isKinematic = !ragdolling;
                otherRigid.linearVelocity = rb.linearVelocity;
                otherRigid.angularVelocity = rb.angularVelocity;

                // è necessario recuperare questa reference per posizionare il
                //  personaggio correttamente quando si ripristinerà dopo la ragdoll
                if (ragdollReference == null && otherRigid.transform != null)
                    ragdollReference = otherRigid.gameObject;
            }


            if (otherRigid == r.Last())
            {
                onRagdolling?.Invoke(ragdolling, otherRigid.gameObject);
            }

        }

        foreach (Collider otherCollid in GetComponentsInChildren<Collider>())
        {
            if (otherCollid != col)
            {
                otherCollid.enabled = ragdolling;
            }
        }

        if (!ragdolling)
        {
            transform.position = ragdollReference.transform.position;
        }

        anim.enabled = !ragdolling;
        col.enabled = !ragdolling;
        rb.isKinematic = ragdolling;
    }


}
