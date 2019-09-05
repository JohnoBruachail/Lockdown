using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLeaper : Enemy {

	void Start () {
		movingRight = true;
		ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update () {
		currentState.Update();
	}

	public void Alerted(){
		ChangeState(new AttackState());
	}

	public void Relax(){
		ChangeState(new IdleState());
	}

	//leaper is going to have a state change if he can see the player
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Bullet") {

			ChangeHP(-other.gameObject.GetComponent<BulletBehavior>().damage);
			Destroy(other.gameObject);
    	}
    }
}