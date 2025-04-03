using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Implement this to participate to the upgrade system and define your own effects
/// </summary>
public abstract class AbstractEffect
{
    public int ID;

    private EffectsDispatcher dispatcher;

    public int name { get; private set; }

    /// <summary>
    /// expression to compute new vals from this effect.
    /// It is already in its compiled form, so it is not necessary to compile it again.
    /// </summary>
    public Expression ex;

    /// <summary>
    /// This is the list of parameters translated into index from the 'expr' string of the json effect
    /// </summary>
    public int[][] localParametersRef;

    public AbstractStatus[] localParametersRefClasses = new AbstractStatus[0];

    public AbstractStatus[] externParametersRefClasses = new AbstractStatus[0];

    public char[] localParametersKey;

    public int[][] externParametersRef;
    public char[] externParametersKey;

    /// <summary>
    /// This array the value of the parameters in parametersRef, it is updated at each DoEffect call
    /// </summary>
    public object[] resolvedValsLocal;
    public object[] resolvedValsExternal;

    /// <summary>
    /// ID of the class to be affected
    /// </summary>
    public int localTargetClassID = -1;

    public int externalTargetClassID = -1;

    /// <summary>
    /// ID of the attribute to be affected within the class specified by <see cref="localTargetClassID"/>
    /// </summary>
    public int targetAttributeID { get; private set; }


    /// <summary>
    /// This constructor inits the effect expression with data from json file. it transaltes the expression parameters into IDs
    /// This constructer is only called at game start when ItemManager reads the JSON file. The expression is compiled only once and stored in the ex variable.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="itemID"></param>
    public AbstractEffect(Dictionary<string, string> data, int itemID, bool inABullet)
    {

        string targetClassString = data["target"];
        string s = data["expr"];


        // search for parameters to resolve locally
        MatchCollection matches = Regex.Matches(data["expr"], @"@\w+\.\d+");
        MatchCollection external = Regex.Matches(data["expr"], @"!\w+\.\d+");

        if (targetClassString.Contains("@"))
        {
            localTargetClassID = ItemManager.statClassToIdRegistry[targetClassString.Split(".")[0].Replace("@", "")];
        }
        else if (targetClassString.Contains("!"))
        {
            externalTargetClassID = ItemManager.statClassToIdRegistry[targetClassString.Split(".")[0].Replace("!", "")];
        }

        targetAttributeID = int.Parse(targetClassString.Split(".")[1]);


        char c = 'A';
        int len = matches.Count;
        localParametersRef = new int[len][];
        localParametersKey = new char[len];
        Debug.Log("this is the numebr of local parameters " + len + "for item " + itemID);

        len = external.Count;

        externParametersRef = new int[len][];
        externParametersKey = new char[len];

        Debug.Log("this is the numebr of external parameters " + len + "for item " + itemID);
        int i = 0;

        // cerchiamo nella stringa tutti i riferimenti a variabili di altre classi, nel caso se ne trovino vengono sostituite con un ID alfabetico nella stringa dell'espression
        // contemporaneamente il nome viene tradotto in ID numerico e inserito nell'array dei riferimenti da risolvere. I valori vengono risolti 
        // ad ogni invocazione di DoEffect in modo che siano sempre aggiornati
        foreach (var match in matches)
        {
            s = s.Replace(match.ToString(), c.ToString());
            string laClass = match.ToString().Split('.')[0].Substring(1);
            int laAttribute = int.Parse(match.ToString().Split('.')[1]);

            localParametersRef[i] = new int[2];
            localParametersRef[i][0] = ItemManager.statClassToIdRegistry[laClass];
            localParametersRef[i][1] = laAttribute;
            localParametersKey[i] = c;

            Debug.Log("it is just been assigned key " + c.ToString() + " for item " + itemID);
            c += (char)1;
            i++;
        }

        foreach (var match in external)
        {
            s = s.Replace(match.ToString(), c.ToString());
            string laClass = match.ToString().Split('.')[0].Substring(1);
            int laAttribute = int.Parse(match.ToString().Split('.')[1]);

            externParametersRef[i] = new int[2];
            externParametersRef[i][0] = ItemManager.statClassToIdRegistry[laClass];
            externParametersRef[i][1] = laAttribute;
            externParametersKey[i] = c;

            c += (char)1;
            i++;
        }

        if (!inABullet)
        {
            if (external.Count > 0)
            {
                if (!data.ContainsKey("targetType") || data["targetType"] == "local")
                {
                    Debug.LogError("External parameters found but no target type specified. This effect will not be applied.");
                    return;
                }
            }
        }

        Debug.Log("for item " + itemID + " this is the expression: " + s);
        Debug.Log("this are local parameters: " + localParametersRef.Length);
        Debug.Log("this are external parameters: " + externParametersRef.Length);


        ex = new Expression(s);
    }

