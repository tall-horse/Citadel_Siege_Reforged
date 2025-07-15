using System;
using System.Linq;
using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [SerializeField] private float maxHealth;
    [field: SerializeField] public bool IsAlive { get; private set; } = true;
    public event Action OnDead;
    void Start()
    {
        CurrentHealth = maxHealth;
    }
    public void ModifyHealth(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsAlive = false;
            OnDead?.Invoke();
            // Debug.Log("time to end in health");
            // FindObjectsByType<Unit>(FindObjectsSortMode.None).ToList().ForEach(a =>
            // {
            //     if (a.Target == GetComponent<IProperty>())
            //     {
            //         a.Target = null;
            //         a.Itself.GetComponent<Damager>().PursureNewTarget();
            //     }

            // });
        }
    }
}
