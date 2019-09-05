using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tier
{
    public int upgradeCost;
	public int currentHP;
	public int maxHP;
	public GameObject buildingSprite;
}

public abstract class Building : MonoBehaviour
{
    public bool active;
    public RoomReference roomReference;
    public int currentTier = 0;
    public GameObject interactionPrompt;
    public GameObject maxLevelAlert;
    public GameObject notEnoughResourcesAlert;
    // Start is called before the first frame update
    public Tier[] buildingTiers;

    public void NewBuilding(ref RoomReference roomReference){
        this.roomReference = roomReference;
    }

    public void CloseAlerts(){
        interactionPrompt.SetActive(true);
        maxLevelAlert.SetActive(false);
        notEnoughResourcesAlert.SetActive(false);
    }

    void DamageBuilding(int damage){
        ChangeHP(-damage);
	}

    public void ChangeHP(int amount){
		buildingTiers[currentTier].currentHP += amount;

		//when a building reaches zero hp we want it to shutdown till it can be repaired.
		if(buildingTiers[currentTier].currentHP <= 0){
			active = false;
            roomReference.destroyedBuildings.Add(this);
		}
	}

    public void OnTriggerEnter2D(Collider2D other){
        
        if (other.gameObject.tag == "Bullet") {
			ChangeHP(-other.gameObject.GetComponent<BulletBehavior>().damage);
			Destroy(other.gameObject);
    	}
        
        if(other.gameObject.CompareTag("Enemy")){
			Debug.Log("Enemy tag triggered");
			DamageBuilding(other.GetComponent<Enemy>().collisionDamage);
		}

        if(other.gameObject.CompareTag("Player")){
			
            interactionPrompt.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){

			interactionPrompt.SetActive(false);
        }
    }

}
