
using System.Collections.Generic;
using UnityEngine;
using static ItemManager;

namespace Weapon.State
{
    public class BulletPoolState : AbstractStatus
    {
        private Rigidbody rb;
        private bool isPrimary = true;
        [SerializeField] public float baseMass = 2;
        [SerializeField] public float explosionRadius = 2;
        [SerializeField] public bool destroyOnHit = true;

        public Item bulletEffects;

        public float directionx;
        public float directiony;
        public float directionz;
        public float widthScale = 1f;
        public float heightScale = 1f;
        public float lengthScale = 1f;

        protected override void Awake()
        {
            base.Awake();
            bulletEffects = ItemManager.bulletPool[0];
        }

        protected override int ComputeID()
        {
            if (isPrimary)
            {
                return ItemManager.statClassToIdRegistry["BulletPoolStatePrimary"];
            }
            else
            {
                return ItemManager.statClassToIdRegistry["BulletPoolStateSecondary"];
            }
        }

    }
}