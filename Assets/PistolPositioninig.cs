using System;
using Unity.Mathematics;
using UnityEngine;

public class PistolPositioninig : MonoBehaviour
{

    [SerializeField] private Transform _pistolPosition;
    [SerializeField] private float dumping;

    [SerializeField] private ControlEventManager cem;

    private bool aiming = false;


    void Start()
    {
        cem.AddListenerAiming((value) => { aiming = value; });
    }
    void Update()
    {
        if (aiming)
        {
            Vector3 bufPosition = transform.position;
            bufPosition.y = math.lerp(transform.position, _pistolPosition.position, Time.deltaTime * dumping).y;
            transform.position = bufPosition;
            transform.rotation = math.slerp(transform.rotation, _pistolPosition.rotation, Time.deltaTime * dumping);
        }
    }

    public bool Aiming
    {
        get => aiming;
        set => aiming = value;
    }
}
