using System;
using UnityEngine;

class testUpdate : AbstractStatus
{
    public float penisSize = 0;

    protected override int ComputeID()
    {

        return ItemManager.statClassToIdRegistry[this.GetType().Name];
    }
}
