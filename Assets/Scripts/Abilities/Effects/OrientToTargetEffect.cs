using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Orient To Target Effect", menuName = "Abilities/Effects/New Orient To Target Effect", order = 0)]
public class OrientToTargetEffect : EffectStrategy
{
    public override void StartEffect(AbilityData data, Action finished)
    {
        data.GetSource().transform.LookAt(data.GetTargetedPoint());
        finished();
    }
}