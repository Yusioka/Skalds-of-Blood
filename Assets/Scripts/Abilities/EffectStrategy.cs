using System;
using UnityEngine;

public abstract class EffectStrategy : ScriptableObject
{
    public abstract void StartEffect(AbilityData data, Action finished);
}