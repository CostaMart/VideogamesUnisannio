using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapon.State
{
    public class FireArmLogic : MonoBehaviour
    {
        [SerializeField] private WeaponState weaponStat;
        [SerializeField] private PlayerInput inputSys;
        [SerializeField] private bool shooting = false;

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
            if (weaponStat.automatic)
            {
                Debug.Log("set as auto weapon");
                inputSys.actions["Attack"].performed += context => {this.shooting = true; };
                inputSys.actions["Attack"].canceled += context => { this.shooting = false; };
            }
            else
            {
                inputSys.actions["Attack"].performed += context => { Shoot(); };
            }


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
            if (shootingIndex != 0 && shootingIndex % weaponStat.magSize == 0)
            {
                return;
            }


            GameObject bullet = weaponStat.bulletPool[shootingIndex];
            bullet.SetActive(true);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
            weaponStat.bulletRigids[shootingIndex].linearVelocity = muzzle.forward * weaponStat.fireStrength;

            shootingIndex = (shootingIndex + 1) % weaponStat.bulletPool.Length;
            lastShotTime = Time.time;
        }
        
        public LineRenderer lineRenderer;
        public float maxDistance = 100f;
        public LayerMask hitLayers; 
        void Update()
        {
            if (shooting)
                Shoot(); 
            
            Vector3 start = muzzle.position;
            Vector3 direction = muzzle.forward;

            Ray ray = new Ray(start, direction);
            RaycastHit hit;

            Vector3 end;
            if (Physics.Raycast(ray, out hit, maxDistance, hitLayers))
            {
                end = hit.point;
            }
            else
            {
                end = start + direction * maxDistance;
            }

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
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