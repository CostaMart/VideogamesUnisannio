using System;

/// <summary>
/// A status class that wants to participate to the item upgrade system must implement this interface.
/// </summary>
public interface IAffectable
{
    int ID { get; }

    void GetId()
    {
        // qui si recuperà l'ID da file
    }

    /// <summary>
    /// This method is called to apply effects to attributes of this class. It must implement the logic to apply the result of effect to the target attribute
    /// /// </summary>
    /// <param name="targetAttribute"></param>
    /// <param name="upgrade"></param>
    void AutoApplyEffect(int targetAttribute, Func<float, float> upgrade);

    /// <summary>
    /// this method is called to resolve the value of a parameter by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    float ResolveParameterValueByID(int id);
}