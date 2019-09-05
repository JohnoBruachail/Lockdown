using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	public int currentHP;
	public int maxHP;
	public int eCrystals;
	public int eCrystalsCapacity;
	public bool alive = true;

	public void ChangeHP(int amount){
		currentHP += amount;

		//set its state to dead, animate its death animation and call for it to be destroyed in 5 seconds.
		if(currentHP <= 0){
			alive = false;
			//in each character we can change the animation if alive = false and remove input/actions in update
			Invoke("Die", 5);
		}
	}

	private void Die(){
        Destroy(gameObject);
    }
}
