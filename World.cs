using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Rooms contains a prefab and prefab exit data.
[System.Serializable]
public class Room
{
    public GameObject prefab;
	public int width;
	public int height;
    public Exit upExit;
    public Exit rightExit;
    public Exit downExit;
    public Exit leftExit;
}
[System.Serializable]
public class Rooms
{
    public Room[] room;
}
[System.Serializable]
public class Exit
{
	public int x;
	public int y;
}

public class World : MonoBehaviour {

    public static World instance = null;

    private int type;

    private Vector3 backupLocation;
    private int backupType;
    private int backupRoomNumber;
    private int backupEntrance;

    private int tempX;
    private int tempY;
    private int tempWidth;
    private int tempHeight;
    private int newRoomType;
    private int thisRoomExit;
    private int newRoomEntrance;
    private int roomCount;
    private int attempts = 20;
    private int leftBranchLength;
    private int rightBranchLength;
    private int smallBranchLength;
    private int branchLength;
    private int startingPoint;
    private int nextBranch;
    private int randomNumber;
    private bool buildingABranch;

    private int nextRoomX, nextRoomY;

    public int worldWidth;
    public int worldHeight;
    public int[,] worldBlueprint;       //The world blueprint is used to store room locations to stop unwanted intersections.
    int[][] leftBranchLayout;
    int[][] rightBranchLayout;

    //public int numberOfRoomsPerBranch = 10;

    Transform worldContainer;           //A container for the entire world
    Vector3 newLocation;

    private GameObject toInstantiate;
    public GameObject debugBreach;
    public Rooms[] theRooms;
    public Stack<newRoom> toBuild = new Stack<newRoom>();
    public newRoom tempRoom = new newRoom();

    public struct newRoom{
        public Vector3 location;
        public int roomType;
        public int roomNumber;
        public int entrance;
		
		public newRoom(Vector3 inputLocation, int inputType, int inputRoomNumber, int inputEntrance){
			location    = inputLocation;
            roomType    = inputType;
			roomNumber  = inputRoomNumber;
            entrance    = inputEntrance;
		}
    }



    void Awake(){

		if (instance == null){
			
			//if not, set instance to this
			instance = this;
		}
		else if (instance != this){

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a world.
            Destroy(gameObject);   
		}
	}

	public void Start (){

        Debug.Log("Generate World");

        leftBranchLayout    = SetupBranch();
        rightBranchLayout   = SetupBranch();
        
        DebugBranches();

        SetupWorldBlueprint();
        SetupMainRoom();

        

        DebugBreaches();
	}


    public void update(){

        if(Input.GetKeyDown(KeyCode.Space)){

            for(int x = 0; x < leftBranchLayout.Length; x++){
                for (int y = 0; y < leftBranchLayout[x].Length; y++){

                    tempRoom = toBuild.Pop();
                    addRoom(x, y, tempRoom.location, tempRoom.roomNumber, tempRoom.entrance);
                }
            }
        }
    }


    int[][] SetupBranch(){
/* 
        Generate an array of arrays that dictates what room type is generated. types are as follows.

        0.	Base
        1.  Empty
        2.	Defense
        3.	Mine
        4.	Reward
        5.	Teleport
        6.	Upgrade
        7.	Branch
        8.  Branch
        9.	Hive
        10. Tunnel
*/

        branchLength            = UnityEngine.Random.Range(8, 12);
        nextBranch              = UnityEngine.Random.Range(3, 4);
        int [][] branchLayout   = new int [branchLength][];
        
        for(int x = 0; x < branchLength; x++){

            //create a branch room
            if (x == nextBranch){
 
                smallBranchLength = UnityEngine.Random.Range(4, 6);
                branchLayout[x] = new int[smallBranchLength];

                //build the off branch to the main branch
                for(int y = 0; y < smallBranchLength; y++){

                    if(y == 0){
                        branchLayout[x][y] = 7;
                    }
                    else if(y == (smallBranchLength - 1)){
                        branchLayout[x][y] = UnityEngine.Random.Range(4, 6);
                    }
                    else
                    {
                        branchLayout[x][y] = UnityEngine.Random.Range(1, 3);
                    }
                }
                nextBranch = nextBranch + UnityEngine.Random.Range(3, 4);
            }
            else{
                branchLayout[x] = new int[1];
                branchLayout[x][0] = UnityEngine.Random.Range(1, 3);
            }
        }
        return branchLayout;
    }



