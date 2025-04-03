
using UnityEngine;
using static ItemManager;

namespace Weapon.State
{
    public class BulletState : AbstractStatus
    {
        private Rigidbody rb;

        [SerializeField] public float baseMass = 2;
        [SerializeField] public float explosionRadius = 2;
        [SerializeField] public bool destroyOnHit = true;
        public Item bulletEffets;

        public float directionx;
        public float directiony;
        public float directionz;
        public float widthScale = 1f;
        public float heightScale = 1f;
        public float lengthScale = 1f;

        new void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }

        new void Update()
        {

            base.Update();

            directionx = transform.forward.x;
            directiony = transform.forward.y;
            directionz = transform.forward.z;
            transform.localScale = new Vector3(widthScale, heightScale, lengthScale);
            rb.mass = baseMass;
        }

        protected override int ComputeID()
        {
            return ItemManager.statClassToIdRegistry[this.GetType().Name];
        }
    }
}