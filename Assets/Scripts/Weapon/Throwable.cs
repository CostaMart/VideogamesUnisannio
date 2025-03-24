using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour
{
    private Rigidbody rb;
    public float explosionRadius = 5;

    public float explosionForce = 1000;
    public float verticalFactor = 3;
    public int countdown = 3;
    private bool exploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {

        if (!exploded)
        {
            exploded = true;
            Collider[] c = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider item in c)
            {
                if (gameObject != item.gameObject)
                {
                    Rigidbody b = item.GetComponent<Rigidbody>();
                    if (b)
                    {
                        b.AddExplosionForce(explosionForce, transform.position, explosionRadius, verticalFactor, ForceMode.Impulse);
                    }
                }
            }
        }

    }

    public void Reload()
    {
        exploded = false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Disegna una sfera al centro dell'oggetto con il raggio specificato
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}


