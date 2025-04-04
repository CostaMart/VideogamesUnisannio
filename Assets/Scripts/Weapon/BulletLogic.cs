using System;
using System.Data;
using UnityEngine;
using Weapon.State;
using static ItemManager;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 initialPos;
    private Rigidbody rb;
    private Collider c;


    [SerializeField] private BulletState bulletState;
    private float EnableTime;


    void Awake()
    {
        c = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        bulletState = transform.GetComponent<BulletState>();
        initialPos = transform.position;
    }

    void Update()
    {
        if (rb.linearVelocity != Vector3.zero)
        {
            EnableTime += Time.deltaTime;
            if (EnableTime > 5)
            {
                EnableTime = 0;
                resetItem();
            }
        }
    }

    // Update is called once per frame
    // quando avviene una collisione il proiettile torna al luogo di origine, disattivando la fisica in modo tale da non dare fastidio.
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet colliding with " + collision.gameObject.name);
        Collider[] colliders = Physics.OverlapSphere(collision.transform.position, bulletState.explosionRadius);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<EffectsDispatcher>(out var d))
            {
                try
                {
                    Debug.Log("Dispatching effects to " + col.gameObject.name);
                    d.DispatchFromOtherDispatcher(bulletState.bulletEffets);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        if (bulletState.destroyOnHit)
            resetItem();
    }


    void resetItem()
    {
        rb.linearVelocity = Vector3.zero; // Azzeriamo la velocità lineare
        rb.angularVelocity = Vector3.zero; // Azzeriamo la velocità angolare
        transform.position = initialPos; // Riportiamo il proiettile alla posizione iniziale
        this.gameObject.SetActive(false); // Disattiviamo il proiettile
    }
}