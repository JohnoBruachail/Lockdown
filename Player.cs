using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Use this for initialization
	private bool invincibilityFrames;
	private Display display;
	private Mine mine;
	IInteractable interactableObject;
	//public Rigidbody2D rigidB;
	//public bool onLadder;
	void Start () {

		//rigidB = GetComponent<Rigidbody2D>();
		display = GameObject.Find("Display").GetComponent<Display>();
	}

	void Update(){

		//If the player is next to an interactable object and presses f they will interact with it
		if(interactableObject != null){
			if(Input.GetKeyDown(KeyCode.F)){
				//ok heres an issue, if the object is a mine we need to send an amount of crystals
				//if its not then we dont, how will this work?
				interactableObject.Interact();

				display.UpdateCrystals(eCrystals);
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
// OK THIS IS SHIT, SHOULDNT BE CHECKING THIS EVERY FRAME, IT MAKES FAR MORE SENSE TO HAVE SEPERATE STATES FOR THE CHARACTER
// AND FOR THE STATE TO DICTATE THE ACTIVITY OF THE CHARACTER. COLLIDING WITH A LADDER SHOULD CHANGE THEIR STATE.

//this shouldnt even be here, it should be in the movement system.
/* 
		if(onLadder){

			//rigidB.velocity = new Vector2(0,0);

			if(Input.GetKeyDown(KeyCode.W)){
				Debug.Log("Going up");
				rigidB.velocity = new Vector2(rigidB.velocity.x, 8);
			}
			if(Input.GetKeyUp(KeyCode.W)){
				Debug.Log("Stop Going up");
				rigidB.velocity = new Vector2(rigidB.velocity.x, 0);
			}
			if(Input.GetKeyDown(KeyCode.S)){
				Debug.Log("Going down");
				rigidB.velocity = new Vector2(rigidB.velocity.x, -8);
			}
			if(Input.GetKeyUp(KeyCode.S)){
				Debug.Log("Stop Going up");
				rigidB.velocity = new Vector2(rigidB.velocity.x, 0);
			}
		}
*/
	}





//UPDATE THE REFERENCES TO THE ENERGY ORBS AS ENERFY CRYSTALS
//update collisions so interacting with enemys causes damage.
	void OnTriggerEnter2D(Collider2D other){
		Debug.Log("Player collider triggered, the object is " + other);

		if(other.gameObject.CompareTag("Enemy")){
			Debug.Log("Enemy tag triggered");
			DamagePlayer(other.GetComponent<Enemy>().collisionDamage);
		}

		if(other.gameObject.CompareTag("EnergyCrystal") && eCrystals < eCrystalsCapacity){
			Debug.Log("collided with EnergyCrystal");
			eCrystals++;
			display.UpdateCrystals(eCrystals);
			Destroy(other.gameObject.transform.parent.gameObject);
		}

		if(other.gameObject.CompareTag("Interactable")){
			interactableObject = other.GetComponent<IInteractable>();
		}

/* 
		if(other.gameObject.CompareTag("Ladder")){
			//Enable ladder movement on character
			Debug.Log("Your on a ladder");
			onLadder = true;
			rigidB.gravityScale = 0;
		}

		if(other.gameObject.CompareTag("DataChip")){
			//Give the player a random data chip from a list of data chips.
			Destroy(other.gameObject);
		}
*/
    }

	void OnTriggerExit2D(Collider2D other){
/* 
		if(other.gameObject.CompareTag("Ladder")){
			//Enable ladder movement on character
			Debug.Log("Your off a ladder");
			onLadder = false;
			rigidB.gravityScale = 3;
		}
*/	
		if(other.gameObject.CompareTag("Interactable")){
			interactableObject = null;
		}
    }

	void DamagePlayer(int damage){
		if(invincibilityFrames == false){
			ChangeHP(-damage);
			display.UpdateHealth(currentHP);
			invincibilityFrames = true;
			Invoke("DisableInvincibilityFrames", 3.0f);
		}
	}

	void DisableInvincibilityFrames(){
		invincibilityFrames = false;
	}
}
