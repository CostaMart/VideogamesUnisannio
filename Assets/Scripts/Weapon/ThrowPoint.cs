using UnityEngine;

public class ThrowPoint : MonoBehaviour
{
    public float throwForce = 600;

    public GameObject toThrow;
    private Rigidbody rb;

    private Throwable tb;
    void Start()
    {
        rb = toThrow.transform.GetComponent<Rigidbody>();
        tb = toThrow.GetComponent<Throwable>();
    }
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
    }
    public void Throw()
    {
        tb.Reload();
        toThrow.transform.position = transform.position;
        rb.AddForce(transform.forward * throwForce);
    }

}
