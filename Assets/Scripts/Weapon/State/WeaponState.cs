using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ItemManager;

public class WeaponState : AbstractStatus
{

    [SerializeField] private float baseMagSize;

    [SerializeField] private float baseFireRate;

    [SerializeField] private bool isPrimary = true; // 0 = primary, 1 = secondary

    public GameObject pool;

    public GameObject[] bulletPool;
    [SerializeField] private GameObject bulletPrefab;

    // the bullet will be fired from this muzzle position and go in the direction of the transform.forward
    [Tooltip("the bullet will be fired from this muzzle position and go in the direction it is pointing")]
    [SerializeField] public int magCount;
    public float magSize;
    public float fireRate;
    public float FireRate => fireRate;
    public bool reloading = false;
    public float fireStrength = 1f;

    public Item bulletEffects;

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (bulletPool.Length < magCount * (int)magSize)
        {
            if (bulletPool != null)
            {
                var newPool = new GameObject[magCount * (int)magSize];
                int i = 0;

                foreach (GameObject bullet in bulletPool)
                {
                    newPool[i] = bullet;
                    i++;
                }

                bulletPool = newPool;
                for (int index = i; index < bulletPool.Length; index++)
                {
                    bulletPool[index] = Instantiate(bulletPrefab, pool.transform);
                    bulletPool[index].transform.position = pool.transform.position;
                    bulletPool[index].SetActive(false);
                }
            }
        }

        if (fireRate < baseFireRate)
            fireRate = baseFireRate;
    }

    protected override int ComputeID()
    {
        if (isPrimary)
        {
            Debug.Log("Primary weapon state");
            return ItemManager.statClassToIdRegistry["PrimaryWeaponState"];
        }
        else
        {
            Debug.Log("Secondary weapon state");
            return ItemManager.statClassToIdRegistry["SecondaryWeaponState"];
        }
    }
}
