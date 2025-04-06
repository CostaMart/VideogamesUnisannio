using System.Net;
using UnityEngine;

[CreateAssetMenu(fileName = "FireWeaponBehaviour", menuName = "Scriptable Objects/weaponLogics/FireWeaponBehaviour")]
public class FireWeaponBehaviour : AbstractWeaponLogic
{
    public bool shooting = false;

    [Tooltip("if animator is provided recharging synchronizes with 'Reload' animation")]
    public Animator anim;
    public bool animatorSet = false;
    public float lastShotTime;
    public int shootingIndex = 0;


    public override void Disable()
    {
        weaponStat.controlEventManager.RemoveListenerReload(Reload);
        weaponStat.inputSys.actions["Attack"].performed -= context => { this.shooting = true; };
        weaponStat.inputSys.actions["Attack"].canceled -= context => { this.shooting = false; };
    }

    public override void Enable()
    {
        lastShotTime = 0;
        shootingIndex = 0;
        weaponStat.controlEventManager.AddListenerReload(Reload);
        weaponStat.inputSys.actions["Attack"].performed += context => { this.shooting = true; };
        weaponStat.inputSys.actions["Attack"].canceled += context => { this.shooting = false; };
    }

    public override void Updating()
    {
        if (weaponStat.bulletPool.Length < weaponStat.magCount * weaponStat.magSize)
        {
            if (weaponStat.bulletPool != null)
            {
                var newPool = new GameObject[weaponStat.magCount * weaponStat.magSize];
                int i = 0;

                foreach (GameObject bullet in weaponStat.bulletPool)
                {
                    newPool[i] = bullet;
                    i++;
                }

                weaponStat.bulletPool = newPool;
                weaponStat.bulletRigids = new Rigidbody[newPool.Length];

                for (int index = i; index < weaponStat.bulletPool.Length; index++)
                {
                    weaponStat.bulletPool[index] = Instantiate(weaponStat.bulletPrefab, weaponStat.pool.transform);
                    weaponStat.bulletPool[index].GetComponent<Bullet>().bulletPoolState = weaponStat.pool;
                    weaponStat.bulletRigids[index] = weaponStat.bulletPool[index].GetComponent<Rigidbody>();
                    weaponStat.bulletPool[index].transform.position = weaponStat.pool.transform.position;
                    weaponStat.bulletPool[index].SetActive(false);
                }
            }
        }

        if (shooting)
            Shoot();

        DrawLaser();
    }


    public override void Shoot()
    {
        // can't shoot while reloading
        if (animatorSet && anim.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
        {
            return;
        }

        // can't shoot if not enough time has passed since last shot
        if (Time.time - lastShotTime < 1 / weaponStat.fireRate) return;

        // can't shoot if no bullets are available
        if (shootingIndex != 0 && shootingIndex % weaponStat.magSize == 0)
        {
            return;
        }


        GameObject bullet = weaponStat.bulletPool[shootingIndex];
        bullet.SetActive(true);
        bullet.transform.position = weaponStat.muzzle.position;
        bullet.transform.rotation = weaponStat.muzzle.rotation;
        weaponStat.bulletRigids[shootingIndex].linearVelocity = weaponStat.muzzle.forward * weaponStat.fireStrength;

        shootingIndex = (shootingIndex + 1) % weaponStat.bulletPool.Length;
        lastShotTime = Time.time;


        if (!weaponStat.automatic)
        {
            shooting = false;
        }
    }

    public override void Reload()
    {
        if (weaponStat.magCount > 0)
        {
            if (animatorSet)
            {
                anim.SetTrigger("Reload");
            }

            lastShotTime = 0;
            weaponStat.magCount--;
            shootingIndex = 0;
        }
    }
    void DrawLaser()
    {
        // Ottieni la posizione e la direzione del laser (dalla posizione del muzzle)
        Vector3 origineLaser = weaponStat.muzzle.position;
        Vector3 direzioneLaser = weaponStat.muzzle.forward; // La direzione della bocca dell'arma

        // Crea un raggio (Ray) che parte dal muzzle e si estende
        Ray ray = new Ray(origineLaser, direzioneLaser);

        RaycastHit hit;
        // Se il raggio colpisce qualcosa, usa la posizione di impatto, altrimenti usa la lunghezza massima
        if (Physics.Raycast(ray, out hit, weaponStat.laserLength, weaponStat.laserMask))
        {
            weaponStat.lineRenderer.SetPosition(0, origineLaser);         // Punto di partenza (muzzle)
            weaponStat.lineRenderer.SetPosition(1, hit.point);            // Punto di impatto
        }
        else
        {
            weaponStat.lineRenderer.SetPosition(0, origineLaser);         // Punto di partenza (muzzle)
            weaponStat.lineRenderer.SetPosition(1, origineLaser + direzioneLaser * weaponStat.laserLength); // Lunghezza massima del laser
        }
    }

}
