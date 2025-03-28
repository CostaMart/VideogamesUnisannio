using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// simply sets effect once
/// </summary>
public class SingleActivationEffect : AbstractEffect
{

  public SingleActivationEffect(Dictionary<string, string> data) : base(data) { }
  public override void Activate(AbstractStatus target, EffectsDispatcher dispatcher)
  {
    base.DoEffect(target);
  }

}
