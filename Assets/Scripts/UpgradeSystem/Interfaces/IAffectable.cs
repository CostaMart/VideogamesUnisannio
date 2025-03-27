using System;

/// <summary>
/// A status class that wants to participate to the item upgrade system must implement this interface, 
/// the effect dispatcher will look for these classes in the gameobject hierarchy.
/// </summary>
public interface AbstractAffectable
{
    /// <summary>
    /// ID of this affectable type 
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// This method shall apply new values to attributes of this class referenced by their ID
    /// /// </summary>
    /// 
    /// <param name="id"> ID of the attribute to change</param>
    /// <param name="newValue">new value to apply</param>
    void SetStatByID(int id, float newValue);

    /// <summary>
    /// This method shall resolve and return the value of a parameter by its ID
    /// </summary>
    /// 
    /// <param name="id"> ID of the parameter to resolve</param>
    /// <returns></returns>
    float GetStatByID(int id);
}