using NCalc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Implement this to participate to the upgrade system and define your own effects
/// </summary>
public abstract class AbstractEffect
{
    public int ID;
    public delegate float ApplyEffect(float newVal, float actualVal);

    public int name { get; private set; }

    public Expression ex;

    /// <summary>
    /// This is the list of parameters translated into index from the 'expr' string of the json effect
    /// </summary>
    public int[][] parametersRef;

    /// <summary>
    /// This array the value of the parameters in parametersRef, it is updated at each DoEffect call
    /// </summary>
    public float[] resolvedVals;

    /// <summary>
    /// ID of the class to be affected
    /// </summary>
    public int targetClassID { get; private set; }

    /// <summary>
    /// ID of the attribute to be affected within the class specified by <see cref="targetClassID"/>
    /// </summary>
    public int targetAttributeID { get; private set; }


    /// <summary>
    /// This constructor inits the effect expression with data from json file. it transaltes the expression parameters into IDs
    /// This constructer is only called at game start when ItemManager reads the JSON file. The expression is compiled only once and stored in the ex variable.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="itemID"></param>
    public AbstractEffect(Dictionary<string, string> data, int itemID)
    {

        string targetClassString = data["target"];
        string s = data["expr"];


        MatchCollection matches = Regex.Matches(data["expr"], @"@\w+\.\d+");
        targetClassID = ItemManager.statClassToIdRegistry[targetClassString.Split(".")[0]];
        targetAttributeID = int.Parse(targetClassString.Split(".")[1]);


        char c = 'A';
        int len = matches.Count;
        parametersRef = new int[len][];
        int i = 0;

        // cerchiamo nella stringa tutti i riferimenti a variabili di altre classi, nel caso se ne trovino vengono sostituite con un ID alfabetico nella stringa dell'espression
        // contemporaneamente il nome viene tradotto in ID numerico e inserito nell'array dei riferimenti da risolvere. I valori vengono risolti 
        // ad ogni invocazione di DoEffect in modo che siano sempre aggiornati
        foreach (var match in matches)
        {
            s = s.Replace(match.ToString(), c.ToString());
            string laClass = match.ToString().Split('.')[0].Substring(1);
            int laAttribute = int.Parse(match.ToString().Split('.')[1]);

            parametersRef[i] = new int[2];
            parametersRef[i][0] = ItemManager.statClassToIdRegistry[laClass];
            parametersRef[i][1] = laAttribute;

            c += (char)1;
            i++;
        }

        ex = new Expression(s);
    }

    /// <summary>
    /// This method applies the effect to the target. It resolves the parameters and applies the expression to compute the new value for the target attribute.
    /// </summary>
    /// <param name="target">The target of the item effect.</param>
    /// <param name="dispatcher">
    /// Provides access to other game system elements, such as event dispatchers. 
    /// Use this to implement custom behaviors.
    /// </param>
    protected void DoEffect(AbstractStatus target, EffectsDispatcher dispatcher)
    {
        resolvedVals = dispatcher.ResolveValue(parametersRef);

        char c = 'A';
        foreach (var value in resolvedVals)
        {
            ex.Parameters[c.ToString()] = value;
            c += (char)1;
        }

        float result = Convert.ToSingle(ex.Evaluate());
        target.SetStatByID(targetAttributeID, result);
    }

    /// <summary>
    /// define here the activation logic of the effect.
    /// to apply the effect call <see cref="DoEffect"/> method.
    /// <paramref name="dispatcher"/> is used to access other game systems.
    /// <paramref name="target"/> is the target of the effect.
    /// </summary>
    /// TODO: potrei voler levare target come parametro per impedire a chi scrive gli effetti di fare cose strane
    public abstract void Activate(AbstractStatus target, EffectsDispatcher dispatcher);

}
