using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recruit : MonoBehaviour {

	Player player;
	bool recruited;
	bool readyToRecruit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("f") && readyToRecruit == true && recruited != true){
			gameObject.transform.Find("Active").gameObject.SetActive(true);

			GameManager.instance.robots.Add(gameObject);

			recruited = true;
			player.orbs -= 1;
			gameObject.transform.Find("InActive").gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && recruited != true){

			Debug.Log("player detected");

			gameObject.transform.Find("Power Up").gameObject.SetActive(true);
			player = other.gameObject.GetComponent<Player>();

			if(player.orbs >= 1){
				Debug.Log("recruitable");
				readyToRecruit = true;
			}

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
			gameObject.transform.Find("Power Up").gameObject.SetActive(false);
			readyToRecruit = false;
        }
    }

	
}
