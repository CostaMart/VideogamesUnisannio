
using UnityEngine;

namespace Weapon.State
{
    public class BulletState : AbstractStatus
    {
        [SerializeField] public float baseDamage;
        [SerializeField] public float baseWeight;
        [SerializeField] public float explosionRadius;


        protected override int ComputeID()
        {
            return ItemManager.statClassToIdRegistry[this.GetType().Name];
        }
    }
}