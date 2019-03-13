using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

	private PlatformEffector2D effector;
	private float waitTime;

	void Start () {
		//Debug.Log("Script running");
		effector = GetComponent<PlatformEffector2D>();
	}

	void Update () {

		if(Input.GetKeyUp(KeyCode.S)){
			//Debug.Log("Test One Way Platform");
			waitTime = 0.1f;
		}

		if(Input.GetKey(KeyCode.S)){
			//Debug.Log("Test One Way Platform");
			if(waitTime <= 0){
				effector.rotationalOffset = 180f;
				waitTime = 0.1f;
			}
			else{
				waitTime -= Time.deltaTime;
			}
		}

		if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)){
			effector.rotationalOffset = 0;
		}

	}


}
