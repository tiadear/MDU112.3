using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

	//public float scaleX = 0.1f;
	//public float scaleY = 0.1f;

	private float horizontal;
	private float vertical;

	private Rigidbody2D rb;
	//private Vector2 userInput;

	//public float movementForce = 1f;
	public float movementSpeed = 5f;
	public float rotationSpeed = 100f;

	void Start () {
		// retrieve rigidbody for player
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update () {

		// get the axes
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");

		// store axes in the user input variable
		//userInput = new Vector2 (horizontal, vertical);
	}

	void FixedUpdate() {

		// move the ladybug
		Vector3 positionOffset = transform.right * vertical * movementSpeed * Time.fixedDeltaTime;
		rb.MovePosition (transform.position + positionOffset);

		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		transform.Rotate (rotationDelta);

	}
		



	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("Collided with "+ collision.collider.name);

		// check if the collider is an obstacle
		// if so destroy it
		if (collision.collider.CompareTag (Tags.Obstacle)) {
			GameObject.Destroy (collision.collider.gameObject);
			Debug.Log ("Leaf destroyed!");
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		Debug.Log("Stopped colliding with "+ collision.collider.name);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Debug.Log("Entered trigger zone "+ collider.name);
	}

	void OnTriggerExit2D (Collider2D collider) {
		Debug.Log("Left trigger zone "+ collider.name);
	}

}
