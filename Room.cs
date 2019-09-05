using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    GameManager gameManager;
    public GameObject enemyContainer;
    public GameObject contentsContainer;//I need to split this into two seperate containers, defense containers and building containers
    public GameObject breach;
    private RoomContents roomContents;//A list of the contents of the room
    public GameObject u1Exit;
    public GameObject u2Exit;
    public GameObject r1Exit;
    public GameObject r2Exit;
    public GameObject d1Exit;
    public GameObject d2Exit;
    public GameObject l1Exit;
    public GameObject l2Exit;

    public SpawnPoint[] normalSpawnPoints;
    public SpawnPoint[] defenseSpawnPoints;
    public SpawnPoint[] breachSpawnPoints;

    [System.Serializable]
    public struct SpawnPoint{
        public bool filled;
        public int x;
        public int y;
    }

    private RoomReference roomReference;

    // Start is called before the first frame update
    void Awake(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        roomContents = contentsContainer.GetComponent<RoomContents>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewRoom(ref RoomReference roomReference){
        this.roomReference = roomReference;

        SetupExits();
        SetupObjects();
    }

    public void CreateBreach(int breachesAge){

        //select a random spawn point and create a breach at it.
        //the breach will then spawn enemys to head to and destroy the core
        int randomBreachSpawnPoint = Random.Range(0, breachSpawnPoints.Length);
        breach = Instantiate(roomContents.fixedSpawnTable[5], new Vector3(breachSpawnPoints[randomBreachSpawnPoint].x + contentsContainer.transform.position.x, breachSpawnPoints[randomBreachSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
        breach.GetComponent<Breach>().NewBreach(breachesAge);
    }

    public void SealBreach(){
        Destroy(breach);
    }

    void SetupExits(){
        if(roomReference.exits[0] == 1){
            u1Exit.SetActive(false);
        }else if(roomReference.exits[0] == 2){
            u2Exit.SetActive(false);
        }
        if(roomReference.exits[1] == 1){
            r1Exit.SetActive(false);
        }else if(roomReference.exits[1] == 2){
            r2Exit.SetActive(false);
        }
        if(roomReference.exits[2] == 1){
            d1Exit.SetActive(false);
        }else if(roomReference.exits[2] == 2){
            d2Exit.SetActive(false);
        }
        if(roomReference.exits[3] == 1){
            l1Exit.SetActive(false);
        }else if(roomReference.exits[3] == 2){
            l2Exit.SetActive(false);
        }
    }

    //Take the room type and build a random selection of objects in the room. The room must always have the room type object
    void SetupObjects(){

        int noOfAdditionalContainers = 0;
        int noOfAdditionalBuildings = 0;
        int noOfAdditionalDefenses = 0;
        int noOfEnemys = 0;

        int randomSpawnPoint            = Random.Range(0, normalSpawnPoints.Length);
        int randomDefensesSpawnPoint    = Random.Range(0, defenseSpawnPoints.Length);

        int initialRandomSpawnPoint;
        int randomObject;
        GameObject building;

        //First things first, place a transfomer in the room

        //select a random container from the list
        building = Instantiate(roomContents.fixedSpawnTable[0], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
        building.GetComponent<Building>().NewBuilding(ref roomReference);
        normalSpawnPoints[randomSpawnPoint].filled = true;

        randomSpawnPoint = Random.Range(0, normalSpawnPoints.Length);
        initialRandomSpawnPoint = randomSpawnPoint;

        //select a new random spawn point for the following room object.
        do{
            if(normalSpawnPoints[randomSpawnPoint].filled == true){
                randomSpawnPoint++;
            }else{
                break;
            }
            if(randomSpawnPoint >= normalSpawnPoints.Length){
                randomSpawnPoint = 0;
            }

        }while(randomSpawnPoint != initialRandomSpawnPoint);

        //depending on the room type we need to spawn the correct object somewhere in the room.
/* 
        Room types are as follows.
        0.  Empty       = nothing
        1.	Core        = nothing
        2.  Standard    = nothing
        3.	Defense     = turret
        4.	Mine        = mine
        5.	Reward      = loot container
        6.	Teleport    = teleport
        7.	Upgrade     = upgrade chest
        8.	Split       = 2 turrets
        9.	Mini Nest   = mini nest
        10. Main Nest   = main nest
*/
        //first we add the object the room is suppose to have based on its type
        switch(roomReference.type){
            case 0:
                break;
            case 1:
                break;
            case 2:
                noOfAdditionalContainers    = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalBuildings     = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalDefenses      = 0;
                noOfEnemys                  = Random.Range(1, 3);//between 1 and 2

                break;
            case 3:
                noOfAdditionalContainers    = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalBuildings     = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalDefenses      = Random.Range(1, 3);//between 1 and 2
                noOfEnemys                  = Random.Range(3, 5);//between 3 and 4

                //spawn a turret
                building = Instantiate(roomContents.fixedSpawnTable[3], new Vector3(defenseSpawnPoints[randomDefensesSpawnPoint].x + contentsContainer.transform.position.x, defenseSpawnPoints[randomDefensesSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                building.GetComponent<Building>().NewBuilding(ref roomReference);
                defenseSpawnPoints[randomDefensesSpawnPoint].filled = true;
               
                break;
            case 4:
                noOfAdditionalContainers    = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalBuildings     = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalDefenses      = Random.Range(0, 2);//between 0 and 1
                noOfEnemys                  = Random.Range(1, 3);//between 1 and 2

                building = Instantiate(roomContents.fixedSpawnTable[1], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                building.GetComponent<Building>().NewBuilding(ref roomReference);
                normalSpawnPoints[randomSpawnPoint].filled = true;

                break;
            case 5:
                noOfAdditionalContainers    = 0;
                noOfAdditionalBuildings     = 0;
                noOfAdditionalDefenses      = 0;
                noOfEnemys                  = 0;

                building = Instantiate(roomContents.fixedSpawnTable[4], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                normalSpawnPoints[randomSpawnPoint].filled = true;

                break;
            case 6:
                noOfAdditionalContainers    = 0;
                noOfAdditionalBuildings     = 0;
                noOfAdditionalDefenses      = 0;
                noOfEnemys                  = 0;

                building = Instantiate(roomContents.fixedSpawnTable[2], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                building.GetComponent<Building>().NewBuilding(ref roomReference);
                normalSpawnPoints[randomSpawnPoint].filled = true;
                break;
            case 7:
                noOfAdditionalContainers    = 0;
                noOfAdditionalBuildings     = 0;
                noOfAdditionalDefenses      = 0;
                noOfEnemys                  = 0;

                //I CURRENTLY DONT HAVE AN UPGRADE CHEST, SO HAVE ADDED A NORMAL CHEST HERE INSTEAD.

                building = Instantiate(roomContents.fixedSpawnTable[4], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                normalSpawnPoints[randomSpawnPoint].filled = true;
                break;
            case 8:
                noOfAdditionalContainers    = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalBuildings     = Random.Range(0, 2);//between 0 and 1
                noOfAdditionalDefenses      = Random.Range(2, 4);//between 2 and 3
                noOfEnemys                  = Random.Range(3, 5);//between 3 and 4
                
                //spawn a turret
                building = Instantiate(roomContents.fixedSpawnTable[3], new Vector3(defenseSpawnPoints[randomDefensesSpawnPoint].x + contentsContainer.transform.position.x, defenseSpawnPoints[randomDefensesSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
                building.GetComponent<Building>().NewBuilding(ref roomReference);
                defenseSpawnPoints[randomDefensesSpawnPoint].filled = true;

                break;
            case 9:
                break;
        }

        //Add the additional containers to the room
        for(int x = 0; x < noOfAdditionalContainers; x++){
            randomSpawnPoint = Random.Range(0, normalSpawnPoints.Length);
            initialRandomSpawnPoint = randomSpawnPoint;

            do{
                if(normalSpawnPoints[randomSpawnPoint].filled == true){
                    randomSpawnPoint++;
                }else{
                    break;
                }
                if(randomSpawnPoint >= normalSpawnPoints.Length){
                    randomSpawnPoint = 0;
                }

            }while(randomSpawnPoint != initialRandomSpawnPoint);

            //select a random container from the list
            randomObject = Random.Range(0, roomContents.randomContainersSpawnTable.Length);
            building = Instantiate(roomContents.randomContainersSpawnTable[randomObject], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);

            normalSpawnPoints[randomSpawnPoint].filled = true;
        }



        //Add the additional buildings to the room
        for(int x = 0; x < noOfAdditionalBuildings; x++){
            randomSpawnPoint = Random.Range(0, normalSpawnPoints.Length);
            initialRandomSpawnPoint = randomSpawnPoint;

            do{
                if(normalSpawnPoints[randomSpawnPoint].filled == true){
                    randomSpawnPoint++;
                }else{
                    break;
                }
                if(randomSpawnPoint >= normalSpawnPoints.Length){
                    randomSpawnPoint = 0;
                }

            }while(randomSpawnPoint != initialRandomSpawnPoint);

            //select a random buildings from the list
            randomObject = Random.Range(0, roomContents.randomBuildingSpawnTable.Length);
            building = Instantiate(roomContents.randomBuildingSpawnTable[randomObject], new Vector3(normalSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
            building.GetComponent<Building>().NewBuilding(ref roomReference);

            normalSpawnPoints[randomSpawnPoint].filled = true;
        }



        //Add the additional defenses to the room
        for(int x = 0; x < noOfAdditionalDefenses; x++){
            randomSpawnPoint = Random.Range(0, defenseSpawnPoints.Length);
            initialRandomSpawnPoint = randomSpawnPoint;

            do{
                if(defenseSpawnPoints[randomSpawnPoint].filled == true){
                    randomSpawnPoint++;
                }else{
                    break;
                }
                if(randomSpawnPoint >= defenseSpawnPoints.Length){
                    randomSpawnPoint = 0;
                }

            }while(randomSpawnPoint != initialRandomSpawnPoint);
             
            randomObject = Random.Range(0, roomContents.randomDefensesSpawnTable.Length);
            building = Instantiate(roomContents.randomDefensesSpawnTable[randomObject], new Vector3(defenseSpawnPoints[randomSpawnPoint].x + contentsContainer.transform.position.x, defenseSpawnPoints[randomSpawnPoint].y + contentsContainer.transform.position.y, 0), Quaternion.identity, contentsContainer.transform);
            building.GetComponent<Building>().NewBuilding(ref roomReference);

            defenseSpawnPoints[randomSpawnPoint].filled = true;
        }




        //spawn enemys at random spawn points
        for(int x = 0; x < noOfEnemys; x++){
            randomSpawnPoint    = Random.Range(0, normalSpawnPoints.Length);
            randomObject        = Random.Range(0, roomContents.randomEnemySpawnTable.Length);
            Instantiate(roomContents.randomEnemySpawnTable[randomObject], new Vector3(normalSpawnPoints[randomSpawnPoint].x + enemyContainer.transform.position.x, normalSpawnPoints[randomSpawnPoint].y + enemyContainer.transform.position.y, 0), Quaternion.identity, enemyContainer.transform);
        }
        enemyContainer.SetActive(false);//Experementing with turning off the containers full of enemys in all rooms

    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){

            enemyContainer.SetActive(true);
        }
    }


    public void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){

            enemyContainer.SetActive(false);
        }
    }
}