    void SetupWorldBlueprint(){
        //Debug.Log("Setup Blueprint");
        worldBlueprint = new int[worldWidth, worldHeight];
    }

    void SetupMainRoom(){
        tempWidth = worldWidth / 2;
        tempHeight = worldHeight / 2; 

        //Debug.Log("HELLO" + tempWidth + "HELLO" + tempHeight + "HELLO" + tempWidth);

        tempWidth = tempWidth - (theRooms[0].room[0].width / 2);
        tempHeight = tempHeight - (theRooms[0].room[0].height / 2);

        Vector3 location = new Vector3(tempWidth, tempHeight, 0f);

        //Debug.Log("Spawning main room at " + location);
        
        GameObject instance = (GameObject) Instantiate(theRooms[0].room[0].prefab);
        instance.transform.position = location;
        instance.transform.SetParent (worldContainer);
        updateBlueprint(location, theRooms[0].room[0].width, theRooms[0].room[0].height);

        //Debug.Log("MAIN EXIT LEFT EXIT FORMULA");
        //Debug.Log("FORMULA: X Location: " + location.x + " - " + rooms[newRoomNumber].width + " = " + (location.x - rooms[newRoomNumber].width));
        //Debug.Log("FORMULA: Y Location: " + location.y + " + " + mainRoom.leftExit.y + " - " + rooms[newRoomNumber].rightExit.y + " = " + ((location.y + mainRoom.leftExit.y) - rooms[newRoomNumber].rightExit.y));
        
        startingPoint = UnityEngine.Random.Range(0, theRooms[leftBranchLayout[0][0]].room.Length);
        randomNumber = startingPoint;
        while(theRooms[leftBranchLayout[0][0]].room[randomNumber].rightExit.x == 0){
            randomNumber++;

            if (randomNumber >= theRooms[leftBranchLayout[0][0]].room.Length){
                randomNumber = 0;
            }
            if(randomNumber == startingPoint){
                Debug.Log("No sutable rooms were found");
                break;
            }
        }
        
        newLocation = new Vector3((location.x - theRooms[leftBranchLayout[0][0]].room[randomNumber].width), ((location.y + theRooms[0].room[0].leftExit.y) - theRooms[leftBranchLayout[0][0]].room[randomNumber].rightExit.y), 0f);             

        //create new room based on main rooms left exit
        if(checkBlueprint(newLocation, theRooms[leftBranchLayout[0][0]].room[randomNumber].width - 1, theRooms[leftBranchLayout[0][0]].room[randomNumber].height) == true){
            
            Debug.Log("Adding room type: " + leftBranchLayout[0][0] + " to build list");
            newRoom newRoom = new newRoom();
            newRoom.location = newLocation;
            newRoom.roomType = 1;
            newRoom.roomNumber = randomNumber;
            newRoom.entrance = 1;

            toBuild.Push(newRoom);
        }
/* 
        //create new room based on main rooms right exit
        rngRoom = UnityEngine.Random.Range(0, rooms.Count);

        //Debug.Log("MAIN EXIT RIGHT EXIT FORMULA");
        //Debug.Log("FORMULA: X Location: " + location.x + " + " + mainRoom.width + " + 1 " + " = " + (location.x + (mainRoom.width + 1)));
        //Debug.Log("FORMULA: Y Location: " + location.y + " + " + mainRoom.rightExit.y + " - " + rooms[newRoomNumber].leftExit.y + " = " + ((location.y + mainRoom.rightExit.y) - rooms[newRoomNumber].leftExit.y));
        newLocation = new Vector3((location.x + mainRoom.width + 1), ((location.y + mainRoom.rightExit.y) - rooms[rngRoom].leftExit.y), 0f);

        //check for collisions with existing rooms
        if(checkBlueprint(newLocation, rooms[rngRoom].width, rooms[rngRoom].height) == true){
            //Debug.Log("Sending room blueprint right of main at: " + newLocation);
            newRoom newRoom = new newRoom();
            newRoom.location = newLocation;
            newRoom.roomNumber = rngRoom;
            newRoom.entrance = 1;

            toBuild.Push(newRoom);
        }
*/
    }

