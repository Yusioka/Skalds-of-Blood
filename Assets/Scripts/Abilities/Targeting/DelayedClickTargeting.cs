using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using Control;
using Core;

[CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click", order = 0)]
public class DelayedClickTargeting : TargetingStrategy
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float areaAffectRadius;
    [SerializeField] Transform targetingPrefab;

    Transform targetingPrefabInstance = null;

    public override void StartTargeting(AbilityData data, Action finished)
    {
        PlayerController playerController = data.GetSource().GetComponent<PlayerController>();
        playerController.StartCoroutine(Targeting(data, finished));
    }

    private IEnumerator Targeting(AbilityData data, Action finished)
    {
   //     playerController.enabled = false;
        GameManager.Instance.IsCursorActive = true;
        GameManager.Instance.IsSkillUsing = true;
        if (targetingPrefabInstance == null)
        {
            targetingPrefabInstance = Instantiate(targetingPrefab);
        }
        else
        {
            targetingPrefabInstance.gameObject.SetActive(true);
        }

        targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);
        while (data.IsCancelled() == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                data.Cancel();
                break;
            }

            if (Physics.Raycast(PlayerController.GetMouseRay(), out var raycastHit, 1000, layerMask))
            {
                targetingPrefabInstance.position = raycastHit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    // Absorb the whole mouse click
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
                    data.SetTargetedPoint(raycastHit.point);
                    data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                    break;
                }
            }
            yield return null;
        }

        targetingPrefabInstance.gameObject.SetActive(false);
        GameManager.Instance.IsCursorActive = false;
        GameManager.Instance.IsSkillUsing = false;
  //      playerController.enabled = true;
        finished();
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
    {
        RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
        foreach (var hit in hits)
        {
            yield return hit.collider.gameObject;
        }
    }
}