using Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData
{
    Entity entity;
    Vector3 targetedPoint;
    IEnumerable<GameObject> targets;
    bool cancelled = false;

    public AbilityData(Entity entity)
    {
        this.entity = entity;
    }
    public Entity GetSource()
    {
        return entity;
    }

    public Vector3 GetTargetedPoint()
    {
        return targetedPoint;
    }

    public void SetTargetedPoint(Vector3 targetedPoint)
    {
        this.targetedPoint = targetedPoint;
    }

    public void SetTargets(IEnumerable<GameObject> targets)
    {
        this.targets = targets;
    }

    public IEnumerable<GameObject> GetTargets()
    {
        return targets;
    }

    public void StartCoroutine(IEnumerator coroutine)
    {
        entity.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }

    public void Cancel()
    {
        cancelled = true;
    }

    public bool IsCancelled()
    {
        return cancelled;
    }
}