using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager Instance;

	void Awake()
	{
		Instance = this;
	}

	public Text pointsDisplay;
	public Text strengthDisplay;
	public Text spideyDisplay;
	public Text levelDisplay;

	public void UpdateHitCount(float points) {
		pointsDisplay.text = "Points: " + points;
	}

	public void UpdateStrength(float strength) {
		strengthDisplay.text = "Strength: " + strength;
	}


	// Reference for code that gets text to pop up and go away
	// How to make text pop up for a few seconds? - Unity Answers. (2016). Answers.unity3d.com. 
	// Retrieved 15 December 2016, from http://answers.unity3d.com/questions/532086/how-to-make-text-pop-up-for-a-few-seconds.html
	public IEnumerator NewLevel (float level) {
		levelDisplay.text = "You are now on Level " + level;
		levelDisplay.enabled = true;
		yield return new WaitForSeconds (2);
		levelDisplay.enabled = false;
	}

	public IEnumerator SpideyDamage (float spideyHealth) {
		spideyDisplay.text = "Spidey health: " + spideyHealth;
		spideyDisplay.enabled = true;
		yield return new WaitForSeconds (2);
		spideyDisplay.enabled = false;
	}

	public IEnumerator Eaten (string message) {
		spideyDisplay.text = message;
		spideyDisplay.enabled = true;
		yield return new WaitForSeconds (2);
		spideyDisplay.enabled = false;
	}
}
