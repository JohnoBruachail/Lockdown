using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	public float distance;

	private float inputHorizontal;
	private float inputVertical;

	private bool isClimbing;

	public LayerMask whatIsLadder;

	private Rigidbody2D rigidB;

	// Use this for initialization
	void Start () {
		rigidB = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		inputHorizontal = Input.GetAxisRaw("Horizontal");
		rigidB.velocity = new Vector2(inputHorizontal * speed, rigidB.velocity.y);

		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

		if(hitInfo.collider != null){
			if(Input.GetKeyDown(KeyCode.W)){

			}
		}
		else {
			isClimbing = false;
		}

		if(isClimbing == true){
			inputVertical = Input.GetAxisRaw("Vertical");
			rigidB.velocity = new Vector2(rigidB.position.x, inputVertical * speed);
			rigidB.gravityScale = 0;
		}
		else{
			rigidB.gravityScale = 5;
		}
	}
}
