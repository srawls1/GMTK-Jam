using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour {

    [SerializeField] private CharacterShooting target;

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

	void Start()
    {
		target.OnAmmoChanged += UpdateAmmoDisplay;
	}

    void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        float portionFill = (float)currentAmmo / maxAmmo;
        slider.value = portionFill;
    }

}
