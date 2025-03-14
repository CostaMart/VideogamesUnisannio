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
    public GameObject weapon;
    private bool aiming = false;
    private bool lastWas = false;
    public float moveSpeed = 5f;


    // riferimetno alla posizione desiderata durante la mira
    [Tooltip("Ik settings")]
    [Header("IK Settings")]
    public Animator _animator;
    [Tooltip("in order to activate IK functinoalities, it is required to apply a constraint ad pass it to this cript. This is the constraint that will move the character upper body part")]
    public MultiAimConstraint aimConstraint;

    [Tooltip("this constraint is required to move the character left hand in order to grab the weapon")]
    public TwoBoneIKConstraint twoBoneIKConstraint;

    [Tooltip("represents the postion of the weapon when the character is not aiming")]
    public Transform relaxedWeaponStand;

    [Tooltip("represents the postion of the weapon when the character is aiming")]
    public Transform aimingWeaponStand;

    public RigBuilder rb;

    [Tooltip("reference to the transform representing where the left hand should be placed on the weapon when the character is aiming")]
    public Transform wpnFrontHandle;

    [Tooltip("reference to the left hand Ik target transform")]
    public Transform IKLeftHand;

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



        // se  stiamo mirando spostiamo l'arma nella posizione di mira, si controlla lastwas per non farlo ripetutamente
        if (aiming && !lastWas)
        {
            lastWas = true;
            weapon.transform.SetParent(aimingWeaponStand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }

        // se non stiamo mirando spostiamo l'arma nella posizione di riposo, si controlla lastwas per non farlo ripetutamente
        if (!aiming && lastWas)
        {
            lastWas = false;
            weapon.transform.SetParent(relaxedWeaponStand);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
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
            if (aimConstraint.data.sourceObjects.GetWeight(0) != 0)
            {
                var newSourceObject = aimConstraint.data.sourceObjects;
                newSourceObject.SetWeight(0, 0);
                newSourceObject.SetWeight(1, 1);
                var newConstraintData = aimConstraint.data;
                newConstraintData.offset = new Vector3(-27.3f, 0, 0);
                aimConstraint.data = newConstraintData;
                aimConstraint.data.sourceObjects = newSourceObject;
                rb.Build();
            }



            twoBoneIKConstraint.weight = 1;
            IKLeftHand.position = wpnFrontHandle.position;
            IKLeftHand.rotation = wpnFrontHandle.rotation;
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
                if (aimConstraint.data.sourceObjects.GetWeight(0) != 1)
                {
                    var newSourceObject = aimConstraint.data.sourceObjects;
                    newSourceObject.SetWeight(0, 1);
                    newSourceObject.SetWeight(1, 0);
                    aimConstraint.data.sourceObjects = newSourceObject;
                    var newConstrintData = aimConstraint.data;
                    newConstrintData.offset = Vector3.zero;
                    aimConstraint.data = newConstrintData;
                    rb.Build();
                }

                twoBoneIKConstraint.weight = 0;
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
