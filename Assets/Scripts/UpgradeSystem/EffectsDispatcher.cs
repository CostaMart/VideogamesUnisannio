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

    private List<SingleActivationEffect> activeOvertime = new List<SingleActivationEffect>();

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
            AbstractEffect up = new SingleActivationEffect(0, 4, (value) => value + 1000);
            Item it = new Item();

            it.effects.Add(up);
            OnItemPickUp(it);
        }
    }

    public void OnItemPickUp(Item it)
    {
        foreach (AbstractEffect up in it.effects)
        {
            if (up.referecedAttributeClassID != null)
                up.newValue = ResolveValue(up.referecedAttributeClassID.Value, up.referencedAttributeID.Value);

            up.ActivateEffect(affectables[up.targetClassID], this);
        }
    }

    /// <summary>
    /// if the effect has a reference to an attribute in a class , this method is called to resolve the value of the reference
    /// </summary>
    /// <param name="effect"></param>
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
