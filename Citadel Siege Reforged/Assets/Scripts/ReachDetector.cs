using System;
using System.Collections.Generic;
using UnityEngine;

public class ReachDetector : MonoBehaviour
{
    public event Action<IProperty> OnTargetFound;

    [SerializeField] private float detectionRadius = 1.65f;
    [SerializeField] private float detectionInterval = 0.5f;
    [SerializeField] private LayerMask targetLayer;

    private Collider ownCollider;
    private IProperty unit;
    private float timer = 0f;

    public bool lookingForTargets = true;

    private readonly List<IProperty> potentialTargets = new List<IProperty>();
    private readonly Collider[] hitBuffer = new Collider[20]; // Adjust size as needed

    void Awake()
    {
        unit = GetComponent<Unit>();
        ownCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!lookingForTargets || unit.Target != null) return;

        timer += Time.deltaTime;

        if (timer >= detectionInterval)
        {
            timer = 0f;
            DetectTarget();
        }
    }

    private void DetectTarget()
    {
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, hitBuffer, targetLayer);
        potentialTargets.Clear();

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = hitBuffer[i];
            if (hit == ownCollider) continue;

            IProperty property = hit.GetComponent<Unit>() as IProperty;
            if (property == null)
                property = hit.GetComponent<Structure>() as IProperty;

            if (property != null && property.Owner != unit.Owner && property != unit)
            {
                potentialTargets.Add(property);
            }
        }

        foreach (var p in potentialTargets)
        {
            OnTargetFound?.Invoke(p);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
