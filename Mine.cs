using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

	Player player;
	bool readyToMine;
	public int miningCost;
	public int payout;
	public Transform EnergyOrb;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("f") && readyToMine == true){
			
			player.orbs -= miningCost;
			//add the object to a worker list for robots.
			for(int x=0; x < payout; x++){
				Instantiate(EnergyOrb, this.gameObject.transform.position, Quaternion.identity);
			}
			GameObject.Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){

			gameObject.transform.Find("Mine").gameObject.SetActive(true);

			player = other.gameObject.GetComponent<Player>();

			if(player.orbs >= miningCost){
				Debug.Log("minable");
				readyToMine = true;
			}
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
			gameObject.transform.Find("Mine").gameObject.SetActive(false);
			readyToMine = false;
        }
	}
}