    //Char priorRoomExitDirection, Exit priorRoomExitLocation
    void addRoom(int x, int y, Vector3 location, int roomNumber, int entrance){

        //reset
        newRoomEntrance = 10;
/*        
        Stages of this method
        1.  room is placed
        2.  select random exit (Ignore 0,0 exits)
        3.  a new room is selected at random
        4.  its spawn location relative to the exit is calculated
        5.  the area the new room takes up is overlayed on the blueprint
        6.  if the space is not empty return to 2

        Builds room based on type
        0.  Base        Main base, to be ignored.
        1.  Empty       Found in the middle of branches.
        2.	Defense     Found in the middle of branches.
        3.	Mine        Found in the middle of branches.
        4.	Reward      Found at the end of branches.
        5.	Teleport    Found at the end of branches.
        6.	Upgrade     Found at the end of branches.
        7.	Branch      Create branches.
        8.  Orthogonal  Found just after a branch.
        9.	Hive        Found at the end of the main left and right branch.
        10. Tunnel      Used if a room cannot be placed.
*/
        type = leftBranchLayout[x][y];
        Debug.Log("SPAWNING A NEW ROOM OF TYPE: " + type + " AT: " + location + " WITH X:" + x + " Y: " + y + " AND ROOMNUMBER: " + roomNumber);
        //Debug.Log("Spawning a new room at " + location);
        GameObject instance = (GameObject) Instantiate(theRooms[leftBranchLayout[x][y]].room[roomNumber].prefab);
        instance.transform.position = location;
        instance.transform.SetParent (worldContainer);
        updateBlueprint(location, theRooms[leftBranchLayout[x][y]].room[roomNumber].width, theRooms[leftBranchLayout[x][y]].room[roomNumber].height);

        //Room is placed, now select an exit and attempt to build the next room in the branch.

        //If the room is an empty, defense or mine type do the following.
        if(type == 1 || type == 2 || type == 3 || type == 8){
            Debug.Log("Building based on room type: " + type);
        
            //The next room type will be
            if(buildingABranch == true){
                newRoomType = leftBranchLayout[x][y+1];
            }
            else{
                newRoomType = leftBranchLayout[x+1][0];
            }
            
            Debug.Log("The new rooms type: " + newRoomType);

            int attempts = 0;
            newRoomEntrance = calculateNewRoomsEntrance(type, roomNumber, entrance);

            Debug.Log("New rooms entrance is: " + newRoomEntrance);
            do{
                //Select the exit to the room that is not the entrance
                if(selectNewRoom(location, type, roomNumber, newRoomType, newRoomEntrance) == true){
                    
                    Debug.Log("Adding room to build list");
                    newRoom newRoom = new newRoom();
                    newRoom.location = newLocation;
                    newRoom.roomType = newRoomType;
                    newRoom.roomNumber = randomNumber;
                    newRoom.entrance = newRoomEntrance;

                    toBuild.Push(newRoom);
                    break;
                }
                else{
                    addAuxiliaryRoom(location, type, roomNumber, 10, newRoomEntrance);
                    //addEmergencyRoom needs to update location, roomNumber and newRoomEntrance

                }

                Debug.LogError("Failed to select a room that fit into the location, Exiting");
                break;
                attempts++;
                
            }while(true);

        }
        //If the room is a reward, teleport or upgrade type do the following.
        else if(type == 4 || type == 5 || type == 6){
            Debug.Log("Building based on room type: " + type);
            buildingABranch = false;
            //Do nothing
        }
        //If the room is a branching room type do the following.
        else if(type == 7){
            Debug.Log("Building based on room type: " + type);
            buildingABranch = true;
            int exit = 10;
            exit = SelectExit(type, roomNumber, entrance);

            backupLocation = location;
            backupType = type;
            backupRoomNumber = roomNumber;
            backupEntrance = entrance;

            //test if the exit is north or south and build the appropreate room.
            if(exit == 0 || exit == 2){
                while(addAuxiliaryRoom(location, type, roomNumber, 8, newRoomEntrance) == false){
                    addAuxiliaryRoom(location, type, roomNumber, 10, newRoomEntrance);
                    newRoomEntrance = calculateNewRoomsEntrance(type, roomNumber, entrance);
                }
                tempRoom = toBuild.Pop();
                type = tempRoom.roomType;
                location = tempRoom.location;
                roomNumber = tempRoom.roomNumber;
                entrance = tempRoom.entrance;
                Debug.Log("RESUMING NORMAL ROOM LAYOUT STARTING AT LOCATION: " + location + " ROOM TYPE:" + type + " ROOM NUMBER: " + roomNumber);
            }
            newRoomEntrance = calculateNewRoomsEntrance(type, roomNumber, entrance);
            newRoomType = leftBranchLayout[x+1][0];

            int attempts = 0;

            Debug.Log("New rooms entrance is: " + newRoomEntrance + " and the room type is: " + newRoomType);

            //if the entrance is 0 or 2 go with an orthagonal exit
            //if the entrace is 1 or 4 go with an empty room entrance.

            do{
                //Select the exit to the room that is not the entrance
                if(selectNewRoom(location, type, roomNumber, newRoomType, newRoomEntrance) == true){
                    
                    Debug.Log("Adding room to build list");
                    newRoom newRoom = new newRoom();
                    newRoom.location = newLocation;
                    newRoom.roomType = newRoomType;
                    newRoom.roomNumber = randomNumber;
                    newRoom.entrance = newRoomEntrance;

                    toBuild.Push(newRoom);
                    break;
                }
                else{
                    addAuxiliaryRoom(location, type, roomNumber, 10, newRoomEntrance);
                    //addEmergencyRoom needs to update location, roomNumber and newRoomEntrance

                }

                Debug.LogError("Failed to select a room that fit into the location, Exiting");
                break;
                attempts++;
                
            }while(true);



            Debug.Log("MOVING ON TO SECOND ROOM");
            //type = backupType;
            //select exit for the second room
            int exitTwo = SelectExit(backupType, backupRoomNumber, backupEntrance, exit);
            //based on direction select room type
            Debug.Log("second exit is: " + exitTwo);

            //test if the exit is north or south and build the appropreate room.
            if(exitTwo == 0 || exitTwo == 2){
                Debug.Log("Exit is a 0 or 2");
                while(addAuxiliaryRoom(backupLocation, backupType, backupRoomNumber, 8, newRoomEntrance) == false){
                    addAuxiliaryRoom(backupLocation, backupType, backupRoomNumber, 10, newRoomEntrance);
                    newRoomEntrance = calculateNewRoomsEntrance(exitTwo);
                }
                tempRoom = toBuild.Pop();
                type = tempRoom.roomType;
                location = tempRoom.location;
                roomNumber = tempRoom.roomNumber;
                entrance = tempRoom.entrance;
                Debug.Log("RESUMING NORMAL ROOM LAYOUT, NEXT ROOM WILL BE AT LOCATION: " + location + "TYPE:" + newRoomType + " room number: " + roomNumber);
            }
            newRoomEntrance = calculateNewRoomsEntrance(exitTwo);
            newRoomType = leftBranchLayout[x][y+1];
            Debug.Log("New room TYPE is: " + newRoomType);


            attempts = 0;

            //Select the exit to the room that is not the entrance
            Debug.Log("New rooms entrance is: " + newRoomEntrance);
            do{
                //Select the exit to the room that is not the entrance
                if(selectNewRoom(backupLocation, backupType, backupRoomNumber, newRoomType, newRoomEntrance) == true){
                    
                    Debug.Log("Adding room to build list");
                    newRoom newRoom = new newRoom();
                    newRoom.location = newLocation;
                    newRoom.roomType = newRoomType;
                    newRoom.roomNumber = randomNumber;
                    newRoom.entrance = newRoomEntrance;

                    toBuild.Push(newRoom);
                    break;
                }
                else{
                    addAuxiliaryRoom(backupLocation, backupType, backupRoomNumber, 10, newRoomEntrance);
                    //addEmergencyRoom needs to update location, roomNumber and newRoomEntrance

                }

                Debug.LogError("Failed to select a room that fit into the location, Exiting");
                break;
                attempts++;
                
            }while(true);
            
        }
        //if the room is a hive room type the branch is complete.
        else if(type == 9){
            Debug.Log("Building based on room type: " + type);
        
        }
    }



