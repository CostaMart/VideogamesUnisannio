using TMPro;
using UnityEngine;

public class WeaponState : AbstractStatus
{

    [SerializeField] private float baseMagSize;

    [SerializeField] private float baseFireRate;

    [SerializeField] private bool isPrimary = true; // 0 = primary, 1 = secondary

    public float magSize;
    public float fireRate;


    public float FireRate => fireRate;



    void Start()

    {

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override int ComputeID()
    {
        if (isPrimary)
            return ItemManager.statClassToIdRegistry["PrimaryWeaponState"];
        else
        {
            Debug.Log("Secondary weapon state");
            return ItemManager.statClassToIdRegistry["SecondaryWeaponState"];
        }
    }
}
