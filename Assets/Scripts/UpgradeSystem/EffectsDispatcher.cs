using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
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


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            this.ItemDispatch(collision.gameObject.GetComponent<ItemMono>().item);
            collision.gameObject.SetActive(false);
        }
    }
    void Awake()
    {
        new ItemManager();
        FindComponentsInChildren<AbstractStatus>(transform);

    }


    /// <summary>
    /// This method is called when an item is picked up by the player
    /// <paramref name="it"/> the item picked up
    /// </summary>
    public void ItemDispatch(Item it)
    {
        foreach (AbstractEffect up in it.effects)
        {
            up.Attach(affectables[up.targetClassID], this);
        }
    }


    /// <summary>
    /// If a member of effect class has a reference to an attribute in a status class, this method is called to resolve the current value of such reference
    /// <paramref name="calssID"/> the ID of the class to reference
    /// <paramref name="attributeID"/> the ID of the attribute to reference
    /// </summary>
    public float[] ResolveValue(int[][] references)
    {
        float[] toret = new float[references.Length];
        int x = 0;


        foreach (var refere in references)
        {
            var referencedClass = affectables[refere[0]];

            Debug.Log("referenced class ID: " + refere[0]);
            Debug.Log("referenced class: " + referencedClass.GetType().Name);

            float referencedAttributeVal = referencedClass.GetStatByID(refere[1]);
            toret[x] = referencedAttributeVal;
            x++;
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
                affectables.Add(upgradable.ID, upgradable);
            }
        }

        foreach (Transform child in parent)
        {
            FindComponentsInChildren<T>(child);  // Chiamata ricorsiva per ogni figlio
        }

    }

}