    bool addAuxiliaryRoom(Vector3 location, int type, int roomNumber, int newRoomType, int entrance){
        Debug.LogWarning("Auxiliary room will be placed");
        do{
            //Select the exit to the room that is not the entrance
            if(selectNewRoom(location, type, roomNumber, newRoomType, entrance) == true){
                
                Debug.Log("BUILDING AUXILIARY ROOM: " + newRoomType + " NUMBER: " + randomNumber + " AT: " + newLocation);

                GameObject instance = (GameObject) Instantiate(theRooms[newRoomType].room[randomNumber].prefab);
                instance.transform.position = newLocation;
                instance.transform.SetParent (worldContainer);
                updateBlueprint(newLocation, theRooms[newRoomType].room[randomNumber].width, theRooms[newRoomType].room[randomNumber].height);

                //I need to update the roomtype, number for the next cycle.
                newRoom newRoom = new newRoom();
                newRoom.location = newLocation;
                newRoom.roomType = newRoomType;
                newRoom.roomNumber = randomNumber;
                newRoom.entrance = entrance;

                toBuild.Push(newRoom);
                return true;
            }
            else{
                Debug.LogError("Plan B has failed");
                return false;
            }
  
        }while(true);
    }



    void updateBlueprint(Vector3 location, int width, int height){
        //Debug.Log("Update Blueprint");
        tempX = (int)location.x + width - 1;
        tempY = (int)location.y + height - 1;

        //Debug.Log("Start location is: " + location);
        //Debug.Log("End location is: (" + tempX + ".0, " + tempY + ".0, 0.0)");

        for(int x = (int)location.x; x <= tempX; x++){
                //Debug.Log("The X is " + x);
            for(int y = (int)location.y; y <= tempY; y++){
                //Debug.Log("The Y is " + y);
                worldBlueprint[x,y] = 1;
            }
        }
    }  



