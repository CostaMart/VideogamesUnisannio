using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Vector3 initialPos;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {

        rb.linearVelocity = Vector3.zero; // Azzeriamo la velocità lineare
        rb.angularVelocity = Vector3.zero; // Azzeriamo la velocità angolare

        // Rimuoviamo anche le forze applicate
        rb.Sleep();
        // Disabilitiamo il GameObject
        rb.isKinematic = false;
        rb.useGravity = false;

        rb.position = initialPos; // Riportiamo il proiettile alla posizione iniziale
    }
}
