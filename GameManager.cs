using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance {
        get {
                return instance;
            }
    }

    private void Awake(){
        if (instance != null && instance != this){
            
            Destroy(this.gameObject);
            
        }
        else {
            instance = this;
        }
    }

    public int transformerUpgradeCost = 1;
    public int transformerUpgradeTally = 0;
    public List<Breach> breaches = new List<Breach>();  // a record of the breaches on the map.
    int breachesAge = 0;                                //To keep track of the number of cycles that have occured
    public List<Room> breachRooms;
    public List<RoomReference> waveSpawnRooms = new List<RoomReference>();


    //I want to keep track of what rooms are breach rooms, and when a breach starts trigger all breach rooms




    // ON START SET THE TRIGGER RATE FOR WAVE CYCLES AND 
    void Start(){
        InvokeRepeating("TriggerBreach", 5.0f, 5.0f);
        Debug.Log("TriggerBreach invoked");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBuilding(){

    }

    //Removes a powered up room from the wave spawn list and adds its children to the list in its place.
    public void PowerUpRoom(RoomReference roomRef){

        if(transformerUpgradeCost == transformerUpgradeTally){
            transformerUpgradeTally = 0;
            transformerUpgradeCost++;
        }else{
            transformerUpgradeTally++;
        }

        waveSpawnRooms.Remove(roomRef);

        foreach(RoomReference child in roomRef.children){
            waveSpawnRooms.Add(child);
        }
    }

    void TriggerBreach(){

        foreach(Room room in breachRooms){

            room.CreateBreach(breachesAge);
            breachesAge++;
        }


    }

    public void AddBreach(Breach breach){
        breaches.Add(breach);
    }
}
