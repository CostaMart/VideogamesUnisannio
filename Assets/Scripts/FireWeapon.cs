using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Queue<(GameObject, Rigidbody, Collider)> bulletPool = new Queue<(GameObject, Rigidbody, Collider)>();

    private Queue<(GameObject, Rigidbody, Collider)> used = new Queue<(GameObject, Rigidbody, Collider)>();
    private Queue<Queue<(GameObject, Rigidbody, Collider)>> mags = new Queue<Queue<(GameObject, Rigidbody, Collider)>>();

    public PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction reloadAction;

    public int magCount;
    public int magSize;
    private float lastShotTime;
    public float fireRate;
    private float fireLatency;
    private bool animatorSet = false;

    [Tooltip("if animator is provided recharging synchronizes with 'Reload' animation")]
    public Animator anim;

    // prepara una bolletpool di dimensione magSize
    void Start()
    {
        // setup input actions
        fireAction = playerInput.actions["Attack"];
        fireAction.performed += ctx => { Shoot(); };
        reloadAction = playerInput.actions["Reload"];
        reloadAction.performed += ctx => { Reoload(); };

        animatorSet = anim != null;

        fireLatency = 1 / fireRate;

        for (int j = 0; j < magCount; j++)
        {
            Queue<(GameObject, Rigidbody, Collider)> mag = new Queue<(GameObject, Rigidbody, Collider)>();

            for (int i = 0; i < magSize; i++)
            {
                GameObject b = Instantiate(bulletPrefab);
                Collider c = b.GetComponent<Collider>();
                Rigidbody r = b.GetComponent<Rigidbody>();
                mag.Enqueue((b, r, c));
                c.providesContacts = false;
                c.enabled = false;
                r.isKinematic = true;
                b.transform.position = bulletPrefab.transform.position;
            }
            mags.Enqueue(mag);
        }
    }

    void Shoot()
    {
        // can't shoot while reloading
        if (animatorSet && anim.GetCurrentAnimatorStateInfo(1).IsName("Reload")) return;

        // can't shoot if not enough time has passed since last shot
        if (Time.time - lastShotTime < fireLatency) return;

        // can't shoot if no bullets are available
        if (bulletPool.Count == 0) return;

        (GameObject bulletPrefab, Rigidbody rb, Collider c) = bulletPool.Dequeue();
        rb.isKinematic = false;
        c.enabled = true;
        c.providesContacts = true;
        bulletPrefab.transform.position = transform.position;
        Vector3 direction = transform.forward;
        direction = direction.normalized;
        rb.linearVelocity = direction * 100;
        lastShotTime = Time.time;
        used.Enqueue((bulletPrefab, rb, c));
    }

    void Reoload()
    {
        if (mags.Count == 0) return;
        if (animatorSet && anim.GetCurrentAnimatorStateInfo(1).IsName("Reload")) return;
        bulletPool = mags.Dequeue();
    }
}
