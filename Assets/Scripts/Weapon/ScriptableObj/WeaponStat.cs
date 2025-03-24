using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "Scriptable Objects/WeaponStat")]
public class WeaponStat : ScriptableObject
{
    [SerializeField] private int magSize;
    [SerializeField] private float fireRate;
    public int MagSize => magSize;
    public float FireRate => fireRate;
}
