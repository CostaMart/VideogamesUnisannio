using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ControlEventManager", menuName = "Scriptable Objects/ControLEventManager")]
public class ControlEventManager : ScriptableObject
{

    private UnityAction<bool> OnAiming;
    private UnityAction OnJump;
    private UnityAction<Vector2> OnMove;
    private UnityAction OnFire;
    private UnityAction OnReload;
    private UnityAction<bool> Ragdoll;

    private UnityAction<Vector2> OnMouseControl;

    public void AddListenerAiming(UnityAction<bool> listener)
    {
        OnAiming += listener;
    }

    public void raiseAimingEvent(bool aiming)
    {
        OnAiming?.Invoke(aiming);
    }

    public void AddListenerJump(UnityAction listener)
    {
        OnJump += listener;
    }

    public void raiseJumpEvent()
    {
        OnJump?.Invoke();
    }

    public void AddListenerMove(UnityAction<Vector2> listener)
    {
        OnMove += listener;
    }

    public void raiseMoveEvent(Vector2 direction)
    {
        OnMove?.Invoke(direction);
    }

    public void AddListenerFire(UnityAction listener)
    {
        OnFire += listener;
    }

    public void raiseFireEvent()
    {
        OnFire?.Invoke();
    }

    public void AddRagdollListener(UnityAction<bool> listener)
    {
        Ragdoll += listener;
    }

    public void raiseRagdollEvent(bool value)
    {
        Ragdoll?.Invoke(value);
    }

    public void AddMouseControlListener(UnityAction<Vector2> listener)
    {
        OnMouseControl += listener;
    }

    public void raiseMouseControlEvent(Vector2 rotation)
    {
        OnMouseControl?.Invoke(rotation);
    }

}
