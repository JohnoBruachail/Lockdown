using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeZapper : MonoBehaviour
{
    public EnemyZapper parent;
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" || other.gameObject.name.Equals("BuildingTower")) {
			Debug.Log("Monster Alerted");
			parent.ChangeState(new RangedAttackState());
    	}
    }

    void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player" || other.gameObject.name.Equals("BuildingTower")) {

      		parent.ChangeState(new IdleState());
    	}
    }
}
