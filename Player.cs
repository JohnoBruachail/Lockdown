using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	// Use this for initialization
	void Start () {

		rigidB = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
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
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.gameObject.CompareTag("EnergyOrb") && orbs < maxOrbs){
			orbs++;
			Destroy(other.gameObject.transform.parent.gameObject);
		}

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
    }

	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.CompareTag("Ladder")){
			//Enable ladder movement on character
			Debug.Log("Your off a ladder");
			onLadder = false;
			rigidB.gravityScale = 3;
		}
    }
}
