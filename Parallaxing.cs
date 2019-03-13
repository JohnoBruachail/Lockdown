using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;	//array to store background elements.
	public float smoothing = 1f;	//How smooth the parallax is going to be.
	private float[] parallaxScales;	//How much camera movement dictaes the movement of the backgrounds.

	private Transform cam;			//Main camera transform.
	private Vector3 previousCamPos;	//Camera transform in the previous frame.
	
	void Awake(){
		// set up camera 
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {

		previousCamPos = cam.position;

		//assign corresponding parallaxscales
		parallaxScales = new float[backgrounds.Length];
		for(int i = 0; i < backgrounds.Length; i++){
			parallaxScales[i] = backgrounds[i].position.z*-1;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		//for each background
		for(int i = 0; i < backgrounds.Length; i++){
			//the parallaxing effect should be the difference between where the cam is now and where it was before.
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set a target x position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;
			
			//create a target potiion which is the backgrouinds current position with its target X position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		previousCamPos = cam.position;
	}
}
