using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerCoreTier
{
	public int currentHP;
	public int maxHP;
	public int upgradeCost;
    public List<GameObject> prefabs;
}

public class PowerCore : MonoBehaviour {

    public GameObject active;
    public GameObject inActive;
	Player player;
	bool readyToUpgrade;
	public List<PowerCoreTier> tiers;
	private PowerCoreTier currentTier;

	public PowerCoreTier CurrentTier
    {
		get{
			return currentTier;
		}
		set{
			currentTier = value;
            int currentLevelIndex = tiers.IndexOf(currentTier);

            GameObject sprite = tiers[currentLevelIndex].prefabs[0];

            for (int i = 0; i < tiers.Count; i++){

                for(int j = 0; j <= tiers[i].prefabs.Count; j++){
                    if (sprite != null){
                        if (i <= currentLevelIndex){
                            tiers[i].prefabs[j].SetActive(true);
                        }
                        else{
                            tiers[i].prefabs[j].SetActive(false);
                        }
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

    public PowerCoreTier getNextLevel()
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
