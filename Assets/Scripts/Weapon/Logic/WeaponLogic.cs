using System.Collections.Generic;
using UnityEngine;

namespace Weapon.State
{
    public class FireArmLogic : MonoBehaviour
    {
        [SerializeField] private WeaponState weaponStat;

        [Tooltip("if animator is provided recharging synchronizes with 'Reload' animation")]
        public Animator anim;
        [SerializeField] private ControlEventManager controlEventManager;
        [SerializeField] private Transform muzzle;

        private bool animatorSet = false;
        private float lastShotTime;
        private int shootingIndex = 0;

        // prepara una bolletpool di dimensione magSize
        void OnEnable()
        {
            controlEventManager.AddListenerReload(Reload);
            controlEventManager.AddListenerAttack(Shoot);
        }

        void OnDisable()
        {
            controlEventManager.RemoveListenerReload(Reload);
            controlEventManager.RemoveListenerAttack(Shoot);
        }

        void Start()
        {
            animatorSet = anim != null;
        }


        public void Shoot()
        {


            // can't shoot while reloading
            if (animatorSet && anim.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
            {
                return;
            }

            // can't shoot if not enough time has passed since last shot
            if (Time.time - lastShotTime < 1 / weaponStat.fireRate) return;

            // can't shoot if no bullets are available
            if (shootingIndex == weaponStat.magSize - 1)
            {
                return;
            }


            GameObject bullet = weaponStat.bulletPool[shootingIndex];
            bullet.SetActive(true);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
            bullet.GetComponent<BulletState>().bulletEffets = weaponStat.bulletEffects;
            bullet.GetComponent<Rigidbody>().linearVelocity = muzzle.forward * weaponStat.fireStrength;

            shootingIndex = (int)((shootingIndex + 1) % weaponStat.magSize);
        }
        void Update()
        {

            Debug.DrawLine(transform.position, transform.position + muzzle.forward * 100, Color.blue);
        }

        void Reload()
        {
            if (weaponStat.magCount > 0)
            {
                if (animatorSet)
                {
                    anim.SetTrigger("Reload");
                }

                weaponStat.magCount--;
                shootingIndex = 0;
            }
        }
    }
}