    //check the blueprint to ensure no room is in the way of the new room
    bool checkBlueprint(Vector3 location, int width, int height){
        //Debug.Log("Checking Blueprint");
        tempX = (int)location.x + width - 1;
        tempY = (int)location.y + height - 1;

        //Debug.Log("Start location is: " + location);
        //Debug.Log("End location is: (" + tempX + ".0, " + tempY + ".0, 0.0)");

        for(int x = (int)location.x; x <= tempX; x++){
            for(int y = (int)location.y; y <= tempY; y++){
                if(worldBlueprint[x,y] == 1){

                    Debug.LogWarning("The block at " + x + " by " + y + " was filled in already");
                    return false;

                }
            }
        }
        return true;
    }



    int SelectExit(int type, int roomNumber, int entrance){

        if(theRooms[type].room[roomNumber].upExit.x != 0 && entrance != 0){
            newRoomEntrance = 2;
            return 0;
        }
        else if(theRooms[type].room[roomNumber].rightExit.x != 0 && entrance != 1){
            newRoomEntrance = 3;
            return 1;
        }
        else if(theRooms[type].room[roomNumber].downExit.x != 0 && entrance != 2){
            newRoomEntrance = 0;
            return 2;
        }
        else if(theRooms[type].room[roomNumber].leftExit.x != 0 && entrance != 3){
            newRoomEntrance = 1;
            return 3;
        }
        else{
            Debug.LogError("A new rooms exit failed to calculate, Type: " + type + " roomNumber: " + roomNumber + " entrance: " + entrance);
            return 10;
        }
    }


