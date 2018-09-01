using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private Health target;

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

	void Start()
    {
		target.OnHealthChanged += UpdateHealthDisplay;
	}

    void UpdateHealthDisplay(float currentHealth, int maxHealth)
    {
        float portionFill = currentHealth / maxHealth;
        slider.value = portionFill;
    }

}
