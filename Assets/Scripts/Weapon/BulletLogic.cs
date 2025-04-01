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

    Item bulletEffets = ItemManager.bulletPool[0]; // TODO: questo è un prototipo, in futuro dovrà essere passato come parametro

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

    public Vector3 gizmopos;

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
                    d.ItemDispatch(bulletEffets);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }

        resetItem();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmopos, bulletState.explosionRadius);
    }

    void resetItem()
    {
        Debug.Log("Resetting bullet");
        rb.linearVelocity = Vector3.zero; // Azzeriamo la velocità lineare
        rb.angularVelocity = Vector3.zero; // Azzeriamo la velocità angolare
        transform.position = initialPos; // Riportiamo il proiettile alla posizione iniziale
        this.gameObject.SetActive(false); // Disattiviamo il proiettile
    }
}