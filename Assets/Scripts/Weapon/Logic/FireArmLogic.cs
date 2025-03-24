using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework.Constraints;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapon.State
{
    public class FireArmLogic : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;

        // modo molto barbino di gestirlo al momento
        private Queue<(GameObject, Rigidbody, Collider)> bulletPool = new Queue<(GameObject, Rigidbody, Collider)>();
        private Queue<(GameObject, Rigidbody, Collider)> used = new Queue<(GameObject, Rigidbody, Collider)>();
        private Queue<Queue<(GameObject, Rigidbody, Collider)>> mags = new Queue<Queue<(GameObject, Rigidbody, Collider)>>();
        public GameObject pool;

        [Tooltip("if animator is provided recharging synchronizes with 'Reload' animation")]
        public Animator anim;

        // the bullet will be fired from this muzzle position and go in the direction of the transform.forward
        [Tooltip("the bullet will be fired from this muzzle position and go in the direction it is pointing")]
        [SerializeField] private Transform muzzle;
        [SerializeField] private int magCount;
        [SerializeField] private WeaponStat weaponStat;
        [SerializeField] private ControlEventManager controlEventManager;

        private float fireLatency;
        private bool animatorSet = false;
        private float lastShotTime;

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

            fireLatency = 1 / weaponStat.FireRate;

            for (int j = 0; j < magCount; j++)
            {
                Queue<(GameObject, Rigidbody, Collider)> mag = new Queue<(GameObject, Rigidbody, Collider)>();

                for (int i = 0; i < weaponStat.MagSize; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab);

                    bullet.transform.SetParent(pool.transform);
                    Collider c = bullet.GetComponent<Collider>();
                    Rigidbody r = bullet.GetComponent<Rigidbody>();
                    mag.Enqueue((bullet, r, c));

                    bullet.SetActive(false);
                    bullet.transform.position = bulletPrefab.transform.position;

                }
                mags.Enqueue(mag);
            }


        }


        public void Shoot()
        {


            // can't shoot while reloading
            if (animatorSet && anim.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
            {
                return;
            }

            // can't shoot if not enough time has passed since last shot
            if (Time.time - lastShotTime < fireLatency) return;

            // can't shoot if no bullets are available
            if (bulletPool.Count == 0)
            {
                return;
            }

            (GameObject bullet, Rigidbody rb, Collider c) = bulletPool.Dequeue();

            bullet.SetActive(true);
            bullet.transform.position = muzzle.position;
            Vector3 direction = muzzle.forward;
            direction = direction.normalized;

            // shooting here
            rb.linearVelocity = direction * 100;
            lastShotTime = Time.time;
            used.Enqueue((bullet, rb, c));
        }
        void Update()
        {

            Debug.DrawLine(transform.position, transform.position + muzzle.forward * 100, Color.blue);
        }

        void Reload()
        {
            Debug.Log("Remaining mags: " + mags.Count + ", remaining bullets: " + bulletPool.Count);
            if (mags.Count == 0) return;
            bulletPool = mags.Dequeue();
        }
    }
}