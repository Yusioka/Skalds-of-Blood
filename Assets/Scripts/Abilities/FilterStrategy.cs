using System.Collections.Generic;
using UnityEngine;

public abstract class FilterStrategy : ScriptableObject
{
    public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject> enumerable);
}