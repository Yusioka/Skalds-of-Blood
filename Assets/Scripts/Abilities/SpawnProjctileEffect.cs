using Combat;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Targeting/Spawn Projectile Effect", order = 0)]
public class SpawnProjctileEffect : EffectStrategy
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] int damage;
    [SerializeField] float speed;

    public override void StartEffect(AbilityData data, Action finished)
    {
        var source = data.GetSource().GetComponent<PlayerAttackController>();
        SpawnProjectileForTargetPoint(data, source.AbilityPoint.position);
        
        finished();
    }

    private void SpawnProjectileForTargetPoint(AbilityData data, Vector3 spawnPosition)
    {
        var projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(data.GetTargetedPoint(), data.GetSource().Faction, damage, speed);
    }
}