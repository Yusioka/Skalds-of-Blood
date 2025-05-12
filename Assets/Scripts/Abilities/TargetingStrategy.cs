using System;
using UnityEngine;

public abstract class TargetingStrategy : ScriptableObject
{
    public abstract void StartTargeting(AbilityData data, Action finished);
}