    int SelectExit(int type, int roomNumber, int entrance, int otherExit){

        if(theRooms[type].room[roomNumber].upExit.x != 0 && entrance != 0 && otherExit != 0){
            return 0;
        }
        else if(theRooms[type].room[roomNumber].rightExit.x != 0 && entrance != 1 && otherExit != 1){
            return 1;
        }
        else if(theRooms[type].room[roomNumber].downExit.x != 0 && entrance != 2 && otherExit != 2){
            return 2;
        }
        else if(theRooms[type].room[roomNumber].leftExit.x != 0 && entrance != 3 && otherExit != 3){
            return 3;
        }
        else{
            Debug.LogError("A new rooms exit failed to calculate, Type: " + type + " roomNumber: " + roomNumber + " entrance: " + entrance);
            return 10;
        }
    }


    int calculateNewRoomsEntrance(int type, int roomNumber, int entrance){
        Debug.Log("Calculating new room entrance type: " + type + " roomNumber: " + roomNumber + " and entrance: " + entrance);
        //find the current rooms exit other then the entrance.
        if(theRooms[type].room[roomNumber].upExit.x != 0 && entrance != 0){
            return 2;
        }
        else if(theRooms[type].room[roomNumber].rightExit.x != 0 && entrance != 1){
            return 3;
        }
        else if(theRooms[type].room[roomNumber].downExit.x != 0 && entrance != 2){
            return 0;
        }
        else if(theRooms[type].room[roomNumber].leftExit.x != 0 && entrance != 3){
            return 1;
        }
        else{
            Debug.LogError("A new rooms exit failed to calculate, Type: " + type + " roomNumber: " + roomNumber + " entrance: " + entrance);
            return 10;
        }


    }
    int calculateNewRoomsEntrance(int otherExit){

        //find the current rooms exit other then the entrance.
        if(otherExit == 0){
            return 2;
        }
        else if(otherExit == 1){
            return 3;
        }
        else if(otherExit == 2){
            return 0;
        }
        else if(otherExit == 3){
            return 1;
        }
        else{
            Debug.LogError("A new rooms second exit failed to calculate");
            return 10;
        }
    }



