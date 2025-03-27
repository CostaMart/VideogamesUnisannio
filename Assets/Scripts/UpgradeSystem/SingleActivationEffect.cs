using System;
using UnityEngine;

/// <summary>
/// simply sets effect once
/// </summary>
public class SingleActivationEffect : AbstractEffect
{
    public SingleActivationEffect(int targetClassID,
    int targetAttributeID, ApplyEffect effect, float val) : base(targetClassID, targetAttributeID, effect, val) { }

    public SingleActivationEffect(int targetClassID, int targetAttributeID, ApplyEffect effect,
     int referencedClassID, int referencedAttributeID) : base(targetClassID, targetAttributeID, effect, referencedClassID,
      referencedAttributeID)
    { }

    public override void Activate(IAffectable target, EffectsDispatcher dispatcher)
    {

    }
}
