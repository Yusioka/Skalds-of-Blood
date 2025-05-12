using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Directional Targeting", menuName = "/Abilities/Targeting/Directional Targeting", order = 0)]
public class DirectionalTargeting : TargetingStrategy
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float groundOffset = 1;

    public override void StartTargeting(AbilityData data, Action finished)
    {
        var source = data.GetSource().transform;

        data.SetTargetedPoint(source.position + source.forward * 50f + Vector3.up * (groundOffset - source.position.y));

        finished();
    }
}