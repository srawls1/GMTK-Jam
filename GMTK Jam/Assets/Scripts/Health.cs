using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string DamageSoundString;
    FMOD.Studio.EventInstance DamageEvent;

    [SerializeField] private int maxHealth;
    [SerializeField] bool isPlayer;

    public float currentHealth;

    public event Action<float, int> OnHealthChanged;
    public event Action OnDeath;

    void Start()
    {
        OnHealthChanged += PrintHealth;
        currentHealth = maxHealth;
    }

    void PrintHealth(float current, int max)
    {
        //Debug.Log(string.Format("HP: {0}/{1}", current, max));
    }

	public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        FMODUnity.RuntimeManager.CreateInstance(DamageSoundString).start();
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
        if (OnDeath != null)
        {
            OnDeath();
        }
    }
}
