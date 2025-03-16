using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TPSInventoryManager : MonoBehaviour
{
    public UnityEvent PrimaryWeapn;
    public UnityEvent SecondaryWeapn;
    public UnityEvent ThrowAction;

    enum ChoosenWeapn
    {
        Primary,
        Secondary,
        Throwable
    }
    private ChoosenWeapn currentWeapn;

    //-------------Input system connection -------
    public PlayerInput playerInput;
    private InputAction doAction;
    private InputAction switchWeapon;

    private InputAction wpn1;
    private InputAction wpn2;
    private InputAction thrw;
    //-------------------------------------------


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // input system setup
        doAction = playerInput.actions["Attack"];
        wpn1 = playerInput.actions["Wpn1"];
        wpn2 = playerInput.actions["Wpn2"];
        thrw = playerInput.actions["Throw"];
        wpn1.performed += ctx => { Debug.Log("selected Primary"); currentWeapn = ChoosenWeapn.Primary; };
        wpn2.performed += ctx => { Debug.Log("selected Secondary"); currentWeapn = ChoosenWeapn.Secondary; };
        thrw.performed += ctx => { Throw(); };
        doAction.performed += ctx => { PerformAction(); };
    }

    void PerformAction()
    {
        switch (currentWeapn)
        {
            case ChoosenWeapn.Primary:
                PrimaryWeapn.Invoke();
                break;
            case ChoosenWeapn.Secondary:
                if (SecondaryWeapn != null) return;
                SecondaryWeapn.Invoke();
                break;
            case ChoosenWeapn.Throwable:
                if (ThrowAction != null) return;
                ThrowAction.Invoke();
                break;
        }
    }

    void Throw()

    {
        if (ThrowAction != null) ThrowAction.Invoke();
    }





}
