using UnityEngine;

public abstract class AbstractWeaponLogic : ScriptableObject
{
    [SerializeField] public WeaponState weaponStat;

    public abstract void Enable();
    public abstract void Disable();
    public abstract void Shoot();
    public abstract void Updating();
    public abstract void Reload();
    public virtual void SetWeaponState(WeaponState weaponState)
    {
        weaponStat = weaponState;
    }

}