    bool selectNewRoom(Vector3 location, int type, int roomNumber, int newRoomType, int newRoomEntrance){

        //select a room that has the correct entrance
        startingPoint = UnityEngine.Random.Range(0, theRooms[newRoomType].room.Length);
        randomNumber = startingPoint;

        do{
            Debug.Log("Testing new room number: " + randomNumber);
            if(newRoomEntrance == 0 && theRooms[newRoomType].room[randomNumber].upExit.x != 0){

                newLocation = calculateLocation(location, type, roomNumber, newRoomType, randomNumber, newRoomEntrance);
                if(checkBlueprint(newLocation, theRooms[newRoomType].room[randomNumber].width, theRooms[newRoomType].room[randomNumber].height) == true){
                    return true;
                }
            }
            else if(newRoomEntrance == 1 && theRooms[newRoomType].room[randomNumber].rightExit.x != 0){

                newLocation = calculateLocation(location, type, roomNumber, newRoomType, randomNumber, newRoomEntrance);
                if(checkBlueprint(newLocation, theRooms[newRoomType].room[randomNumber].width, theRooms[newRoomType].room[randomNumber].height) == true){
                    return true;
                }
            }
            else if(newRoomEntrance == 2 && theRooms[newRoomType].room[randomNumber].downExit.x != 0){

                newLocation = calculateLocation(location, type, roomNumber, newRoomType, randomNumber, newRoomEntrance);
                if(checkBlueprint(newLocation, theRooms[newRoomType].room[randomNumber].width, theRooms[newRoomType].room[randomNumber].height) == true){
                    return true;
                }
            }
            else if(newRoomEntrance == 3 && theRooms[newRoomType].room[randomNumber].leftExit.x != 0){

                newLocation = calculateLocation(location, type, roomNumber, newRoomType, randomNumber, newRoomEntrance);
                if(checkBlueprint(newLocation, theRooms[newRoomType].room[randomNumber].width, theRooms[newRoomType].room[randomNumber].height) == true){
                    return true;
                }
            }
            Debug.Log(randomNumber + " Did not work");
            randomNumber++;
            if(randomNumber >= theRooms[newRoomType].room.Length){
                Debug.Log("End of list, reset");
                randomNumber = 0;
            }
        }while(randomNumber != startingPoint);

        Debug.LogError("No room was selected");
        return false;
    }




/* 
        int selectNewRoom(int newRoomType, int newRoomEntrance){

        //select a room that has the correct entrance
        startingPoint = UnityEngine.Random.Range(0, theRooms[newRoomType].room.Length);
        randomNumber = startingPoint;
        if(newRoomEntrance == 0){

            while(theRooms[newRoomType].room[randomNumber].upExit.x == 0){
                randomNumber++;
                Debug.Log("Checking room number: " + randomNumber);


                if (randomNumber >= theRooms[newRoomType].room.Length){
                    Debug.Log("End of list, reset");
                    randomNumber = 0;
                }
                
                if(randomNumber == startingPoint){
                    Debug.Log("No sutable rooms were found");
                    break;
                }
            }
            Debug.Log("No sutable rooms were found");
        }
        else if(newRoomEntrance == 1){

            while(theRooms[newRoomType].room[randomNumber].rightExit.x == 0){
                randomNumber++;

                if (randomNumber >= theRooms[newRoomType].room.Length){
                    randomNumber = 0;
                }
                
                if(randomNumber == startingPoint){
                    Debug.Log("No sutable rooms were found");
                    break;
                }
            }
            
        }
        else if(newRoomEntrance == 2){

            while(theRooms[newRoomType].room[randomNumber].downExit.x == 0){
                randomNumber++;

                if (randomNumber >= theRooms[newRoomType].room.Length){
                    randomNumber = 0;
                }
                
                if(randomNumber == startingPoint){
                    Debug.Log("No sutable rooms were found");
                    break;
                }
            }
            
        }
        else if(newRoomEntrance == 3){

            while(theRooms[newRoomType].room[randomNumber].leftExit.x == 0){
                randomNumber++;

                if (randomNumber >= theRooms[newRoomType].room.Length){
                    randomNumber = 0;
                }
                
                if(randomNumber == startingPoint){
                    Debug.Log("No sutable rooms were found");
                    break;
                }
            }
        }
        return randomNumber;
    }
*/


