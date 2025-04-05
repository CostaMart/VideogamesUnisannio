using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A status class that wants to participate to the item upgrade system must implement this interface, 
/// the effect dispatcher will look for these classes in the gameobject hierarchy.
/// </summary>
public abstract class AbstractStatus : MonoBehaviour
{
    private FieldInfo[] fields;

    private List<AbstractEffect> activeEffects = new List<AbstractEffect>();

    public bool dirty = false;

    private List<AbstractEffect> effectsToRemove = new List<AbstractEffect>();

    /// <summary>
    /// ID of this affectable type 
    /// </summary>
    public int ID { get; private set; }

    protected abstract int ComputeID();


    protected void Update()
    {
        this.ActivateEffects();

        foreach (var ef in effectsToRemove)
        {
            activeEffects.Remove(ef);
        }

        effectsToRemove.Clear();
    }

    public void Awake()
    {
        Debug.Log("Assigning ID to status class " + this.GetType().Name);
        ID = ComputeID();
        new ItemManager();
        Type type = this.GetType();
        Debug.Log("Type: " + type);
        fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            Debug.Log("Field: " + field.Name);
        }
    }

    /// <summary>
    /// This method shall apply new values to attributes of this class referenced by their ID
    /// <paramref name="id"/> ID of the attribute to change
    /// <paramref name="newValue"/> new value to apply
    /// </summary>
    public void SetStatByID(int id, object newValue)
    {
        fields[id].SetValue(this, Convert.ChangeType(newValue, fields[id].FieldType));
        dirty = true;
    }

    /// <summary>
    /// This method shall resolve and return the value of a parameter by its ID
    /// <paramref name="id"/> ID of the parameter to resolve
    /// </summary>
    public object GetStatByID(int id)
    {
        if (id < fields.Length)
        {
            return fields[id].GetValue(this);
        }
        else
        {
            throw new Exception("Unable to resolve attribute ID " + id + " for status calss " + this.GetType().Name);
        }
    }

    /// <summary>
    /// Attach an effect to this status class.
    /// </summary>
    /// <param name="effect"></param>
    public void AttachEffect(AbstractEffect effect)
    {
        this.activeEffects.Add(effect);
    }

    /// <summary>
    /// Remove an effect from this status class.
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveEffect(AbstractEffect effect)
    {
        this.effectsToRemove.Add(effect);
    }

    /// <summary>
    /// Activate effect in the effect list
    /// </summary>
    protected void ActivateEffects()
    {
        foreach (var ef in activeEffects)
        {
            float? toApply = ef.Activate(this);
            if (toApply != null)
            {
                this.SetStatByID(ef.targetAttributeID, toApply.Value);
            }
        }
    }
}