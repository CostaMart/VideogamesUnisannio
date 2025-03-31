using System;
using System.Reflection;
using UnityEngine;

/// <summary>
/// A status class that wants to participate to the item upgrade system must implement this interface, 
/// the effect dispatcher will look for these classes in the gameobject hierarchy.
/// </summary>
public abstract class AbstractStatus : MonoBehaviour
{
    public FieldInfo[] fields;

    public AbstractStatus()
    {
        ID = ItemManager.statClassToIdRegistry[this.GetType().Name];
    }

    /// <summary>
    /// ID of this affectable type 
    /// </summary>
    public int ID { get; private set; }

    void Awake()
    {
        new ItemManager();
        Type type = this.GetType();
        Debug.Log("Type: " + type);
        fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        Debug.Log("Fields found: " + fields.Length);
    }


    /// <summary>
    /// This method shall apply new values to attributes of this class referenced by their ID
    /// <paramref name="id"/> ID of the attribute to change
    /// <paramref name="newValue"/> new value to apply
    /// </summary>
    public void SetStatByID(int id, float newValue)
    {
        fields[id].SetValue(this, newValue);
    }

    /// <summary>
    /// This method shall resolve and return the value of a parameter by its ID
    /// <paramref name="id"/> ID of the parameter to resolve
    /// </summary>
    public float GetStatByID(int id)
    {
        if (id < fields.Length)
        {
            return (float)fields[id].GetValue(this);
        }
        else
        {
            Debug.LogError("attribute ID" + id + " out of range for class" + this.GetType().Name + ", requested value will be resolved with 0");
            return 0f;
        }
    }
}