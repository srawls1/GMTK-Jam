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
    [SerializeField] private GameObject characterBullet;

    private Health health;
    private int currentAmmo;
    private float destinationTimeScale;

    void Awake()
    {
        health = GetComponent<Health>();
        destinationTimeScale = 1f;
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
                    currentAmmo = maxAmmo;
                }
            }
            else
            {
                health.TakeDamage(b.damage);
            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ChargeShot()
    {
        destinationTimeScale = minTimeScale;
        float timePassed = 0f;
        float ammoConsumed = 0f;

        while (Input.GetButton("Shoot"))
        {
            ammoConsumed += ammoConsumptionRate * Time.deltaTime;
            destinationTimeScale += timeDilationDecayRate * Time.deltaTime;
            destinationTimeScale = Mathf.Min(destinationTimeScale, 1f);
            timePassed += Time.deltaTime;

            yield return null;

            if (timePassed >= chargeTimeDamageThreshold)
            {
                health.TakeDamage(chargeTooLongDamage * Time.deltaTime);
            }

            if (ammoConsumed >= currentAmmo)
            {
                break;
            }
        }

        int wholeAmmoConsumed = Mathf.RoundToInt(ammoConsumed);
        currentAmmo -= wholeAmmoConsumed;

        if (wholeAmmoConsumed > 0)
        {
            GameObject bullet = Instantiate(characterBullet, bulletPosition.position, bulletPosition.rotation);
            // TODO Set the bullet damage based on ammoConsumed
        }

        destinationTimeScale = 1f;
    }
}
