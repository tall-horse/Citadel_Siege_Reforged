using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    private bool isAlive = true;
    public event Action OnDead;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
            OnDead?.Invoke();
        }
    }
}
