using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

	public float scaleX = 0.1f;
	public float scaleY = 0.1f;

	private Rigidbody2D rb;
	private Vector2 userInput;

	//public float movementForce = 10f;
	//public float movementSpeed = 2f;
	//public float rotationSpeed = 50f;

	void Start () {
		// retrieve rigidbody for player
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update () {

		// retrieve the axes
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		// store axes in the user input variable
		userInput = new Vector2 (horizontal, vertical);

		//transform.position = transform.position + new Vector3 (userInput.x * scaleX, userInput.y * scaleY, 0.0f);

		// Method 3 - part a (rotation)
		//Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		//transform.Rotate (rotationDelta);
	}

	void FixedUpdate() {

		// all physics changes should occur here
		// bare minimum code goes here

		// Method 1 of moving by physics
		//rb.AddForce(userInput * movementForce);

		// Method 2 - teleport with collision checks
		//Vector2 newPosition = transform.position;
		//newPosition += userInput * movementSpeed * Time.fixedDeltaTime;
		//rb.MovePosition (newPosition);
		rb.MovePosition (transform.position + new Vector3 (userInput.x * scaleX, userInput.y * scaleY, 0));

		// Method 3 - part b (movement)
		//rb.AddForce(transform.up * userInput.y * movementForce);

	}

	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("Collided with "+ collision.collider.name);

		// check if this is an obstacle
		if (collision.collider.CompareTag (Tags.Obstacle)) {
			// tag match
			// destroy obstacle
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
