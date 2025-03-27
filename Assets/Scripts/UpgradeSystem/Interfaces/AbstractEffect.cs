using System;
using UnityEngine;

/// <summary>
/// Implement this to participate to the upgrade system and define your own effects
/// </summary>
public abstract class AbstractEffect
{
    public delegate float ApplyEffect(float newValue);

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
    public int? referecedAttributeClassID { get; private set; }

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
    /// </summary>
    protected ApplyEffect Apply { get; set; }

    public AbstractEffect(int targetClassID, int targetAttributeID, ApplyEffect effect)
    {
        this.targetClassID = targetClassID;
        this.targetAttributeID = targetAttributeID;
        Apply = effect;
    }
    public AbstractEffect(int targetClassID, int targetAttributeID, ApplyEffect effect, int referencedClassID, int referencedAttributeID)
    {
        this.targetClassID = targetClassID;
        this.targetAttributeID = targetAttributeID;
        referecedAttributeClassID = referencedClassID;
        this.referencedAttributeID = referencedAttributeID;
        Apply = effect;
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
    public abstract void ActivateEffect(IAffectable target, EffectsDispatcher dipsatcher);
}
