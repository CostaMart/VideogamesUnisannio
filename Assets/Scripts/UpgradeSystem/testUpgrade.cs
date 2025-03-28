using System;
using Unity.VisualScripting;
using UnityEngine;

class testUpdate : MonoBehaviour
{
    public int ID => 1;
    [SerializeField] public float theParam;

    public float GetStatByID(int id)
    {
        return theParam;
    }

    public void SetStatByID(int id, float newValue)
    {
        Debug.Log("Applying effect to " + id + " with value " + newValue);
    }
}
