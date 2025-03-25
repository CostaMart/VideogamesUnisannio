using UnityEngine;

class testUpdate : MonoBehaviour, IUpgradable
{
    public int ID => 1;
    public void Upgrade(Upgrade upgrade)
    {
        Debug.Log("got a fantastic update of type " + upgrade.value);
    }
}
