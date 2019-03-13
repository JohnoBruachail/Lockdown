using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : NPC {

	bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void stateCheck() {

	}

	void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){

			gameObject.transform.Find("Power Up").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){

			gameObject.transform.Find("Power Up").gameObject.SetActive(false);
        }
    }
}
