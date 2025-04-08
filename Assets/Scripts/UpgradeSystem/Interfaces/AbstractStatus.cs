using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// A status class that wants to participate to the item upgrade system must implement this interface, 
/// the effect dispatcher will look for these classes in the gameobject hierarchy.
/// </summary>
public abstract class AbstractStatus : MonoBehaviour
{
    public Dictionary<FeatureType, Feature> features = new();

    private List<AbstractEffect> activeEffects = new List<AbstractEffect>();

    public bool dirty = false;

    private List<AbstractEffect> effectsToRemove = new List<AbstractEffect>();

    /// <summary>
    /// ID of this affectable type 
    /// </summary>
    public int ID { get; private set; }

    public AbstractStatus()
    {

    }

    public Dictionary<FeatureType, Feature> LoadFeatures()
    {

        string[] lines = File.ReadAllLines("/home/costamh/HeroDivers/gameConfig/Features.txt");
        bool found = false;
        bool hasBeenFound = false;
        Dictionary<FeatureType, Feature> features = new Dictionary<FeatureType, Feature>();

        foreach (var thisLine in lines)
        {

            var line = thisLine.Trim();
            line = line.Replace(" ", "");
            line = line.Split("//")[0];

            if (found && line.Contains("##"))
            {
                found = false;
            }


            if (found)
            {
                try
                {
                    string[] parts = line.Split("=");

                    Type t;
                    FeatureType featureType = (FeatureType)Enum.Parse(typeof(FeatureType), parts[0]);

                    if (int.TryParse(parts[1], out _))
                    {
                        Feature f = new Feature(featureType, int.Parse(parts[1]), typeof(int));
                        f.SetValue(int.Parse(parts[1]));
                        features.Add(featureType, f);
                        Debug.Log("gameobject" + this.gameObject.name + " parsed value: " + parts[1] + " of " + parts[0]
                         + " as int");
                    }
                    else if (float.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _))
                    {
                        Feature f = new Feature(featureType, float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture), typeof(float));
                        f.SetValue(float.Parse(parts[1].Replace(" ", ""), System.Globalization.CultureInfo.InvariantCulture));
                        features.Add(featureType, f);
                        Debug.Log("gameobject" + this.gameObject.name + " parsed value: " + parts[1] + " of " + parts[0]
                         + " as float");
                    }
                    else
                    {
                        Feature f = new Feature(featureType, bool.Parse(parts[1]), typeof(bool));
                        f.SetValue(bool.Parse(parts[1]));
                        features.Add(featureType, f);
                        Debug.Log("gameobject" + this.gameObject.name + " parsed value: " + parts[1] + " of " + parts[0]
                         + " as bool");
                    }

                }
                catch
                (Exception e)
                {
                    Debug.LogError("Error in parsing line: " + line);
                }


            }

            if (line.Contains("#" + this.gameObject.name + "-" + this.GetType().Name))
            {
                found = true;
                hasBeenFound = true;
            }
        }

        if (hasBeenFound)
            return features;

        /// simply empty
        return new Dictionary<FeatureType, Feature>();


    }

    protected abstract int ComputeID();

    protected virtual void Update()
    {
        this.ActivateEffects();

        foreach (var ef in effectsToRemove)
        {
            activeEffects.Remove(ef);
        }

        effectsToRemove.Clear();
    }

    protected virtual void Awake()
    {
        features = LoadFeatures();
        Debug.Log("Assigning ID to status class " + this.GetType().Name);
        ID = ComputeID();
        new ItemManager();
        this.dirty = true;
    }

    /// <summary>
    /// This method shall apply new values to attributes of this class referenced by their ID
    /// <paramref name="id"/> ID of the attribute to change
    /// <paramref name="newValue"/> new value to apply
    /// </summary>
    public void SetStatByID(int id, object newValue)
    {
        FeatureType featureType = (FeatureType)id;
        features[featureType].SetValue(Convert.ChangeType(newValue, features[featureType].GetValue().GetType()));
        dirty = true;
    }

    /// <summary>
    /// This method shall resolve and return the value of a parameter by its ID
    /// <paramref name="id"/> ID of the parameter to resolve
    /// </summary>
    public T GetStatByID<T>(int id)
    {
        try
        {
            return (T)features[(FeatureType)id].GetValue();
        }

        catch (KeyNotFoundException)
        {
            Debug.LogError("invoked GetStatByID of object: " + this.GetType().Name
            + " in gameobject " + this.gameObject.name + " for " + (FeatureType)id +
            " but the feature as not been assigned");
        }
        catch (InvalidCastException)
        {
            Debug.LogError("invoked GestStatByID with id: " + id + " and type: " + typeof(T) +
            " but the value is of type: " + features[(FeatureType)id].GetValue().GetType());
        }

        return default(T);
    }

    /// <summary>
    /// Attach an effect to this status class.
    /// </summary>
    /// <param name="effect"></param>
    public virtual void AttachEffect(AbstractEffect effect)
    {
        this.activeEffects.Add(effect);
    }

    /// <summary>
    /// Remove an effect from this status class.
    /// </summary>
    /// <param name="effect"></param>
    public virtual void RemoveEffect(AbstractEffect effect)
    {
        this.effectsToRemove.Add(effect);
    }

    /// <summary>
    /// Activate effect in the effect list
    /// </summary>
    protected virtual void ActivateEffects()
    {
        object toApply;

        foreach (var featureKey in features.Keys)
        {
            if (features[featureKey].type == typeof(int))
            {
                int inthelper = (int)features[featureKey].baseValue;
                inthelper += Convert.ToInt32(activeEffects.Where
                (x => x.targetAttributeID == (int)features[featureKey].id).Sum(x => Convert.ToInt32(x.Activate(this))));
                toApply = inthelper;
            }
            else if (features[featureKey].type == typeof(float) || features[featureKey].type == typeof(Single))
            {
                float floathelper = (float)features[featureKey].baseValue;
                floathelper += (float)activeEffects.Where
                (x => x.targetAttributeID == (int)features[featureKey].id).Sum(x => (float)x.Activate(this));
                toApply = floathelper;
            }
            else if (features[featureKey].type == typeof(bool))
            {
                if (activeEffects.Where(x => x.targetAttributeID == (int)features[featureKey].id).Count() != 0)
                    toApply = activeEffects.Where(x => x.targetAttributeID == (int)features[featureKey].id).Last().Activate(this);
                else toApply = true;
            }
            else
            {
                throw new ArgumentException("Invalid type: " + features[featureKey].type);
            }

            if (activeEffects.Count > 0)
                features[featureKey].SetValue(toApply);
        }


    }
}