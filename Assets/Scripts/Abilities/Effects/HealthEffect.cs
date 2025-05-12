using Combat;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/New Health Effect", order = 0)]
public class HealthEffect : EffectStrategy
{
    [SerializeField] int healthChange;

    public override void StartEffect(AbilityData data, Action finished)
    {
        foreach (var target in data.GetTargets())
        {
            if (target.TryGetComponent<EnemyHealth>(out var health))
            {
                if (healthChange < 0)
                {
                    health.TakeHit(healthChange);
                }
            }
        }

        finished();
    }
}