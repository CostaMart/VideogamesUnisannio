using System;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CharStats : MonoBehaviour, AbstractAffectable
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

    private Action<float>[] updatabales;

    void Start()
    {
        updatabales = new Action<float>[]{
        (newVal) => { rotationSpeed =    newVal; },
        (newVal) => { life =             newVal; },
        (newVal) => { aimRotationSpeed = newVal; },
        (newVal) => { jumpSpeed =        newVal; },
        (newVal) => { moveSpeed =        newVal; },
        (newVal) => { jumpSpeed =        newVal; },
        (newVal) => { maxJumps =   (int) newVal; },
        (newVal) => { speedLimitBeforeRagdolling = newVal; }
    };

    }
    public void SetStatByID(int targetAttribute, float upgrade)
    {
        updatabales[targetAttribute](upgrade);
    }

    public float GetStatByID(int id)
    {
        float ret;
        switch ((UpgradeType)id)
        {
            case UpgradeType.RotationSpeed:
                ret = rotationSpeed;
                break;
            case UpgradeType.Life:
                ret = life;
                break;
            case UpgradeType.AimRotationSpeed:
                ret = aimRotationSpeed;
                break;
            case UpgradeType.JumpSpeed:
                ret = jumpSpeed;
                break;
            case UpgradeType.MoveSpeed:
                ret = moveSpeed;
                break;
            case UpgradeType.AnotherJumpSpeed:
                ret = jumpSpeed;
                break;
            case UpgradeType.MaxJumps:
                ret = maxJumps;
                break;
            case UpgradeType.SpeedLimitBeforeRagdolling:
                ret = speedLimitBeforeRagdolling;
                break;
            default:
                ret = 0;
                break;
        }
        return ret;
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
