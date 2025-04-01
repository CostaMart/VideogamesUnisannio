
using Unity.Cinemachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class PhysicalState : AbstractStatus
{
    public float mass;

    public bool isaffectedByGravity = true;

    public float forcey = -1f;
    public float forcex = -1;
    public float forcez = -1f;
    public float linearDumping = 2f;

    private Rigidbody rb;

    protected override int ComputeID()
    {
        var ret = ItemManager.statClassToIdRegistry[this.GetType().Name];
        Debug.Log("ID of PhysicalState: " + ret);
        return ret;
    }

    new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }
    new void Update()
    {

        base.Update();

        // update the rigidbody mass
        rb.mass = mass;
        rb.useGravity = isaffectedByGravity;
        rb.linearDamping = linearDumping;


        if (forcey != -1f)
        {
            rb.AddForce(new Vector3(0, forcey, 0), ForceMode.Force);
        }
        if (forcex != -1f)
        {
            rb.AddForce(new Vector3(forcex, 0, 0), ForceMode.Force);
        }
        if (forcez != -1f)
        {
            rb.AddForce(new Vector3(0, 0, forcez), ForceMode.Force);
        }

    }
}