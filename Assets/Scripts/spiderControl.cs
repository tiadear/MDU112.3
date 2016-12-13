using UnityEngine;
using System.Collections;

public class spiderControl : MonoBehaviour {
	
	private int pos;
	public float speed = 2f;
	public Transform[] points; 

	// Use this for initialization
	void Start () {
		transform.position = points[0].position;
		pos = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position == points[pos].position) {
			++pos;
		}
		if (pos >= points.Length) {
			pos = 0;
		}
		transform.position = Vector3.MoveTowards (transform.position, points[pos].position, speed * Time.deltaTime);
	}
}
