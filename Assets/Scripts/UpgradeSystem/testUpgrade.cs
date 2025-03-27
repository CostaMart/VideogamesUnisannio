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

    public float GetStatByID(int id)
    {
        return theParam;
    }

    public void SetStatByID(int id, float newValue)
    {
        Debug.Log("Applying effect to " + id + " with value " + newValue);
    }
}
