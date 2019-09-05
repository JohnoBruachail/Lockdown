using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TeleporterTier
{
    public int teleportCost;
}

public class Teleporter : Building, IInteractable
{

    public TeleporterTier[] tpTiers;

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
            if(tpTiers[currentTier].teleportCost > player.eCrystals){
                //flash the message "NOT ENOUGH RESOURCES"
                interactionPrompt.SetActive(false);
                notEnoughResourcesAlert.SetActive(true);
                Invoke("CloseAlerts", 5.0f);
            }else if(active){
                //activate the teleport back to base
                interactionPrompt.SetActive(false);
                player.eCrystals -= tpTiers[currentTier].teleportCost;
                Invoke("Teleport", 2.0f);
            }else{
                //flash message building is inactive
            }
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
        }else{
            //flash message that room is not powered
        }

    }

    public void Teleport(){
        player.gameObject.transform.position = new Vector3(50,50,50);
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
