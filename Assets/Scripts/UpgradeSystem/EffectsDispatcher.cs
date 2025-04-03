using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Weapon.State;
using static ItemManager;

/// <summary>
/// This component is responsible for dispatching the effects to the correct classes, and serves as a
/// bridge between the upgrades and all the gameobject components which could be useful to implement effects
/// 
/// this class shall manage overTime effects activation too
/// </summary>
public class EffectsDispatcher : MonoBehaviour
{

    [SerializeField] Dictionary<int, AbstractStatus> affectables = new Dictionary<int, AbstractStatus>();
    [SerializeField] private ControlEventManager controlEventManager;
    private List<AbstractEffect> toExternalDispatch = new List<AbstractEffect>();


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemDispatch(collision.gameObject.GetComponent<ItemMono>().item);
        }
    }
    void Start()
    {
        new ItemManager();
        FindComponentsInChildren<AbstractStatus>(transform);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && transform.name == "PBRCharacter")
        {
            ItemDispatch(ItemManager.bulletPool[3]);
        }
    }


    /// <summary>
    /// This method is called when an item is picked up by the player
    /// <paramref name="it"/> the item picked up
    /// </summary>
    public void ItemDispatch(Item it)
    {
        Debug.Log("Dispatching bullet effect " + it.ToString());

        if (it.name.Contains("Bullet"))
        {
            foreach (var effect in it.effects)
            {
                effect.localParametersRefClasses = attachReferences(effect.localParametersRef);
            }

            WeaponState b = (WeaponState)affectables[4];
            b.bulletEffects = it;
            return;
        }

        foreach (AbstractEffect up in it.effects)
        {
            Debug.Log("Dispatching effect " + up.ToString());
            up.localParametersRefClasses = attachReferences(up.localParametersRef);

            if (up.localTargetClassID == -1)
            {
                toExternalDispatch.Add(up);
                continue;
            }

            up.Attach(affectables[up.localTargetClassID], this);
        }
    }

    public void ItemDispatchFromExternalSource(Item it)
    {
        foreach (AbstractEffect up in it.effects)
        {
            Debug.Log("Dispatching external effect " + up.ToString());

            up.externParametersRefClasses = attachReferences(up.externParametersRef);
            up.Attach(affectables[up.externalTargetClassID], this);

        }
    }


    /// <summary>
    /// If a member of effect class has a reference to an attribute in a status class, this method is called to resolve the current value of such reference
    /// <paramref name="calssID"/> the ID of the class to reference
    /// <paramref name="attributeID"/> the ID of the attribute to reference
    /// </summary>
    public AbstractStatus[] attachReferences(int[][] references)
    {
        AbstractStatus[] toret = new AbstractStatus[references.Length];
        int x = 0;

        Debug.Log("following the list of references to resolve");
        foreach (var refere in references)
            Debug.Log("id :" + refere[0]);

        foreach (var refere in references)
        {
            try
            {
                toret[x] = affectables[refere[0]];
                x++;
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError("Class ID " + refere[0] + " not found in the dispatcher of object " + transform.name + " with ID: " + refere[1]);
                x++;
            }
            catch (Exception e)
            {
                Debug.LogError("Error in resolving value: " + e.Message);
            }

        }

        return toret;
    }


    /// <summary>
    /// search for all the components of type T in the hierarchy of the parent transform
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    private void FindComponentsInChildren<T>(Transform parent) where T : AbstractStatus
    {
        var components = parent.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component is T upgradable)
            {
                try
                {
                    Debug.Log("Found component added in affectables of type " + upgradable.GetType().Name + " in object: " + transform.gameObject.name + "in object" + parent.name + "with ID: " + upgradable.ID);
                    affectables.Add(upgradable.ID, upgradable);
                }
                catch (ArgumentException e)
                {
                    if (upgradable.GetType().Name == "WeaponState")
                    {
                        Debug.LogError("This character has two Weapon of the same type (primary or secondary), please check gameobject: " + transform.gameObject.name + "in object" + parent.name);
                    }
                    else
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }
        }

        foreach (Transform child in parent)
        {
            FindComponentsInChildren<T>(child);  // Chiamata ricorsiva per ogni figlio
        }
    }

}

