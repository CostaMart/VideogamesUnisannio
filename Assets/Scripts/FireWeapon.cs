using TMPro;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = bulletPrefab.GetComponent<Rigidbody>();
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
        rb.useGravity = true;
        rb.isKinematic = false;
        bulletPrefab.transform.position = transform.position;
        Vector3 direction = transform.forward;
        direction = direction.normalized;
        rb.linearVelocity = direction * 100;
    }
}
