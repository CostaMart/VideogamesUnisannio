using System;
using UnityEngine;

[System.Serializable]
public class CharStats : AbstractStatus
{

    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private float life = 100;
    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] public float jumpSpeedy = 5f;
    [SerializeField] public float jumpSpeedx = 5f;
    [SerializeField] public float jumpSpeedz = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedLimitBeforeRagdolling = 20f;
    [SerializeField] private int maxJumps = 1;

    public float Life => life;
    public float RotationSpeed => rotationSpeed;
    public float AimRotationSpeed => aimRotationSpeed;
    public float MoveSpeed => moveSpeed;
    public int MaxJumps => maxJumps;
    public float SpeedLimitBeforeRagdolling => speedLimitBeforeRagdolling;

    private Action<float>[] updatabales;

    protected override int ComputeID()
    {
        return ItemManager.statClassToIdRegistry[this.GetType().Name];
    }

    new void Update()
    {
        base.Update();
        
        if (life <= 0)
        {
            Debug.Log("im dead");
            this.gameObject.SetActive(false);
        }
    }
}
