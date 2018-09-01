using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private float currentHealth;

    public event Action<float, int> OnHealthChanged;

    void Start()
    {
        OnHealthChanged += PrintHealth;
        currentHealth = maxHealth;
    }

    void PrintHealth(float current, int max)
    {
        Debug.Log(string.Format("HP: {0}/{1}", current, max));
    }

	public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        OnHealthChanged(currentHealth, maxHealth);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Dead");
    }
}
