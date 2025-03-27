
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

    public OverTimeEffect(int targetClassID, int targetAttributeID, ApplyEffect effect, float val, float tickRate, float totalDuration) : base(targetClassID, targetAttributeID, effect, val)
    {
        this.totalDuration = totalDuration;
        timeLimitBeforeActivation = 1 / tickRate;
    }

    public override void Activate(IAffectable target, EffectsDispatcher dipsatcher)
    {
        /// do not subscribe to time based effects more then once
        if (!active)
        {
            active = true;
            dipsatcher.AddToOvertime(this);
        }

        Tick(target, dipsatcher);
    }

    private void Tick(IAffectable target, EffectsDispatcher dispatcher)
    {
        totalTimeElapsed += Time.deltaTime;
        activationTimer += Time.deltaTime;

        if (activationTimer >= timeLimitBeforeActivation)
        {
            activationTimer = 0;
            Debug.Log("tick, elapsed time: " + totalTimeElapsed);
            base.DoEffect(target, dispatcher);
        }

        if (totalTimeElapsed >= totalDuration)
        {
            dispatcher.RemoveFromOvertime(this);
            active = false;
        }


    }
}