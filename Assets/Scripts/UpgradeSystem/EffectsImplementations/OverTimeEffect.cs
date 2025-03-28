
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

    public OverTimeEffect(Dictionary<string, string> data) : base(data)
    {
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
            Debug.Log("tick, elapsed time: " + totalTimeElapsed);
            base.DoEffect(target);
        }

        if (totalTimeElapsed >= totalDuration)
        {
            dispatcher.RemoveFromOvertimeList(this);
            active = false;
        }


    }
}