using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {

    [SerializeField] List<string> tutorialTexts;
    [SerializeField] float textDuration;

    Text textDisplay;

	// Use this for initialization
	IEnumerator Start () {
		textDisplay = GetComponent<Text>();

        foreach (string tutorial in tutorialTexts)
        {
            textDisplay.text = tutorial;
            yield return new WaitForSeconds(textDuration);
        }

        textDisplay.gameObject.SetActive(false);
	}
}
