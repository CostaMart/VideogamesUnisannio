using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CharStats : MonoBehaviour, IUpgradable
{
    public int ID => 0;

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

    private Action<Upgrade>[] updatabales;

    void Start()
    {
        updatabales = new Action<Upgrade>[]{
        (up) => { rotationSpeed =  up.operation(rotationSpeed); },
        (up) => { life = up.operation(life); },
        (up) => { aimRotationSpeed = up.operation(aimRotationSpeed); },
        (up) => { jumpSpeed = up.operation(jumpSpeed); },
        (up) => { moveSpeed = up.operation(moveSpeed); },
        (up) => { jumpSpeed = up.operation(jumpSpeed); },
        (up) => { maxJumps =  (int) up.operation(maxJumps); },
        (up) => { speedLimitBeforeRagdolling = up.operation(speedLimitBeforeRagdolling); }
    };

    }
    public void Upgrade(Upgrade upgrade)
    {
        updatabales[upgrade.attributeTarget](upgrade);
        {
            /*
            float op1;
            switch (upgrade.attributeTarget)
            {
                case (int)UpgradeType.RotationSpeed:
                    op1 = rotationSpeed;
                    rotationSpeed = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.Life:
                    op1 = life;
                    life = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.AimRotationSpeed:
                    op1 = aimRotationSpeed;
                    aimRotationSpeed = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.JumpSpeed:
                    op1 = jumpSpeed;
                    jumpSpeed = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.MoveSpeed:
                    op1 = moveSpeed;
                    moveSpeed = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.AnotherJumpSpeed:
                    op1 = jumpSpeed;
                    jumpSpeed = upgrade.operation(op1);
                    break;
                case (int)UpgradeType.MaxJumps:
                    op1 = maxJumps;
                    maxJumps = (int)upgrade.operation(op1);
                    break;
                case (int)UpgradeType.SpeedLimitBeforeRagdolling:
                    op1 = speedLimitBeforeRagdolling;
                    speedLimitBeforeRagdolling = upgrade.operation(op1);
                    break;
                default:
                    op1 = 0;
                    break;

            }
            */
        }
    }

    private enum UpgradeType
    {
        RotationSpeed = 0,
        Life = 1,
        AimRotationSpeed = 2,
        JumpSpeed = 3,
        MoveSpeed = 4,
        AnotherJumpSpeed = 5,
        MaxJumps = 6,
        SpeedLimitBeforeRagdolling

    }
}
