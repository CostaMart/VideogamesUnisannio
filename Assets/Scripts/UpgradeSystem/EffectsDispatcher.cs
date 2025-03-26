using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is responsible for dispatching the effects to the correct classes, and serves as a
/// bridge between the upgrades and all the gameobject components which could be useful to implement effects
/// </summary>
public class EffectsDispatcher : MonoBehaviour
{

    [SerializeField] IAffectable[] affectables = new IAffectable[3];
    [SerializeField] private ControlEventManager controlEventManager;

    private List<SingleActivationIncrementEffect> activeOvertime = new List<SingleActivationIncrementEffect>();

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
            SingleActivationIncrementEffect up = new SingleActivationIncrementEffect();
            Item it = new Item();
            up.targetClassID = 0;
            up.targetAttributeID = 4;
            up.value = 143;
            it.effects.Add(up);
            OnItemPickUp(it);
        }
    }

    public void OnItemPickUp(Item it)
    {
        foreach (var up in it.effects)
        {
            if (up.referecedAttributeClass != -1)
                ResolveValue(up);

            up.ActivateEffect(affectables[up.targetClassID], this);
        }
    }

    /// <summary>
    /// if the effect has a reference to an attribute in a class , this method is called to resolve the value of the reference
    /// </summary>
    /// <param name="effect"></param>
    void ResolveValue(IEffect effect)
    {
        var referencedClass = affectables[effect.referecedAttributeClass];
        float referencedAttributeVal = referencedClass.ResolveParameterValueByID(effect.referencedAttribute);
        effect.value = referencedAttributeVal;
    }


    void FindComponentsInChildren<T>(Transform parent) where T : IAffectable
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
