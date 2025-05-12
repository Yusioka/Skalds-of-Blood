using UnityEngine;

public class HookSpell : Spell
{
    [SerializeField] float distance;
    [SerializeField] Transform shootPoint;

    [SerializeField] GameObject hookPrefab;

    Vector3 startPosition;

    public override void ActivateSpell(Vector3 point)
    {
        startPosition.y = 1f;

        GameObject hook = Instantiate(hookPrefab, shootPoint.position, Quaternion.identity);
        hook.GetComponentInChildren<Hook>().Initialize(distance, shootPoint.position, new Vector3(point.x, 1f, point.z));
    }
}
