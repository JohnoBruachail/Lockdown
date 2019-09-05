using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour {

	//DONT WANT THIS, WANT AN OVERSEEING MANAGER THAT SEARCHES FOR AND STORES EACH BREACH AS ITS ADDED TO THE GAME, ORDERS THEM INTO A LIST AND TRIGGERS WHATEVER ONE IS AT THE TOP OF THE LIST
	//IE IS CLOSEST TO THE GENERATOR. AS EACH BREACH IS 

	public GameObject[] creatures;
	int numberOfEnemys;

	public void NewBreach(int breachesAge){

		numberOfEnemys = 1;
		//early game, mid game and late game enemy types
		if(breachesAge <= 20){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[0]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));
			}
		}
		else if(breachesAge <= 40){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[UnityEngine.Random.Range(0,1)]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));

			}
		}
		else if(breachesAge <= 60){
			for(int i = 0; i < numberOfEnemys; i++){

				GameObject instance = (GameObject) Instantiate(creatures[UnityEngine.Random.Range(0,2)]);
				instance.transform.position = gameObject.transform.position;
				instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 600));

			}
		}
	}
}
