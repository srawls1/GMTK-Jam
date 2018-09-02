using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFlashOnChange : MonoBehaviour
{
    [SerializeField] private int numFramesToFlash;

    Image barImage;
    bool flashing;

	// Use this for initialization
	void Start () {
		Slider bar = GetComponent<Slider>();
        barImage = bar.fillRect.GetComponent<Image>();
        bar.onValueChanged.AddListener(FlashBar);
	}

	void FlashBar(float value)
    {
        if (barImage != null)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    IEnumerator FlashRoutine()
    {
        if (flashing)
        {
            yield break;
        }

        flashing = true;
        Color startingColor = barImage.color;
        barImage.color = Color.white;

        for (int i = 0; i < numFramesToFlash; ++i)
        {
            yield return null;
        }

        barImage.color = startingColor;

        for (int i = 0; i < numFramesToFlash; ++i)
        {
            yield return null;
        }

        flashing = false;

    }
}
