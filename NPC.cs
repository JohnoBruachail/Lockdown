using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

	public int currentHP = 5;
	public int maxHP = 5;
	public bool hasanorb;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

	public void AddjustCurrentHealth(int change){
		Debug.Log("Change: " + change);

		currentHP += change;

		Debug.Log("currentHP: " + currentHP);

		if(currentHP <= 0){
			Destroy(gameObject);
		}
	}
}
