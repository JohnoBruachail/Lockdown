using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeLeaper : MonoBehaviour
{
    public EnemyLeaper parent;
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" || other.gameObject.name.Equals("BuildingTower")) {
			Debug.Log("Monster Alerted");
			parent.ChangeState(new AttackState());
    	}
    }

    void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player" || other.gameObject.name.Equals("BuildingTower")) {

      		parent.ChangeState(new IdleState());
    	}
    }
}
