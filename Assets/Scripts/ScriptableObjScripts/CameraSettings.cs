using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    private static CameraSettings _instance;

    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float aimingFov = 40f;
    [SerializeField] private float zoom = 1.5f;

    [SerializeField] private float defaultFov = 60f;

    public float Sensitivity => sensitivity;
    public float AimingFov => aimingFov;
    public float DefaultFov => defaultFov;
    public float Zoom => zoom;
}
