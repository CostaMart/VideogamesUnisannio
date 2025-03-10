using System.Data;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 initialPos;
    private Rigidbody rb;
    private Collider c;
    private float EnableTime;
    void Awake()
    {
        c = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
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
        resetItem();
    }

    void resetItem()
    {
        Debug.Log("Resetting bullet");
        c.enabled = false;
        rb.isKinematic = true;
        c.providesContacts = false;
        rb.linearVelocity = Vector3.zero; // Azzeriamo la velocità lineare
        rb.angularVelocity = Vector3.zero; // Azzeriamo la velocità angolare
        rb.Sleep(); // Disattiviamo il rigidbody
        transform.position = initialPos; // Riportiamo il proiettile alla posizione iniziale
    }
}