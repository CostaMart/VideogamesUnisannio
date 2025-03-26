using System;
using UnityEngine;

public class SingleActivationIncrementEffect : IEffect
{
    public int targetClassID { get; set; }
    public int targetAttributeID { get; set; }

    public int referecedAttributeClass => -1;

    public int referencedAttribute => -1;

    public float value { get; set; }

    public void ActivateEffect(IAffectable upgradable, UpgradeDispatcher dipsatcher)
    {
        upgradable.AutoApplyEffect(targetAttributeID, (f1) => { return f1 + value; });
    }

}
