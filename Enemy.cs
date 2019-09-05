using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {

	//state changing includes current state and attack state

	public IState currentState;
	public float speed;
	public int collisionDamage;
	public bool movingRight;
	
	public Transform wallDetector;
	public Transform groundDetector;

	public RaycastHit2D wallCollisionInfo;
	public RaycastHit2D groundCollisionInfo;
	
	// Update is called once per frame
	void Update () {
		currentState.Update();

	}

	//consider making a new script for new enemys, this one never changes state.
	public void ChangeState(IState newState){
		if(currentState != null){
			currentState.Exit();
		}

		currentState = newState;
		currentState.Enter(this);
	}

	public void Alerted(){
		ChangeState(new RangedAttackState());
	}

	public void Relax(){
		ChangeState(new IdleState());
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Bullet") {

			ChangeHP(-other.gameObject.GetComponent<BulletBehavior>().damage);
			Destroy(other.gameObject);
    	}
    }
}