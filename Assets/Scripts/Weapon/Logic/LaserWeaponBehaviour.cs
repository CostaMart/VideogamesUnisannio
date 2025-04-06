using UnityEngine;

[CreateAssetMenu(fileName = "LaserWeaponBehaviour", menuName = "Scriptable Objects/weaponLogic/LaserWeaponBehaviour")]
public class LaserWeaponBehaviour : AbstractWeaponLogic
{
    private bool shooting = false;
    public override void Disable()
    {
        weaponStat.controlEventManager.RemoveListenerReload(Reload);
        weaponStat.inputSys.actions["Attack"].performed -= context => { shooting = true; };
        weaponStat.inputSys.actions["Attack"].canceled -= context => { shooting = false; };
    }

    public override void Enable()
    {
        weaponStat.controlEventManager.AddListenerReload(Reload);
        weaponStat.inputSys.actions["Attack"].performed += context => { shooting = true; };
        weaponStat.inputSys.actions["Attack"].canceled += context => { shooting = false; };
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override void Updating()
    {
        throw new System.NotImplementedException();
    }
}
