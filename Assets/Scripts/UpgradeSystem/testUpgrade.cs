using System;
using Unity.VisualScripting;
using UnityEngine;

class testUpdate : MonoBehaviour, IAffectable
{
    public int ID => 1;
    [SerializeField] public float theParam;

    public void AutoApplyEffect(IEffect upgrade)
    {
        throw new System.NotImplementedException();
    }

    public float ResolveParameterValueByID(int id)
    {
        return theParam;
    }


    public void AutoApplyEffect(int targetAttribute, Func<float, float> upgrade)
    {
        Debug.Log("Applying effect to " + targetAttribute + " with value " + upgrade(theParam));
    }
}
