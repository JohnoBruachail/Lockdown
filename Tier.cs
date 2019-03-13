using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingTier
{
	public int upgradeCost;
	public int currentHP;
	public int maxHP;
	public GameObject sprite;
}

public class Tier : MonoBehaviour {

	Player player;
	bool readyToUpgrade;
	public List<BuildingTier> tiers;
	private BuildingTier currentTier;

	public BuildingTier CurrentTier
    {
		get{
			return currentTier;
		}
		set{
			currentTier = value;
            int currentLevelIndex = tiers.IndexOf(currentTier);

            GameObject sprite = tiers[currentLevelIndex].sprite;
            for (int i = 0; i < tiers.Count; i++){
                if (sprite != null){
                    if (i == currentLevelIndex){
                        tiers[i].sprite.SetActive(true);
                    }
                    else{
                        tiers[i].sprite.SetActive(false);
                    }
            	}
			}
        }
	}

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("f") && readyToUpgrade == true){
			
			Debug.Log("cost: " + CurrentTier.upgradeCost);

			player.orbs -= CurrentTier.upgradeCost;
			upgrade();
		}
	}

	void OnEnable()
    {
        CurrentTier = tiers[0];
    }

    public BuildingTier getNextLevel()
    {
        int currentLevelIndex = tiers.IndexOf(currentTier);
        int maxLevelIndex = tiers.Count - 1;
        if (currentLevelIndex < maxLevelIndex)
        {
            return tiers[currentLevelIndex + 1];
        }
        else
        {
            return null;
        }
    }

	public void powerDown(){
		tiers[0].sprite.SetActive(true);
		int currentLevelIndex = tiers.IndexOf(currentTier);
		tiers[currentLevelIndex].sprite.SetActive(false);
	}

	public void powerUp(){
		tiers[0].sprite.SetActive(false);
		int currentLevelIndex = tiers.IndexOf(currentTier);
		tiers[currentLevelIndex].sprite.SetActive(true);
	}

    public void upgrade()
    {
        int currentLevelIndex = tiers.IndexOf(currentTier);
        if (currentLevelIndex < tiers.Count - 1)
        {
            CurrentTier = tiers[currentLevelIndex + 1];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){

			Debug.Log("player detected");
			gameObject.transform.Find("Upgrade").gameObject.SetActive(true);

			player = other.gameObject.GetComponent<Player>();

			if(player.orbs >= currentTier.upgradeCost && tiers.IndexOf(currentTier) != tiers.Count){
				Debug.Log("upgradeable");
				readyToUpgrade = true;
			}
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
			gameObject.transform.Find("Upgrade").gameObject.SetActive(false);
			readyToUpgrade = false;
        }
	}
}
