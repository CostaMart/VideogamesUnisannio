using System;
using System.Numerics;
using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CharMovement : MonoBehaviour
{
    public CinemachineCamera camera;
    public float moveSpeed = 5f;
    public float rotationSpeed = 0.5f;
    public Animator _animator;

    private float usedRotationSp;
    private float aimingRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        usedRotationSp = rotationSpeed;
        aimingRotation = rotationSpeed + 10;

    }



    void Update()
    {
        Move();
        Aim();
    }

    private void Move()
    {
        Vector3 direction = Vector3.zero;

        // Determina la direzione del movimento rispetto alla telecamera
        if (Input.GetKey(KeyCode.W)) direction += camera.transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= camera.transform.forward;
        if (Input.GetKey(KeyCode.D)) direction += camera.transform.right;
        if (Input.GetKey(KeyCode.A)) direction -= camera.transform.right;

        // Annulla la componente verticale per evitare movimenti indesiderati
        direction.y = 0;

        // Se c'è movimento, normalizza la direzione e aggiorna la posizione
        if (direction != Vector3.zero)
        {
            _animator.SetBool("walking", true);

            // Se non stiamo mirando, il personaggio ruota verso la direzione di movimento
            if (!_animator.GetBool("aiming"))
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, usedRotationSp);
            }

            // Muove il personaggio nella direzione calcolata
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            _animator.SetBool("walking", false);
        }
    }

    private void Aim()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _animator.SetBool("aiming", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _animator.SetBool("aiming", false);
        }
        // se stai mirando gira il personaggio nella direzione della videocamera
        if (_animator.GetBool("aiming"))
        {
            Vector3 q = camera.transform.forward;
            transform.forward = Vector3.Lerp(transform.forward, q, aimingRotation * Time.deltaTime);
        }

    }

}
