using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScavenger : Enemy {

	void Start () {
		movingRight = true;
		ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update () {
		currentState.Update();
	}

}