    Vector3 calculateLocation(Vector3 location, int type, int roomNumber, int newRoomType, int newRoomNumber, int newRoomEntrance){
        //Calculate the location of the new room
        switch (newRoomEntrance){
            //up entrance
            case 0:
                {
/* 
                    Debug.Log("DOWN EXIT FORMULA");
                    Debug.Log("FORMULA DETAILS, old room type: " + type + " room number: " + roomNumber + " new room type: " + newRoomType + " new room number: " + newRoomNumber);
                    Debug.Log("FORMULA: X Location: " + location.x + " + " + theRooms[type].room[roomNumber].downExit.x + " - " + theRooms[newRoomType].room[newRoomNumber].upExit.x + " = " + (location.x + (theRooms[type].room[roomNumber].downExit.x - theRooms[newRoomType].room[newRoomNumber].upExit.x)));
                    Debug.Log("FORMULA: Y Location: " + location.y + " - " + theRooms[newRoomType].room[newRoomNumber].height + " = " + (location.y - theRooms[newRoomType].room[newRoomNumber].height));
*/
                    newLocation = new Vector3((location.x + (theRooms[type].room[roomNumber].downExit.x - theRooms[newRoomType].room[newRoomNumber].upExit.x)), (location.y - theRooms[newRoomType].room[newRoomNumber].height), 0f);
                    break;
                }
            //right entrance
            case 1:
                {
/* 
                    Debug.Log("LEFT EXIT FORMULA");
                    Debug.Log("FORMULA DETAILS, old room type: " + type + " room number: " + roomNumber + " new room type: " + newRoomType + " new room number: " + newRoomNumber);
                    Debug.Log("FORMULA: X Location: " + location.x + " - " + theRooms[newRoomType].room[newRoomNumber].width + " = " + (location.x - theRooms[newRoomType].room[newRoomNumber].width));
                    Debug.Log("FORMULA: Y Location: " + location.y + " + " + theRooms[type].room[roomNumber].leftExit.y + " - " + theRooms[newRoomType].room[newRoomNumber].rightExit.y + " = " + (location.y + (theRooms[type].room[roomNumber].leftExit.y - theRooms[newRoomType].room[newRoomNumber].rightExit.y)));
*/
                    newLocation = new Vector3((location.x - theRooms[newRoomType].room[newRoomNumber].width), (location.y + (theRooms[type].room[roomNumber].leftExit.y - theRooms[newRoomType].room[newRoomNumber].rightExit.y)), 0f);
                    break;
                }
            //down entrance
            case 2:
                {   
/* 
                    Debug.Log("UP EXIT FORMULA");
                    Debug.Log("FORMULA DETAILS, old room type: " + type + " room number: " + roomNumber + " new room type: " + newRoomType + " new room number: " + newRoomNumber);
                    Debug.Log("FORMULA: X Location: " + location.x + " + " + theRooms[type].room[roomNumber].upExit.x + " - " + theRooms[newRoomType].room[newRoomNumber].downExit.x + " = " + (location.x + (theRooms[type].room[roomNumber].upExit.x - theRooms[newRoomType].room[newRoomNumber].downExit.x)));
                    Debug.Log("FORMULA: Y Location: " + location.y + " + " + theRooms[type].room[roomNumber].height + " = " + (location.y + theRooms[type].room[roomNumber].height));
*/                                
                    newLocation = new Vector3((location.x + (theRooms[type].room[roomNumber].upExit.x - theRooms[newRoomType].room[newRoomNumber].downExit.x)), (location.y + theRooms[type].room[roomNumber].height), 0f);
                    break;
                }
            //left entrance
            case 3:
                {
/* 
                    Debug.Log("RIGHT EXIT FORMULA");
                    Debug.Log("FORMULA DETAILS, old room type: " + type + " room number: " + roomNumber + " new room type: " + newRoomType + " new room number: " + newRoomNumber);
                    Debug.Log("FORMULA: X Location: " + location.x + " + " + theRooms[type].room[roomNumber].width + " = " + (location.x + theRooms[type].room[roomNumber].width));
                    Debug.Log("FORMULA: Y Location: " + location.y + " + " + theRooms[type].room[roomNumber].rightExit.y + " - " + theRooms[newRoomType].room[newRoomNumber].leftExit.y + " = " + (location.y + (theRooms[type].room[roomNumber].rightExit.y - theRooms[newRoomType].room[newRoomNumber].leftExit.y)));
*/                    
                    newLocation = new Vector3((location.x + theRooms[type].room[roomNumber].width), (location.y + (theRooms[type].room[roomNumber].rightExit.y - theRooms[newRoomType].room[newRoomNumber].leftExit.y)), 0f);
                    break;
                }
        }
        return newLocation;
    }



    void DebugBranches(){

        string outputMain = "The main branch is ";
        string outputSide = "The side branchs are ";

        for(int x = 0; x < leftBranchLayout.Length; x++){
            outputMain += leftBranchLayout[x][0].ToString();
            outputMain += ", ";

            if(leftBranchLayout[x].Length > 1){
                for (int y = 0; y < leftBranchLayout[x].Length; y++){
                    outputSide += leftBranchLayout[x][y].ToString();
                    outputSide += ", ";
                }
                outputSide += " And ";
            }
        }
        Debug.Log(outputMain);
        Debug.Log(outputSide);  
    }



    void DebugBreaches(){

        GameObject instance = (GameObject) Instantiate(debugBreach);
        Vector3 location = new Vector3((worldWidth/2)-30, (worldHeight/2)-2, 0f);
        instance.transform.position = location;
        instance.transform.SetParent (worldContainer);

        GameManager.instance.addBreachToList(instance);

    }
}
