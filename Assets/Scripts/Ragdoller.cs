

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Ragdoller : MonoBehaviour
{
    Collider col;
    Rigidbody rb;
    Animator anim;
    GameObject ragdollReference;
    public UnityAction<bool, GameObject> onRagdolling;
    public float speedLimitBeforeRagdolling = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Ragdolling(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(rb.linearVelocity.y) > speedLimitBeforeRagdolling || Math.Abs(rb.linearVelocity.x) > speedLimitBeforeRagdolling || Math.Abs(rb.linearVelocity.z) > speedLimitBeforeRagdolling)
            Ragdolling(true);
    }

    /*Activate or deactivate ragdolling*/
    void Ragdolling(bool ragdolling)
    {

        Rigidbody[] r = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody otherRigid in r)
        {
            if (rb != otherRigid)
            {
                otherRigid.isKinematic = !ragdolling;
                otherRigid.linearVelocity = rb.linearVelocity;
                otherRigid.angularVelocity = rb.angularVelocity;

                // è necessario recuperare questa reference per posizionare il
                //  personaggio correttamente quando si ripristinerà dopo la ragdoll
                if (ragdollReference == null && otherRigid.transform != null)
                    ragdollReference = otherRigid.gameObject;
            }


            if (otherRigid == r.Last())
            {
                Debug.Log("Ragdolling: " + ragdolling);
                onRagdolling?.Invoke(ragdolling, otherRigid.gameObject);
            }

        }

        foreach (Collider otherCollid in GetComponentsInChildren<Collider>())
        {
            if (otherCollid != col)
            {
                otherCollid.enabled = ragdolling;
            }
        }

        if (!ragdolling)
        {
            transform.position = ragdollReference.transform.position;
        }

        anim.enabled = !ragdolling;
        col.enabled = !ragdolling;
        rb.isKinematic = ragdolling;
    }


}