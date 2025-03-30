using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implement this to participate to the upgrade system and define your own effects
/// </summary>
public abstract class AbstractEffect
{
    public delegate float ApplyEffect(float newVal, float actualVal);

    public int name { get; private set; }

    /// <summary>
    /// ID of the class to be affected
    /// </summary>
    public int targetClassID { get; private set; }

    /// <summary>
    /// ID of the attribute to be affected within the class specified by <see cref="targetClassID"/>
    /// </summary>
    public int targetAttributeID { get; private set; }

    /// <summary>
    /// by setting this parameter to a not null value different, you can request the dispatcher to resolve the value.
    /// This parameter becomes the index of the class you want to get the value from.
    /// </summary>
    public int? referencedAttributeClassID { get; private set; }

    /// <summary>
    /// By setting referencedAttributeClass to a not null value , you can specify this value which attribute 
    /// of the class you want to use as parameter.
    /// </summary>
    public int? referencedAttributeID { get; private set; }

    /// <summary>
    /// Value used to compute the final value of the effect. It can be processed into Apply and then applied into ActivateEffect 
    /// </summary>
    public float newValue { get; set; }

    /// <summary>
    /// The value returned by this function can be applied to the stat target value.
    /// This function can be passed from exteranl sources to change the value computation.
    /// For example you may want to sum something to the referencedValue to generate the newValue,
    /// or you want to multiply it, you can just change what this function outputs in a "strategy pattern" fashion.
    /// 
    /// IMPORTANT:
    /// The first parameter is the new value to be applied to the target attribute.
    /// The second parameter is the actual value of the target attribute.
    /// this is what the programmer whose implementing the effect expect to compute the value, so don't do anything fancy here.
    /// 
    /// </summary>
    private ApplyEffect Apply { get; set; }

    public AbstractEffect(Dictionary<string, string> data, int itemID)
    {
        this.targetClassID = int.Parse(data["targetClass"]);
        this.targetAttributeID = int.Parse(data["targetStat"]);

        if (data.ContainsKey("referencedAttributeClassID"))
            this.referencedAttributeClassID = int.Parse(data["referencedAttributeClassID"]);

        if (data.ContainsKey("referencedAttributeID"))
            this.referencedAttributeID = int.Parse(data["referencedAttributeID"]);

        if (data.ContainsKey("effectValue"))
            this.newValue = float.Parse(data["effectValue"], System.Globalization.CultureInfo.InvariantCulture);


        // where actual value is the current value of the attribute, newval is the one to be applied by the effect
        if (data.ContainsKey("effectOperand"))
            switch ((string)data["effectOperand"])
            {
                case "add":
                    Apply = (newVal, actualVal) => newVal + actualVal;
                    break;
                case "multiply":
                    Apply = (newVal, actualVal) => newVal * actualVal;
                    break;
                case "divide":
                    Apply = (newVal, actualVal) => actualVal / newVal;
                    break;
                case "sub":
                    Apply = (newVal, actualVal) => actualVal - newVal;
                    break;
                case "set":
                    Apply = (newVal, actualVal) => newVal;
                    break;
                default:
                    Apply = (newVal, actualVal) => newVal;
                    break;
            }
    }

    /// <summary>
    /// This method applies a new value to the IAffecatble feature. 
    /// Following application logic defined by this method, such as applying this effect 
    /// in response to an event or appling it only once.
    /// </summary>
    /// 
    /// <param name="target">The target of the item effect.</param>
    /// <param name="dispatcher">
    /// Provides access to other game system elements, such as event dispatchers. 
    /// Use this to implement custom behaviors.
    /// </param>
    protected void DoEffect(AbstractStatus target)
    {
        var val = Apply(newValue, target.GetStatByID(targetAttributeID));
        target.SetStatByID(targetAttributeID, val);
    }

    /// <summary>
    /// define here the activation logic of the effect.
    /// to apply the effect call <see cref="DoEffect"/> method.
    /// <paramref name="dispatcher"/> is used to access other game systems.
    /// <paramref name="target"/> is the target of the effect.
    /// 
    /// </summary>
    /// 
    /// TODO: potrei voler levare target come parametro per impedire a chi scrive gli effetti di fare cose strane
    public abstract void Activate(AbstractStatus target, EffectsDispatcher dispatcher);


}
