﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private Health health;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        animator.SetFloat("Health", 1f);
        health.OnHealthChanged += SetDamageAnimation;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet b = other.GetComponent<Bullet>();
            health.TakeDamage(b.damage);
            animator.SetTrigger("Damage");
        }
    }

    void SetDamageAnimation(float currentHealth, int maxHealth)
    {
        animator.SetFloat("Health", currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            animator.SetFloat("Health", -1);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
