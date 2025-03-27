using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is responsible for dispatching the effects to the correct classes, and serves as a
/// bridge between the upgrades and all the gameobject components which could be useful to implement effects
/// 
/// this class shall manage overTime effects activation too
/// </summary>
public class EffectsDispatcher : MonoBehaviour
{

    [SerializeField] IAffectable[] affectables = new IAffectable[3];
    [SerializeField] private ControlEventManager controlEventManager;

    private List<AbstractEffect> activeOvertime = new List<AbstractEffect>();
    private List<AbstractEffect> readyForRemoval = new List<AbstractEffect>();

    void Awake()
    {
        FindComponentsInChildren<IAffectable>(transform);
    }

    void Update()
    {
        // PROTO: solo per prototipazione, verranno eliminati 
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Dispatching upgrade");

            // dispatcher doesn't care of which type of effect it is activating
            AbstractEffect up = new OverTimeEffect(0, 4, (newValue, actualVal) => { float toreturn; toreturn = actualVal + newValue; Debug.Log("tick: increasing of" + newValue); return toreturn; }, 10, 0.5f, 10);

            Item it = new Item();

            it.effects.Add(up);
            OnItemPickUp(it);
        }
    }

    /// <summary>
    /// EffectsDispatcher assures that the effects in activeOverTime are activated in the correct order and at fixed intervals 
    /// </summary>
    void FixedUpdate()

    {
        // activate over time effects
        foreach (var effect in activeOvertime)
        {
            effect.Activate(affectables[effect.targetClassID], this);
        }

        CleanEffects();
    }

    /// <summary>
    /// This method is called when an item is picked up by the player
    /// </summary>
    /// <param name="it"></param>
    public void OnItemPickUp(Item it)
    {
        foreach (AbstractEffect up in it.effects)
        {
            if (up.referencedAttributeClassID != null)
                up.newValue = ResolveValue(up.referencedAttributeClassID.Value, up.referencedAttributeID.Value);

            up.Activate(affectables[up.targetClassID], this);
        }
    }

    /// <summary>
    /// Used to add an effect to the list of active over time effects
    /// </summary>
    /// <param name="effect"></param>
    public void AddToOvertime(AbstractEffect effect)
    {
        activeOvertime.Add(effect);
    }

    /// <summary>
    /// Used to remove an effect from the list of active over time effects
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveFromOvertime(AbstractEffect effect)
    {
        readyForRemoval.Add(effect);
    }

    /// <summary>
    /// Used internally to remove effects from the list safely
    /// </summary>
    private void CleanEffects()
    {
        activeOvertime.RemoveAll((effect) => readyForRemoval.Contains(effect));
        readyForRemoval.Clear();
    }

    /// <summary>
    /// if the effect has a reference to an attribute in a class , this method is called to resolve the value of the reference
    /// </summary>
    /// <param name="calssID"></param>
    /// <param name="attributeID"></param>
    private float ResolveValue(int classID, int attributeID)
    {
        var referencedClass = affectables[classID];
        float referencedAttributeVal = referencedClass.GetStatByID(attributeID);
        return referencedAttributeVal;
    }

    private void FindComponentsInChildren<T>(Transform parent) where T : IAffectable
    {
        var components = parent.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component is T upgradable)
            {
                Debug.Log("Found IUpgradable component: " + component.GetType().Name);
                affectables[upgradable.ID] = upgradable;
            }
        }

        foreach (Transform child in parent)
        {
            FindComponentsInChildren<T>(child);  // Chiamata ricorsiva per ogni figlio
        }
    }

}
