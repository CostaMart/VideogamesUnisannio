using System;
using UnityEngine;

/// <summary>
/// simply sets effect once
/// </summary>
public class SingleActivationEffect : AbstractEffect
{
    public SingleActivationEffect(int targetClassID,
    int targetAttributeID, ApplyEffect effect) : base(targetClassID, targetAttributeID, effect) { }

    public SingleActivationEffect(int targetClassID, int targetAttributeID, ApplyEffect effect,
     int referencedClassID, int referencedAttributeID) : base(targetClassID, targetAttributeID, effect, referencedClassID,
      referencedAttributeID)
    { }

    public override void ActivateEffect(IAffectable upgradable, EffectsDispatcher dipsatcher)
    {
        var val = Apply(newValue);
        upgradable.SetStatByID(targetAttributeID, val);
    }
}
