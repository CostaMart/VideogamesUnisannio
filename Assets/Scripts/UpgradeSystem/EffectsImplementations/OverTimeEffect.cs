
using System.Collections.Generic;
using UnityEngine;

class OverTimeEffect : AbstractEffect
{
    /// <summary>
    /// Total duration of the effect
    /// </summary>
    private float totalDuration;
    private float timeLimitBeforeActivation;
    private float activationTimer = 0;
    private float totalTimeElapsed = 0;
    private bool active = false;

    public OverTimeEffect(Dictionary<string, string> data, int itemID, int effectID) : base(data, itemID)

    {
        if (!data.ContainsKey("totalDuration") || !data.ContainsKey("rate"))
        {
            throw new System.Exception("OverTimeEffect: 'totalDuration' or 'rate' is null for effect " + effectID + " in item with ID: " + itemID + " check if the itemList.json file is well formatted");
        }

        float totald = float.Parse(data["totalDuration"]);
        float actRate = float.Parse(data["rate"]);

        totalDuration = totald;
        timeLimitBeforeActivation = 1 / actRate;
    }

    public override void Activate(AbstractStatus target, EffectsDispatcher dipsatcher)
    {
        /// do not subscribe to time based effects more then once
        if (!active)
        {
            active = true;
            dipsatcher.AddToOvertimeList(this);
        }

        Tick(target, dipsatcher);
    }


    private void Tick(AbstractStatus target, EffectsDispatcher dispatcher)
    {
        totalTimeElapsed += Time.deltaTime;
        activationTimer += Time.deltaTime;

        if (activationTimer >= timeLimitBeforeActivation)
        {
            activationTimer = 0;
            base.DoEffect(target, dispatcher);
        }

        if (totalTimeElapsed >= totalDuration)
        {
            dispatcher.RemoveFromOvertimeList(this);
            active = false;
        }


    }

}