using UnityEngine;

public class Upgrade
{
    public delegate float Operation(float operand);
    public float lastActivation;
    public int durationTicks = 0;
    public float tickNumber = 0;
    public float tickTime = 1;
    public float nextTickTime = 0;
    public bool isOvertime = false;
    public int classTarget;
    public int attributeTarget;
    public int valueRef;
    public float value;
    public Operation operation;

}
