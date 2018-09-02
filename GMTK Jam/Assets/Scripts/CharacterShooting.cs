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
    [SerializeField] private Transform bulletPivot;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject[] chargedShots;

    private Health health;
    private Animator animator;
    private int currentAmmo;
    private float destinationTimeScale;
    private bool interrupted;
    private bool usingJoystick;

    public event Action<int, int> OnAmmoChanged;

    public bool charging
    {
        get; private set;
    }

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

    public Quaternion GetLaserRotation()
    {
        bool currentInput = false;
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (!Mathf.Approximately(mouseMovement.magnitude, 0))
        {
            usingJoystick = false;
        }
        Vector2 aimInput = new Vector2(Input.GetAxis("AimX"), Input.GetAxis("AimY"));
        if (Mathf.Abs(aimInput.magnitude) >= 0.2f)
        {
            usingJoystick = true;
            currentInput = true;
        }

        if (!usingJoystick)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimInput = new Vector2(worldMousePosition.x - transform.position.x,
                                    worldMousePosition.y - transform.position.y)
                                    .normalized;
        }
        if (usingJoystick && !currentInput)
        {
            return bulletPivot.rotation;
        }

        float aimRotation = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, aimRotation);
    }

    IEnumerator ChargeShot()
    {
        interrupted = false;
        destinationTimeScale = minTimeScale;
        float timePassed = 0f;
        float ammoConsumed = 1f;

        charging = true;

        while (Input.GetButton("Shoot") && !interrupted)
        {
            ammoConsumed += ammoConsumptionRate * Time.deltaTime;
            destinationTimeScale += timeDilationDecayRate * Time.deltaTime;
            destinationTimeScale = Mathf.Min(destinationTimeScale, 1f);

            animator.SetFloat("ChargeLevel", ammoConsumed);

            yield return null;

            Quaternion aimRotation = GetLaserRotation();
            bulletPivot.rotation = aimRotation;
            animator.SetFloat("Rotation", aimRotation.eulerAngles.z);
            timePassed += Time.deltaTime;

            if (timePassed >= chargeTimeDamageThreshold)
            {
                health.TakeDamage(chargeTooLongDamage * Time.deltaTime);
            }

            if (ammoConsumed >= currentAmmo)
            {
                break;
            }

            if (ammoConsumed >= chargedShots.Length)
            {
                ammoConsumed = chargedShots.Length;
            }
        }

        int wholeAmmoConsumed = Mathf.RoundToInt(ammoConsumed);
        currentAmmo -= wholeAmmoConsumed;
        charging = false;

        animator.SetFloat("ChargeLevel", 0f);

        OnAmmoChanged(currentAmmo, maxAmmo);

        if (wholeAmmoConsumed > 0 && !interrupted)
        {
            Instantiate(chargedShots[wholeAmmoConsumed - 1], bulletPosition.position, bulletPosition.rotation);
        }

        destinationTimeScale = 1f;
    }
}
