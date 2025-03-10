using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;

    private Queue<(GameObject, Rigidbody, Collider)> bulletPool = new Queue<(GameObject, Rigidbody, Collider)>();

    private Queue<(GameObject, Rigidbody, Collider)> used = new Queue<(GameObject, Rigidbody, Collider)>();
    private Queue<Queue<(GameObject, Rigidbody, Collider)>> mags = new Queue<Queue<(GameObject, Rigidbody, Collider)>>();

    public int magCount;
    public int magSize;

    // prepara una bolletpool di dimensione magSize
    void Start()
    {
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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reload");
            Reoload();
        }
    }

    void Shoot()
    {
        Debug.Log("bullets left: " + bulletPool.Count);
        if (bulletPool.Count == 0) return;
        (GameObject bulletPrefab, Rigidbody rb, Collider c) = bulletPool.Dequeue();
        rb.isKinematic = false;
        c.enabled = true;
        c.providesContacts = true;
        bulletPrefab.transform.position = transform.position;
        Vector3 direction = transform.forward;
        direction = direction.normalized;
        rb.linearVelocity = direction * 100;
        used.Enqueue((bulletPrefab, rb, c));
    }

    void Reoload()
    {
        if (mags.Count == 0) return;
        bulletPool = mags.Dequeue();
    }
}