    public AbstractEffect() { }

    /// <summary>
    /// This method applies the effect to the target. It resolves the parameters and applies the expression to compute the new value for the target attribute.
    /// </summary>
    /// <param name="target">The target of the item effect.</param>
    /// <param name="dispatcher">
    /// Provides access to other game system elements, such as event dispatchers. 
    /// Use this to implement custom behaviors.
    /// </param>
    protected float DoEffect()
    {
        resolvedValsLocal = resolveValues(localParametersRefClasses, localParametersRef);
        resolvedValsExternal = resolveValues(externParametersRefClasses, externParametersRef);

        var x = 0;
        Debug.Log("intrnal keys available: " + localParametersKey.Length);
        Debug.Log("internal classes reference available: " + localParametersRefClasses.Length);
        foreach (var reference in localParametersRefClasses)
        {
            Debug.Log("internal keys resolved: " + localParametersKey[x].ToString());
            ex.Parameters[localParametersKey[x].ToString()] = resolvedValsLocal[x];
            x++;
        }

        x = 0;
        Debug.Log("external keys available: " + externParametersKey.Length);
        Debug.Log("external classes reference available: " + externParametersRefClasses.Length);
        Debug.Log("this is the expression: " + ex.ToString());
        foreach (var reference in externParametersRefClasses)
        {
            Debug.Log("external keys resolved: " + externParametersKey[x].ToString());
            Debug.Log("with external class reference: " + externParametersRefClasses[x].GetType().Name);
            Debug.Log("to parameter " + externParametersKey[x].ToString());
            ex.Parameters[externParametersKey[x].ToString()] = resolvedValsExternal[x];
            x++;
        }


        try
        {
            var returnable = Convert.ToSingle(ex.Evaluate());
            Debug.Log("expression computed : " + ex.ToString() + " for item " + ID);

            return returnable;
        }
        catch (Exception e)
        {
            Debug.LogError("Error in expression evaluation: " + e.Message);
        }

        return 0;
    }

    private object[] resolveValues(AbstractStatus[] statusClass, int[][] paramIndexes)
    {
        Debug.Log("resolving values for " + statusClass.GetType().Name + " parameters");
        var x = 0;
        object[] resolved = new object[statusClass.Length];

        foreach (var refClass in statusClass)
        {
            resolved[x] = refClass.GetStatByID(paramIndexes[x][1]);
        }

        return resolved;
    }

    /// <summary>
    /// attach this effect to the target status class. 
    /// and to the dispatcher.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="dispatcher"></param>
    public void Attach(AbstractStatus target, EffectsDispatcher dispatcher)
    {
        this.dispatcher = dispatcher;
        target.AttachEffect(this);
    }


    /// <summary>
    /// define here the activation logic of the effect.
    /// to apply the effect call <see cref="DoEffect"/> method.
    /// <paramref name="dispatcher"/> is used to access other game systems.
    /// <paramref name="target"/> is the target of the effect.
    /// </summary>
    /// TODO: potrei voler levare target come parametro per impedire a chi scrive gli effetti di fare cose strane
    public abstract float? Activate(AbstractStatus target);

}
