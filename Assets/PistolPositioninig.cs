using System;
using Unity.Mathematics;
using UnityEngine;

public class PistolPositioninig : MonoBehaviour
{

    [SerializeField] private Transform _pistolPosition;
    [SerializeField] private float dumping;

    [SerializeField] private ControlEventManager cem;
    private bool aiming = false;

    private Vector3 initialPosition;

    void Start()
    {
        cem.AddListenerAiming((value) => { aiming = value; });
    }
    void Update()
    {
        if (aiming)
        {

            transform.localPosition = Vector3.Lerp(transform.localPosition, _pistolPosition.localPosition, dumping * Time.deltaTime);


            // Mantieni la rotazione sull'asse Z della pistola
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, _pistolPosition.eulerAngles.z);
        }
    }

    public bool Aiming
    {
        get => aiming;
        set => aiming = value;
    }
}
