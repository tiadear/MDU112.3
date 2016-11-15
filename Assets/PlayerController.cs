using UnityEngine;
using System.Collections;

// The line directly below forces the game object to have a Rigidbody 2D
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

	private Rigidbody2D rb;

	private Vector2 userInput;
	public float movementForce = 10f;
	public float movementSpeed = 2f;
	public float rotationSpeed = 50f;

	void Start () {

		// ask our game for the rigidbody 2d component
		rb = gameObject.GetComponent<Rigidbody2D>();

	}

	void Update () {
		
		// retrieve the axes
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		//Debug.Log ("H: " + horizontal + ", V: " + vertical);

		// store axes in the user input variable
		userInput = new Vector2 (horizontal, vertical);

		// Method 3 - part a (rotation)
		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		transform.Rotate (rotationDelta);
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

		// Method 3 - part b (movement)
		rb.AddForce(transform.up * userInput.y * movementForce);

	}
}
