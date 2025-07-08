using System;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    // private Animator animator;
    private IProperty property;
    public event Action OnPursueNewTarget;
    void Awake()
    {
        // animator = GetComponent<Animator>();
        property = GetComponent<IProperty>();
    }
    public void Damage()
    {
        if (property.Target == null)
        {
            Debug.Log("No target - no damage");
            return;
        }
        //OnDamageDealt?.Invoke();
        var targetHealth = property.Target.Itself.GetComponent<Health>();
        targetHealth.ModifyHealth(-damageAmount);
        if (targetHealth.IsAlive == false)
        {
            PursureNewTarget();
            //find another target and switch to Walk state
        }
    }
    public void PursureNewTarget()
    {
        OnPursueNewTarget?.Invoke();
    }
}
