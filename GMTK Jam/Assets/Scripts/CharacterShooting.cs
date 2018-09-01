using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{

    [SerializeField] private int maxAmmo;
    [SerializeField] private float ammoConsumptionRate;
    [SerializeField] private float chargeTimeDamageThreshold;
    [SerializeField] private float tooMuchAmmoDamage;
    [SerializeField] private bool multiplyTooMuchAmmoDamageByBulletDamage;
    [SerializeField, Range(0f, 1f)] private float minTimeScale;
    [SerializeField, Range(0f, 1f)] private float timeDilationSmoothing;
    [SerializeField] private float timeDilationDecayRate;
    [SerializeField] private float chargeTooLongDamage;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject[] chargedShots;

    private Health health;
    private Animator animator;
    private int currentAmmo;
    private float destinationTimeScale;
    private bool interrupted;

    public event Action<int, int> OnAmmoChanged;

    void Awake()
    {
        OnAmmoChanged += PrintAmmo;
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        destinationTimeScale = 1f;
    }

    void PrintAmmo(int current, int max)
    {
        //Debug.Log(string.Format("Ammo: {0}/{1}", current, max));
    }

	void Update()
    {
		if (Input.GetButtonDown("Shoot"))
        {
            StartCoroutine(ChargeShot());
        }

        Time.timeScale = Mathf.Lerp(Time.timeScale, destinationTimeScale, timeDilationSmoothing);
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet b = collision.gameObject.GetComponent<Bullet>();
            if (b.collectable)
            {
                ++currentAmmo;
                if (currentAmmo > maxAmmo)
                {
                    float damage = tooMuchAmmoDamage;
                    if (multiplyTooMuchAmmoDamageByBulletDamage)
                    {
                        damage *= b.damage;
                    }
                    health.TakeDamage(damage);
                    Interrupt();
                    currentAmmo = maxAmmo;
                }
                OnAmmoChanged(currentAmmo, maxAmmo);
            }
            else
            {
                health.TakeDamage(b.damage);
                Interrupt();
            }
            Destroy(collision.gameObject);
        }
    }

    public void Interrupt()
    {
        interrupted = true;
        animator.SetFloat("ChargeLevel", 0f);
    }

    IEnumerator ChargeShot()
    {
        interrupted = false;
        destinationTimeScale = minTimeScale;
        float timePassed = 0f;
        float ammoConsumed = 1f;

        while (Input.GetButton("Shoot") && !interrupted)
        {
            ammoConsumed += ammoConsumptionRate * Time.deltaTime;
            destinationTimeScale += timeDilationDecayRate * Time.deltaTime;
            destinationTimeScale = Mathf.Min(destinationTimeScale, 1f);

            animator.SetFloat("ChargeLevel", ammoConsumed);

            yield return null;

            if (timePassed >= chargeTimeDamageThreshold)
            {
                health.TakeDamage(chargeTooLongDamage * Time.deltaTime);
            }

            if (ammoConsumed >= chargedShots.Length)
            {
                ammoConsumed = chargedShots.Length;
            }
        }

        int wholeAmmoConsumed = Mathf.RoundToInt(ammoConsumed);
        currentAmmo -= wholeAmmoConsumed;

        animator.SetFloat("ChargeLevel", 0f);

        OnAmmoChanged(currentAmmo, maxAmmo);

        if (wholeAmmoConsumed > 0 && !interrupted)
        {
            GameObject bullet = Instantiate(chargedShots[wholeAmmoConsumed - 1], bulletPosition.position, bulletPosition.rotation);
        }

        destinationTimeScale = 1f;
    }
}
