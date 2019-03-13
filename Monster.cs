using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : NPC {

	BulletBehavior BB;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Bullet") {

			BB = other.gameObject.GetComponent<BulletBehavior>();

			AddjustCurrentHealth(-BB.damage);
			Destroy(other.gameObject);
    	}
    }
}