using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerTier
{
	public int upgradeCost;
	public int currentHP;
	public int maxHP;
	public GameObject sprite;
	public GameObject bullet;
	public float fireRate;	
}

public class Tower : MonoBehaviour {

	public List<TowerTier> tiers;
	private TowerTier currentTier;

	public TowerTier CurrentTier
    {
		get{
			return currentTier;
		}
		set{
			currentTier = value;
            int currentLevelIndex = tiers.IndexOf(currentTier);

            GameObject levelVisualization = tiers[currentLevelIndex].sprite;
            for (int i = 0; i < tiers.Count; i++)
            {
                if (levelVisualization != null)
                {
                    if (i == currentLevelIndex)
                    {
                        tiers[i].sprite.SetActive(true);
                    }
                    else
                    {
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
		
	}

	void OnEnable()
    {
        CurrentTier = tiers[0];
    }

    public TowerTier getNextLevel()
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

    public void increaseLevel()
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

			gameObject.transform.Find("Upgrade").gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){

			gameObject.transform.Find("Upgrade").gameObject.SetActive(false);
        }
    }

	
}
