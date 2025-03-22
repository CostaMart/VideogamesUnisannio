using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharMovementLogic))]
public class PlayerControlManager : MonoBehaviour
{

    // input actions

    public PlayerInput playerInput;

    private InputAction aim;
    private InputAction move;
    private InputAction reload;
    private InputAction jump;
    private InputAction wpn1;
    private InputAction wpn2;
    private CharMovementLogic charMovementLogic;

    // fight controls
    private InputAction fireAction;
    private InputAction reloadAction;


    [SerializeField] private ControlEventManager ControlEventManager;
    [SerializeField] private EquipmentEventManager EquipmentEventManager;
    // ------------------------------------
    void Awake()
    {

        charMovementLogic = GetComponent<CharMovementLogic>();

        if (charMovementLogic == null) return;

        move = playerInput.actions["Move"];
        aim = playerInput.actions["Aim"];
        reload = playerInput.actions["Reload"];
        jump = playerInput.actions["Jump"];
        wpn1 = playerInput.actions["Wpn1"];
        wpn2 = playerInput.actions["Wpn2"];

        // movement controls
        aim.performed += ctx => { charMovementLogic.Aiming = true; };
        aim.performed += ctx => { ControlEventManager.raiseAimingEvent(true); };
        aim.canceled += ctx => { ControlEventManager.raiseAimingEvent(false); };
        aim.canceled += ctx => { charMovementLogic.Aiming = false; };

        jump.performed += ctx => { ControlEventManager.raiseJumpEvent(); };
        move.performed += ctx => { ControlEventManager.raiseMoveEvent(ctx.ReadValue<Vector2>()); };
        move.canceled += ctx => { ControlEventManager.raiseMoveEvent(Vector2.zero); };

        // weapon controls
        wpn1.performed += ctx => { EquipmentEventManager.RaiseWeaponSelected(1); };
        wpn2.performed += ctx => { EquipmentEventManager.RaiseWeaponSelected(2); };

        // fight controls
        reloadAction = playerInput.actions["Reload"];
    }
}