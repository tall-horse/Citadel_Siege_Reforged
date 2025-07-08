using System;
using System.Linq;
using UnityEngine;

public class ReachDetector : MonoBehaviour
{
    public event Action<IProperty> OnTargetFound;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float detectionInterval = 0.2f; // check 5 times per second
    [SerializeField] private LayerMask targetLayer;
    private Collider collider;
    private Unit unit;
    private float timer = 0f;
    void Awake()
    {
        unit = GetComponent<Unit>();
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= detectionInterval)
        {
            timer = 0f;
            DetectTarget();
        }
    }

    private void DetectTarget()
    {
        if (unit.Target != null) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);
        hits.ToList().Remove(collider);
        for (int i = 0; i < hits.Length; i++)
        {
            if (unit.Target != null) return;
            var potentialTarget = hits[i].GetComponent<IProperty>();
            if (potentialTarget == null) return;
            var a = potentialTarget.Itself.position;
            if (Vector3.Distance(transform.position, a) <= detectionRadius)
            {
                if (potentialTarget != null && potentialTarget != unit)
                {
                    OnTargetFound?.Invoke(potentialTarget);
                }
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
