using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

	// POINTS REQUIRED TO REACH EACH LEVEL
	public float L1 = 0;
	public float L2 = 500;
	public float L3 = 1000;
	public float L4 = 1500;
	public float L5 = 2000;


	// INITIAL POINTS
	private int points = 0;
	private int pointsGained;
	private int level = 1;


	// MOVEMENT VARIABLES
	private float horizontal;
	private float vertical;

	private Rigidbody2D rb;
	private Vector2 userInput;

	public float movementForce = 1f;
	public float rotationSpeed = 100f;


	// STATS
	public int agility;
	public int strength;

	// agility grows with each level
	public float getAgility (int agility, int XP) {
		if (XP < L2) { agility = 4f; } 
		else if (XP >= L2 && (XP < L3)) { agility = 6f; } 
		else if (XP >= L3 && (XP < L4)) { agility = 8f; } 
		else { agility = 10f; } 
	}

	// strength grows with each level
	public float getStrength (int agility, int XP) {
		if (XP < L2) { strength = 20f; } 
		else if (XP >= L2 && (XP < L3)) { strength = 40f; } 
		else if (XP >= L3 && (XP < L4)) { strength = 60f; } 
		else { strength = 80f; } 
	}

	// speed and damage are based on agility and strength
	public float movementSpeed = Mathf.FloorToInt(getAgility(agility, points) * 0.5f);
	public float damage = Mathf.FloorToInt(getAgility(agility, points) * 0.3f);






	// return how many points you need to reach the next level
	public int pointsReqdForNextLevel(int XP) {
		// if you have less than 500 points
		// you're on level 1
		if (XP < L2) {
			return (Mathf.RoundToInt (L2) - XP);
		} 
		// between 500 and 1000 points is level 2
		else if (XP >= L2 && (XP < L3)) {
			return (Mathf.RoundToInt (L3) - XP);
		} 
		// between 1000 and 1500 points is level 3
		else if (XP >= L3 && (XP < L4)) {
			return (Mathf.RoundToInt (L4) - XP);
		} 
		// between 1500 and 2000 points is level 4
		else {
			return (Mathf.RoundToInt (L5) - XP);
		} 
	}

	// return current level
	public int newPoints(int XP, int currentLevel, int XPgained){
		// new current points
		int currentXP = XP + XPgained;

		//if your current points is higher than those required to level up
		if (XPgained >= (pointsReqdForNextLevel(XP))) {
			return ++currentLevel;
		} else {
			return currentLevel;
		}
	}








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

		//rotate the ladybug
		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		transform.Rotate (rotationDelta);

	}


	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("Collided with "+ collision.collider.name);

		// if the bug hits a wall or leaf - make the sound
		if (collision.collider.CompareTag (Tags.Wall)) {
			SoundController.OnHitWall ();
		}
			
		// if the bug hits an aphid
		// destroy the aphid and earn points
		if (collision.collider.CompareTag (Tags.Points)) {
			GameObject.Destroy (collision.collider.gameObject);
			Debug.Log ("All the aphid points!!!");
			SoundController.OnPowerUp ();

			// points gained
			pointsGained = 100;
			// get current level
			int currentLevel = newPoints (points, level, pointsGained);
			// add new points on
			points = points + pointsGained;
			Debug.Log ("You now have " + points);

		}

		// if the bug hits a spider
		// start again
		if (collision.collider.CompareTag (Tags.Obstacle))  {
			Debug.Log ("You were eaten");
			SoundController.OnEatenBySpider ();

			//you died
			int pointsToLeveLUp = pointsReqdForNextLevel(points);
			points = (points + pointsToLeveLUp) - 500;
			Debug.Log ("You now have " + points);
		}

		if (collision.collider.CompareTag (Tags.Evil)) {
			Debug.Log ("Battle time!");

			string attacker = collision.collider.gameObject;


		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Debug.Log("Entered trigger zone "+ collider.name);
	}

}
