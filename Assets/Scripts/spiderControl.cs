using UnityEngine;
using System.Collections;

public class spiderControl : MonoBehaviour {

	private float horizontal = 1f;
	private float vertical = 1f;
	public float speed = 40f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 positionOffset = -transform.right * vertical * 5 * Time.fixedDeltaTime;
		transform.position = transform.position + positionOffset;

		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * speed * Time.deltaTime);
		transform.Rotate (rotationDelta);

	}
}
