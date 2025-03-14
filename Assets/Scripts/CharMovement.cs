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
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CharMovement : MonoBehaviour
{
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
        Application.targetFrameRate = 60;
        usedRotationSp = rotationSpeed;
        aimingRotation = rotationSpeed + 10;
    }

    void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("executed");
    }

    void Update()
    {
        Move();
        CheckActions();
    }

    private void Move()
    {
        // controlliamo eventuale stato di mira
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aiming = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            aiming = false;
        }

        Vector3 direction = Vector3.zero;

        // Determina la direzione del movimento rispetto alla telecamera
        if (Input.GetKey(KeyCode.W)) direction += camera.transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= camera.transform.forward;
        if (Input.GetKey(KeyCode.D)) direction += camera.transform.right;
        if (Input.GetKey(KeyCode.A)) direction -= camera.transform.right;

        // Annulla la componente verticale per evitare movimenti indesiderati
        direction.y = 0;


        //1*\ qui cambiamo il punto verso cui il personaggio si rivolge, quando miriamo lo facciamo girare leggermente più a destra per un effetto migliore
        if (aiming)
        {
            // rotazione basata sulla direzione della telecamera, se stiamo mirando
            Vector3 q = camera.transform.forward;
            q.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, q, aimingRotation * Time.deltaTime);
        }

        if (direction != Vector3.zero)
        {
            // questo è il duale del commento 1*, se non stiamo mirando vogliamo il personaggio rivolto in avanti 
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
