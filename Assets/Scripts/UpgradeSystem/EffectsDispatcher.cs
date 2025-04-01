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

    private List<AbstractEffect> activeOvertime = new List<AbstractEffect>();
    private List<AbstractEffect> readyForRemoval = new List<AbstractEffect>();

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
    /// <paramref name="it"/> the item picked up
    /// </summary>
    public void ItemDispatch(Item it)
    {
        foreach (AbstractEffect up in it.effects)
        {
            up.Activate(affectables[up.targetClassID], this);
        }
    }

    /// <summary>
    /// Used to add an effect to the list of active over time effects
    /// <paramref name="effect"/> the effect to add
    /// </summary>
    public void AddToOvertimeList(AbstractEffect effect)
    {
        activeOvertime.Add(effect);
    }

    /// <summary>
    /// Used to remove an effect from the list of active over time effects
    /// <paramref name="effect"/> the effect to remove
    /// </summary>
    public void RemoveFromOvertimeList(AbstractEffect effect)
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
