using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TowerTier
{
    public float fireRate;	
	public GameObject bullet;
}

public class Tower : Building, IInteractable
{
    public TowerTier[] towerTiers;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }



    public void Interact(){

        if(buildingTiers[currentTier].upgradeCost == 0){
            //flash the message "MAX LEVEL REACHED"
            interactionPrompt.SetActive(false);
            maxLevelAlert.SetActive(true);
            Invoke("CloseAlerts", 5.0f);

        }else if(buildingTiers[currentTier].upgradeCost > player.eCrystals){
            //flash the message "NOT ENOUGH RESOURCES"
            interactionPrompt.SetActive(false);
            notEnoughResourcesAlert.SetActive(true);
            Invoke("CloseAlerts", 5.0f);

        }else if(roomReference.isRoomPowered){
            //flash upgrade message
            player.eCrystals -= buildingTiers[currentTier].upgradeCost;
            buildingTiers[currentTier].buildingSprite.SetActive(false);
            currentTier++;
            buildingTiers[currentTier].buildingSprite.SetActive(true);  
            if(active == false){
                active = true;
            } 
        }else{
            //flash message that room is not powered
        }

    }

    public new void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){

            player = other.GetComponent<Player>();
            Debug.Log("Player collided with Tower");

			interactionPrompt.SetActive(true);
        }
    }


    public new void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){

            player = null;
			interactionPrompt.SetActive(false);
        }
    }
}
