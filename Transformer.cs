using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : Building, IInteractable
{
    GameManager gameManager;
    public Player player;
    // Start is called before the first frame update

    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(){

        if(active){

            //flash the message "Already Powered"
            interactionPrompt.SetActive(false);

        }else if(gameManager.transformerUpgradeCost > player.eCrystals){
            //flash the message "NOT ENOUGH RESOURCES"
            interactionPrompt.SetActive(false);
            notEnoughResourcesAlert.SetActive(true);
            Invoke("CloseAlerts", 5.0f);
            
        }else if(!active && roomReference.parent.isRoomPowered && currentTier == 0){
            active = true;
            roomReference.isRoomPowered = true;//Changes the state of the var isRoomPowered in room reference to true.

            player.eCrystals -= gameManager.transformerUpgradeCost;

            gameManager.PowerUpRoom(roomReference);//update room information with gamemanager for spawning enemys

            //JOHN YOU HAVE TO UPGRADE THE REFERENCE OF THIS ROOM TO POWERED THEN ADD ITS CHILDREN TO THE SPAWN ENEMYS LIST.
            //SHIT, THIS CAN ONLY BE SET TO TRUE IF THE ROOMS PARENT HAS THEIR POWER ENABLEDED, PASS THIS DOWN TO THE TRANSFORMER AS WELL?

            buildingTiers[currentTier].buildingSprite.SetActive(false);
            currentTier++;
            buildingTiers[currentTier].buildingSprite.SetActive(true);   
        }else if(!active && !roomReference.parent.isRoomPowered && currentTier == 1){

            
        }
        else{
            //flash message that shows this transformer is disconnected from the network.
            //please activate the room
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
