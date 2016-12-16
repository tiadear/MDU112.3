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
	private float currentlevel;
	private string currentscene;


	// MOVEMENT VARIABLES
	private float horizontal;
	private float vertical;

	private Rigidbody2D rb;
	private Vector2 userInput;

	public float movementForce = 1f;
	public float rotationSpeed = 100f;


	public GameObject spider;
	private float spideyHealth;
	private LevelLoader ll;


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
		if (XP < L2) { return 40f; } // on level 1
		else if (XP >= L2 && (XP < L3)) { return 60f; } // on level 2
		else if (XP >= L3 && (XP < L4)) { return 80f; } // on level 3
		else { return 1000f; } // on level 4
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

			// get the next level
			float newlevel = ++currentLevel;

			// what to do if this is the final level
			if (newlevel == 4) {
				ll.LoadLevel ("EndGame");
				return newlevel;
			}
			else {
				// start next scene
				StartCoroutine(UIManager.Instance.NewLevel(newlevel));
				string strlevel = newlevel.ToString();
				StartCoroutine(ll.LoadNextLevel (strlevel));
				return newlevel;
			}
		} 

		else {
			return currentLevel;
		}
	}








	void Start () {
		// retrieve rigidbody for player
		rb = gameObject.GetComponent<Rigidbody2D>();

		// get the levelloader script
		ll = FindObjectOfType<LevelLoader>();

		//return current level
		currentscene = ll.CurrentLevel(); // 1, 2, 3, etc
		currentlevel = float.Parse(currentscene); // convert to float

		// set points for that level
		// get the strength value for that level
		// update the points on the UI for that level
		// update the strength on the UI for that level
		if (currentlevel == 1) {
			points = L1;
			strength = getStrength (points);
			UIManager.Instance.UpdateHitCount (points);
			UIManager.Instance.UpdateStrength (strength);
			agility = getAgility (points);
			UIManager.Instance.UpdateAgility (agility);
		} 
		else if (currentlevel == 2) {
			transform.localScale += new Vector3 (0.3f, 0.3f, 0);
			points = L2;
			UIManager.Instance.UpdateHitCount (points);
			strength = getStrength (points);
			UIManager.Instance.UpdateStrength (strength);
			agility = getAgility (points);
			UIManager.Instance.UpdateAgility (agility);
		} 
		else if (currentlevel == 3) {
			transform.localScale += new Vector3 (1f, 1f, 0);
			points = L3;
			UIManager.Instance.UpdateHitCount (points);
			spideyHealth = 1000f;
			strength = getStrength (points);
			UIManager.Instance.UpdateStrength (strength);
			agility = getAgility (points);
			UIManager.Instance.UpdateAgility (agility);
		} 

		// send new level data to spidey control
		spiderControl spiderControl = spider.GetComponent<spiderControl>();
		spiderControl.setSpideySpeed (currentlevel);
	}




	void Update () {

		// get the axes
		horizontal = Input.GetAxis ("Horizontal");
		vertical = Input.GetAxis ("Vertical");
	}




	void FixedUpdate() {

		// speed based on agility which goes up with each level
		float movementSpeed = getAgility(points) * 0.5f;

		// move the ladybug
		Vector3 positionOffset = transform.right * vertical * movementSpeed * Time.fixedDeltaTime;
		rb.MovePosition (transform.position + positionOffset);

		//rotate the ladybug
		Vector3 rotationDelta = new Vector3(0, 0, -horizontal * rotationSpeed * Time.deltaTime);
		transform.Rotate (rotationDelta);

	}



	void OnCollisionEnter2D(Collision2D collision) {
		float damage = getStrength(points) * 1.2f;

		// if the bug hits a wall or leaf - make the sound
		if (collision.collider.CompareTag (Tags.Wall)) {
			SoundController.OnHitWall ();
		}
			
		// if the bug hits a spider
		if (collision.collider.CompareTag (Tags.Obstacle))  {
			float pointsToLeveLUp = pointsReqdForNextLevel(points);

			//if you're strong enough you can battle the spider
			// otherwise you get eaten
			if (strength < 80f) {
				//you died
				SoundController.OnEatenBySpider ();
				StartCoroutine(UIManager.Instance.Eaten("You were eaten"));

				// reset your points for that level
				points = (points + pointsToLeveLUp) - 500;
				UIManager.Instance.UpdateHitCount(points);

				// restart that scene
				StartCoroutine(ll.LoadNextLevel (currentscene));
			} 

			else {
				// battle the spider
				float spideyDamage = 80f;

				// update spidey health
				spideyHealth = spideyHealth - damage;
				StartCoroutine(UIManager.Instance.SpideyDamage(spideyHealth));

				// make spidey smaller
				spider.transform.localScale -= new Vector3(0.1f, 0.1f, 1);

				// if spidey's health goes below 0
				// he died
				if (spideyHealth <= 0) {
					GameObject.Destroy (collision.collider.gameObject);
					SoundController.OnPowerUp ();
					StartCoroutine(UIManager.Instance.Eaten("You destroyed the spider!"));

					// points gained
					pointsGained = 300;

					//level
					float level = newPoints(points, currentlevel, pointsGained);

					// add new points on
					points = points + pointsGained;
					UIManager.Instance.UpdateHitCount(points);
				}

			}
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		// if the bug hits an aphid
		// destroy the aphid and earn points
		if (collider.CompareTag (Tags.Points)) {
			GameObject.Destroy (collider.gameObject);
			SoundController.OnPowerUp ();

			// points gained
			pointsGained = 100;

			//level
			float level = newPoints(points, currentlevel, pointsGained);

			// add new points on
			points = points + pointsGained;
			UIManager.Instance.UpdateHitCount(points);
		}
	}

}
