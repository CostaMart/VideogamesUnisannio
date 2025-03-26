using System;
using UnityEngine;

public interface IEffect
{

    /// <summary>
    /// ID of the class to be affected
    /// </summary>
    int targetClassID { get; set; }

    /// <summary>
    /// ID of the attribute to be affected within the class specified above
    /// </summary>
    int targetAttributeID { get; set; }

    /// <summary>
    /// by setting this parameter to a value different from -1, you can request the dispatcher to resolve the value. This parameter becomes the index of the class you want to get the value from*/
    /// </summary>
    int referecedAttributeClass { get; }

    /*by setting referencedAttributeClass to a valure different from -1 , you can specify whtih this value which attribute of the class you want to use as parameter*/
    public int referencedAttribute { get; }

    /// <summary>
    /// value to be applied as modiciation
    /// </summary>
    public float value { get; set; }



    /// <summary>
    ///  This method should do the job, here you must call the AutoApply method on IAffecatble if you want to apply the effect, or do whatever you effect is about
    /// </summary>
    /// <param name="upgradable"></param>
    /// <param name="dipsatcher"></param>
    public void ActivateEffect(IAffectable upgradable, UpgradeDispatcher dipsatcher);


}
