using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// simply sets effect once
/// </summary>
public class SingleActivationEffect : AbstractEffect
{

  public SingleActivationEffect(Dictionary<string, string> data, int itemID, int effectID) : base(data, itemID) { }
  public override float? Activate(AbstractStatus target)
  {
    var result = base.DoEffect();
    target.RemoveEffect(this);
    return result;
  }

}
