using System;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private float damageAmount;

    private IProperty property;
    private Health cachedTargetHealth;

    public event Action OnPursueNewTarget;

    void Awake()
    {
        property = GetComponent<IProperty>();
    }

    public void Damage()
    {
        var target = property.Target;

        if (target == null)
        {
#if UNITY_EDITOR
            // Optional: log only in editor to avoid GC in builds
            Debug.Log("No target - no damage");
#endif
            return;
        }

        // Cache health component to avoid repeated GetComponent allocations
        if (cachedTargetHealth == null || cachedTargetHealth.gameObject != target.Itself)
        {
            cachedTargetHealth = target.Itself.GetComponent<Health>();
        }

        cachedTargetHealth.ModifyHealth(-damageAmount);

        if (!cachedTargetHealth.IsAlive)
        {
            cachedTargetHealth = null; // Clear cached reference
            //PursureNewTarget();
        }
    }

    public void PursureNewTarget()
    {
        OnPursueNewTarget?.Invoke();
    }
}
