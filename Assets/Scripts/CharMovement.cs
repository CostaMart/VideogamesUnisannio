using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using NUnit.Framework.Constraints;
using Palmmedia.ReportGenerator.Core;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharMovement : MonoBehaviour
{

    public PlayerInput playerInput;
    private InputAction aim;
    private InputAction move;
    private InputAction reload;
    public CinemachineCamera camera;
    private bool aiming = false;
    private bool lastWas = false;
    public float moveSpeed = 5f;



    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    private float usedRotationSp;
    private float aimingRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // input system setup
        move = playerInput.actions["Move"];
        aim = playerInput.actions["Aim"];
        reload = playerInput.actions["Reload"];
        aim.performed += ctx => { aiming = true; };
        aim.canceled += ctx => { aiming = false; };

        Application.targetFrameRate = 60;
        usedRotationSp = rotationSpeed;
        aimingRotation = rotationSpeed + 10;
    }

    void Update()
    {
        Move();
        CheckActions();
    }

    private void Move()
    {

        Vector2 directionInput = move.ReadValue<Vector2>();
        Vector3 direction = Vector3.zero;

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

    void CheckActions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // fare salto
        }
    }



}
