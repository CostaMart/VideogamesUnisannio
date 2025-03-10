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

    public int magSize;

    // prepara una bolletpool di dimensione magSize
    void Start()
    {
        for (int i = 0; i < magSize; i++)
        {

            GameObject b = Instantiate(bulletPrefab);
            Collider c = b.GetComponent<Collider>();
            c.providesContacts = false;
            Rigidbody r = b.GetComponent<Rigidbody>();
            bulletPool.Enqueue((b, r, c));
            c.enabled = false;
            r.isKinematic = true;
            b.transform.position = bulletPrefab.transform.position;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    void Shoot()
    {
        if (bulletPool.Count == 0) return;
        (GameObject bulletPrefab, Rigidbody rb, Collider c) = bulletPool.Dequeue();
        c.enabled = true;
        rb.isKinematic = false;
        c.providesContacts = true;
        bulletPrefab.transform.position = transform.position;
        Vector3 direction = transform.forward;
        direction = direction.normalized;
        rb.linearVelocity = direction * 100;
        used.Enqueue((bulletPrefab, rb, c));
    }
}
