using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float rotationSpeed = 0.5f;

    [SerializeField] private float aimRotationSpeed = 5f;

    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedLimitBeforeRagdolling = 20f;

    [SerializeField] private int maxJumps = 1;

    public float RotationSpeed => rotationSpeed;
    public float AimRotationSpeed => aimRotationSpeed;
    public float JumpSpeed => jumpSpeed;
    public float MoveSpeed => moveSpeed;
    public float AnotherJumpSpeed => JumpSpeed;
    public int MaxJumps => maxJumps;
    public float SpeedLimitBeforeRagdolling => speedLimitBeforeRagdolling;
}
