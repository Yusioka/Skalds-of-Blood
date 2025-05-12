using Control;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities/New Ability", order = 1)]
public class Ability : ScriptableObject
{
    [SerializeField] TargetingStrategy targetingStrategy;
    [SerializeField] FilterStrategy[] filterStrategies;
    [SerializeField] EffectStrategy[] effectStrategies;
    [SerializeField] public float cooldownTime = 0;

    [System.NonSerialized] public float CooldownTimer = 0;
    private float _cooldownMultiplier;

    public bool TryUse(Entity entity, float cooldownMultiplier)
    {
        if (CooldownTimer > 0)
        {
            return false;
        }

        _cooldownMultiplier = cooldownMultiplier;
        var data = new AbilityData(entity);
        targetingStrategy.StartTargeting(data, () => TargetAquired(data));
        return true;
    }

    public void UpdateCooldown(float deltaTime)
    {
        CooldownTimer -= deltaTime;
    }

    private void TargetAquired(AbilityData data)
    {
        if (data.IsCancelled()) return;

        foreach (var filterStrategy in filterStrategies)
        {
            data.SetTargets(filterStrategy.Filter(data.GetTargets()));
        }

        foreach (var effect in effectStrategies)
        {
            effect.StartEffect(data, EffectFinished);
        }
    }

    private void EffectFinished()
    {
        CooldownTimer = cooldownTime / _cooldownMultiplier;
    }
}