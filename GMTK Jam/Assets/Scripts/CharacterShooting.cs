using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{

    [SerializeField] private int maxAmmo;
    [SerializeField] private float ammoConsumptionRate;
    [SerializeField] private float chargeTimeDamageThreshold;
    [SerializeField, Range(0f, 1f)] private float minTimeScale;
    [SerializeField, Range(0f, 1f)] private float timeDilationSmoothing;
    [SerializeField] private float timeDilationDecayRate;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject characterBullet;

    private int currentAmmo;
    private float destinationTimeScale;

    void Awake()
    {
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
                    // TODO damage
                    Debug.Log("Taking damage from too much ammo");
                    currentAmmo = maxAmmo;
                }
            }
            else
            {
                // TODO damage
                Debug.Log("Taking damage from bullet");
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
                // TODO damage
                Debug.Log("Taking damage from charging too long");
            }

            if (ammoConsumed >= currentAmmo)
            {
                break;
            }
        }

        currentAmmo -= Mathf.RoundToInt(ammoConsumed);
        GameObject bullet = Instantiate(characterBullet, bulletPosition.position, bulletPosition.rotation);
        // TODO Set the bullet damage based on ammoConsumed

        destinationTimeScale = 1f;
    }
}
