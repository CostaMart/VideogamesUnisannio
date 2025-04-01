using UnityEngine;

public class WeaponState : AbstractStatus
{

    [SerializeField] private int baseMagSize;

    [SerializeField] private float baseFireRate;

    public int magSize;
    public float fireRate;

    public int MagSize => magSize;

    public float FireRate => fireRate;


    void Start()

    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
