using System;
using UnityEngine;

[System.Serializable]
public class CharStats : AbstractStatus
{

    private delegate float Command(float op1, float op2);
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float life = 100;
    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedLimitBeforeRagdolling = 20f;
    [SerializeField] private int maxJumps = 1;

    public float Life => life;
    public float RotationSpeed => rotationSpeed;
    public float AimRotationSpeed => aimRotationSpeed;
    public float JumpSpeed => jumpSpeed;
    public float MoveSpeed => moveSpeed;
    public float AnotherJumpSpeed => JumpSpeed;
    public int MaxJumps => maxJumps;
    public float SpeedLimitBeforeRagdolling => speedLimitBeforeRagdolling;

    private Action<float>[] updatabales;




}
