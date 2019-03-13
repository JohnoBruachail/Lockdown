using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	float breachTriggerTime = 10f;
	float breachEndTime = 90f;
	float time;

	public List<GameObject> robots;
	public List<GameObject> monsters;
	public List<GameObject> breaches;

	public List<GameObject> workerJobs;

	void Awake(){

		if (instance == null){
			
			//if not, set instance to this
			instance = this;
		}
		else if (instance != this){

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);   
		}

		DontDestroyOnLoad(gameObject);

		robots = new List<GameObject>();
		monsters = new List<GameObject>();
		breaches = new List<GameObject>();
	}

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

		updateTime();
		
	}

	public void addRobotToList()
	{
		
	}

	public void addBuildToList()
	{

	}

	public void addBreachToList(GameObject inputBreach)
	{
		breaches.Add(inputBreach);
	}

	void updateTime(){
		time += Time.deltaTime;
		if(time > breachTriggerTime){

			Debug.Log("Breach Time Boys");

			foreach(GameObject breach in breaches){
				breach.GetComponent<Breach>().Trigger();
			}

			time = 0;

		}
	}
}
