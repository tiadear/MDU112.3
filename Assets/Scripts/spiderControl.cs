using UnityEngine;
using System.Collections;

public class spiderControl : MonoBehaviour {

	// Reference for this bit of code
	// GameGrind. (2014, January 21). Making a Simple Game in Unit (Part 2) - Unity C# Tutorial [Video file]. Retrieved from https://www.youtube.com/watch?v=PqRA3fXVJ6M&t=1100s

	private int pos;
	private float speed;
	public Transform[] points; 

	public void setSpideySpeed(float currentLevel) {
		speed = currentLevel * 1.5f;
		Debug.Log ("set spidey speed: " + speed);
	}

	// Use this for initialization
	void Start () {
		transform.position = points[0].position;
		pos = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("spidey speed: " + speed);

		if (transform.position == points[pos].position) {
			++pos;
		}
		if (pos >= points.Length) {
			pos = 0;
		}
		transform.position = Vector3.MoveTowards (transform.position, points[pos].position, speed * Time.deltaTime);
	}
}
