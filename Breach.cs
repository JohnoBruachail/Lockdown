using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour {

	public GameObject[] creatures;

	int age = 0;
	int numberOfEnemys;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Trigger(){

		numberOfEnemys = 1;
		//UnityEngine.Random.Range(age, (age+5));

		//early game, mid game and late game enemy types
		if(age <= 20){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[0]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));
			}
		}
		else if(age <= 40){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[UnityEngine.Random.Range(0,1)]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));

			}
		}
		else if(age <= 60){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[UnityEngine.Random.Range(0,2)]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));

			}
		}

		age++;
	}
}
