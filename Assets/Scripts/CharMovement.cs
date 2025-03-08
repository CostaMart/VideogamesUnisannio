using System;
using NUnit.Framework.Constraints;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Aim();
    }
    private void Move()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _animator.SetBool("walking", true);
            Vector3 forward = camera.transform.forward; // muovi avanti

            /*calcola la direzione in cui spostarsi ruotando il vettore "forward".
             Viene utilizzato il booleano corrsipondente al tasto premuto per aggiungere o no il termine alla somma. 
             In questo modo se il tasto non è premuto 
             non verrà consdierata la corrispettiva direzione nel calcolo del vettore*/
            Vector3 direction =
                forward * Convert.ToInt32(Input.GetKey(KeyCode.W)) +
                Quaternion.AngleAxis(90f, Vector3.up) * forward * Convert.ToInt32(Input.GetKey(KeyCode.D)) +
                Quaternion.AngleAxis(180f, Vector3.up) * forward * Convert.ToInt32(Input.GetKey(KeyCode.S)) +
                Quaternion.AngleAxis(-90f, Vector3.up) * forward * Convert.ToInt32(Input.GetKey(KeyCode.A));


            // Normalizza la direzione per evitare velocità più elevate quando si premono tasti diagonali
            direction.Normalize();

            // Annulla la componente Y della direzione per evitare movimenti verticali
            direction.y = 0;

            // per rispettare lo schema di comandi se non sati mirando volta il personaggio nella direzione di movimento
            if (!_animator.GetBool("aiming"))
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, usedRotationSp * Time.fixedDeltaTime);
            }
            // Muove l'oggetto nella direzione desiderata
            transform.Translate(direction * moveSpeed, Space.World);
        }
        else
        {
            _animator.SetBool("walking", false);
        }
    }

    private void Aim()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _animator.SetBool("aiming", true);
        }
        else
        {
            _animator.SetBool("aiming", false);
        }
        // se stai mirando gira il personaggio nella direzione della videocamera
        if (_animator.GetBool("aiming"))
        {
            Quaternion q = Quaternion.Lerp(transform.rotation, camera.transform.rotation, usedRotationSp * Time.deltaTime);
            q.z = transform.rotation.z;
            q.x = transform.rotation.x;
            transform.rotation = q;
        }

    }

}
