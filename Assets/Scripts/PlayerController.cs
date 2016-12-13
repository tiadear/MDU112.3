using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

	// POINTS REQUIRED TO REACH EACH LEVEL
	private float L1 = 0f;
	private float L2 = 500f;
	private float L3 = 1000f;
	private float L4 = 1500f;
	private float L5 = 2000f;


	// INITIAL POINTS
	private float points;
	private float pointsGained;
	private float level = 1f;


	// MOVEMENT VARIABLES
	private float horizontal;
	private float vertical;

	private Rigidbody2D rb;
	private Vector2 userInput;

	public float movementForce = 1f;
	public float rotationSpeed = 100f;

	private int hits;


	// STATS
	public float agility;
	public float strength;

	// agility grows with each level
	public float getAgility (float XP) {
		if (XP < L2) { return 4f; } 
		else if (XP >= L2 && (XP < L3)) { return 6f; } 
		else if (XP >= L3 && (XP < L4)) { return 8f; } 
		else { return 10f; } 
	}

	// strength grows with each level
	public float getStrength (float XP) {
		if (XP < L2) { return 20f; } // on level 1
		else if (XP >= L2 && (XP < L3)) { return 40f; } // on level 2
		else if (XP >= L3 && (XP < L4)) { return 60f; } // on level 3
		else { return 80f; } // on level 4
	}







	// return how many points you need to reach the next level
	public float pointsReqdForNextLevel(float XP) {
		// if you have less than 500 points
		// you're on level 1
		if (XP < L2) {
			return (L2 - XP);
		} 
		// between 500 and 1000 points is level 2
		else if (XP >= L2 && (XP < L3)) {
			return (L3 - XP);
		} 
		// between 1000 and 1500 points is level 3
		else if (XP >= L3 && (XP < L4)) {
			return (L4 - XP);
		} 
		// between 1500 and 2000 points is level 4
		else {
			return (L5 - XP);
		} 
	}

	// return current level
	public float newPoints(float XP, float currentLevel, float XPgained){
		//if your current points is higher than those required to level up
		if (XPgained >= (pointsReqdForNextLevel(XP))) {
			float newlevel = ++currentLevel;
			Debug.Log ("You have moved up a level!!!");
			Debug.Log ("You are now on level " + newlevel);

			return newlevel;
		} else {
			return currentLevel;
		}
	}








	void Start () {
		// retrieve rigidbody for player
		rb = gameObject.GetComponent<Rigidbody2D>();
		points = 0;
		strength = getStrength(points);
		hits = 0;
	}

	void Update () {

		// get the axes
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");

		// store axes in the user input variable
		//userInput = new Vector2 (horizontal, vertical);
	}







	void FixedUpdate() {

		// speed and damage are based on agility and strength
		float movementSpeed = getAgility(points) * 0.5f;

		// move the ladybug
		Vector3 positionOffset = transform.right * vertical * movementSpeed * Time.fixedDeltaTime;
		rb.MovePosition (transform.position + positionOffset);

		//rotate the ladybug
		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		transform.Rotate (rotationDelta);

	}









	void OnCollisionEnter2D(Collision2D collision) {
		float damage = getStrength(points) * 0.8f;


		// if the bug hits a wall or leaf - make the sound
		if (collision.collider.CompareTag (Tags.Wall)) {
			SoundController.OnHitWall ();
		}
			
		// if the bug hits an aphid
		// destroy the aphid and earn points
		if (collision.collider.CompareTag (Tags.Points)) {
			GameObject.Destroy (collision.collider.gameObject);
			SoundController.OnPowerUp ();

			// points gained
			pointsGained = 100;
			// get current level
			level = newPoints (points, level, pointsGained);
			// add new points on
			points = points + pointsGained;
			Debug.Log ("You now have " + points + " points");
		}

		// if the bug hits a spider
		if (collision.collider.CompareTag (Tags.Obstacle))  {
			float pointsToLeveLUp = pointsReqdForNextLevel(points);

			//if you're strong enough you can battle the spider
			// otherwise you get eaten

			if (strength < 60f) {
				Debug.Log (strength);
				//you died
				Debug.Log ("You were eaten");
				SoundController.OnEatenBySpider ();
				points = (points + pointsToLeveLUp) - 500;
				Debug.Log ("You now have " + points + " points");

				Application.LoadLevel(Application.loadedLevel);
			} 

			else {
				Debug.Log (strength);
				// battle the spider
				float spideyHealth = 2000f;
				float spideyDamage = 80f;

				Debug.Log ("Battle time!");

				if (hits == 1) {
					points = points - spideyDamage;
					hits = 0;
					Debug.Log ("You were hit");
					Debug.Log ("You now have " + points + " points");
				} else {
					spideyHealth = spideyHealth - damage;
					++hits;
					Debug.Log ("You hit Spidey! ");
					Debug.Log ("Spidey now has " + points);
				}

				if (spideyHealth <= 0) {
					Debug.Log ("You defeated the spider");
					GameObject.Destroy (collision.collider.gameObject);
					points = points + 100;
					Debug.Log ("You now have " + points + " points");
				}
				if (points <= (points + pointsToLeveLUp) - 500) {
					//you died
					Debug.Log ("You were eaten");
					SoundController.OnEatenBySpider ();
					points = (points + pointsToLeveLUp) - 500;
					Debug.Log ("You now have " + points + " points");

					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Debug.Log("Entered trigger zone "+ collider.name);
	}

}
