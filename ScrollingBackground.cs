using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Panels
{
    public GameObject panel;
}

public class ScrollingBackground : MonoBehaviour {

	public int panelHeight;
	public int panelWidth;
	public Panels[] panels;

	private int upBarrier;
	private int rightBarrier;
	private int downBarrier;
	private int leftBarrier;
	private Transform camTransform;			//Main camera transform.
	private Vector3 temp;

	void Start () {
		// set up camera 
		camTransform = Camera.main.transform;
		upBarrier = (int)camTransform.position.y + panelHeight;
		rightBarrier = (int)camTransform.position.x + panelWidth;
		downBarrier = (int)camTransform.position.y - panelHeight;
		leftBarrier = (int)camTransform.position.x - panelWidth;
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log("Camera's position is: " + camTransform.position + " Is it less then " + (camTransform.position.x - panelWidth));

		if(camTransform.position.y > upBarrier){
			ScrollUp();
		}
		if(camTransform.position.x > rightBarrier){
			ScrollRight();
		}
		if(camTransform.position.y < downBarrier){
			ScrollDown();
		}
		if(camTransform.position.x < leftBarrier){
			ScrollLeft();
		}
	}

	private void ScrollUp(){
		for(int i = 0; i < 9; i++){
			temp = panels[i].panel.transform.position;
			temp.y += panelHeight;
			panels[i].panel.transform.position = temp;
		}
		upBarrier += panelHeight;
		downBarrier += panelHeight;
	}
	private void ScrollRight(){
		for(int i = 0; i < 9; i++){
			temp = panels[i].panel.transform.position;
			temp.x += panelWidth;
			panels[i].panel.transform.position = temp;
		}
		rightBarrier += panelWidth;
		leftBarrier += panelWidth;
	}
	private void ScrollDown(){
		for(int i = 0; i < 9; i++){
			temp = panels[i].panel.transform.position;
			temp.y -= panelHeight;
			panels[i].panel.transform.position = temp;
		}
		downBarrier -= panelHeight;
		upBarrier -= panelHeight;
	}
	private void ScrollLeft(){
		for(int i = 0; i < 9; i++){
			temp = panels[i].panel.transform.position;
			temp.x -= panelHeight;
			panels[i].panel.transform.position = temp;
		}
		leftBarrier -= panelWidth;
		rightBarrier -= panelWidth;
	}
}
