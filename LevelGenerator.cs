using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public bool debugMode;
    //each branch represents the connection paths between rooms in the tree
    private int leftTunnelLength;
    private int rightTunnelLength;
    private int roomWidth = 63;
    private int roomHeight = 63;
    private int levelWidth = 100;
    private int levelHeight = 100;
    private int[,] levelBlueprint;

    //The following are a list of arrays containing rooms with different sets of exits
    public GameObject RoomContainer;
    public GameObject CoreRoom;
    public GameObject[] ULRooms;
    public GameObject[] UDRooms;
    public GameObject[] URRooms;
    public GameObject[] LDRooms;
    public GameObject[] LRRooms;
    public GameObject[] DRRooms;

    //Split rooms
    public GameObject[] URDRooms;
    public GameObject[] RDLRooms;
    public GameObject[] UDLRooms;
    public GameObject[] URLRooms;
    

    //The Blueprint room number prefabs used for debuging
    public GameObject BPContainer;
    public GameObject BPRoom0Prefab;
    public GameObject BPRoom1Prefab;
    public GameObject BPRoom2Prefab;
    public GameObject BPRoom3Prefab;
    public GameObject BPRoom4Prefab;
    public GameObject BPRoom5Prefab;
    public GameObject BPRoom6Prefab;
    public GameObject BPRoom7Prefab;
    public GameObject BPRoom8Prefab;
    public GameObject BPRoom9Prefab;
    public GameObject BPRoom10Prefab;
    public RoomReference coreRoomReference;

    public struct TunnelBP{
        public int[,] levelBP;
        public RoomReference roomReference;

        public TunnelBP(int[,] levelBP, RoomReference roomReference){
            this.levelBP = levelBP;
            this.roomReference = roomReference;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        NewLevel();
    }



    // Update is called once per frame
    void Update()
    {
        
    }



    public void NewLevel(){

        TunnelBP tunnelBP;
        RoomReference firstLeftTunnelRoom;
        RoomReference firstRightTunnelRoom;

        //create both tunnels traveling left and right
        int[][] leftTunnel  = SetupTunnel();
        int[][] rightTunnel = SetupTunnel();

        //this is the setup for the core blueprint
        do{
            tunnelBP = SetupCoreBlueprint();
        }while(tunnelBP.levelBP == null);
        
        coreRoomReference = tunnelBP.roomReference;
        tunnelBP.roomReference = null;
        tunnelBP.levelBP = null;

        //this is to setup the left tunnel blueprint
        //tunnelBP = SetupTunnelBlueprint(true, leftTunnel, levelBlueprint, coreRoomReference);
        do{
            tunnelBP = SetupTunnelBlueprint(true, leftTunnel, levelBlueprint, coreRoomReference);
        }while(tunnelBP.levelBP == null);

        firstLeftTunnelRoom = tunnelBP.roomReference;
        coreRoomReference.children.Add(firstLeftTunnelRoom);
        levelBlueprint = tunnelBP.levelBP;
        tunnelBP.roomReference = null;
        tunnelBP.levelBP = null;

        //this is to setup the right tunnel blueprint
        //tunnelBP = SetupTunnelBlueprint(false, rightTunnel, levelBlueprint, coreRoomReference);
        do{
           tunnelBP = SetupTunnelBlueprint(false, rightTunnel, levelBlueprint, coreRoomReference);
        }while(tunnelBP.levelBP == null);

        firstRightTunnelRoom = tunnelBP.roomReference;
        coreRoomReference.children.Add(firstRightTunnelRoom);
        levelBlueprint = tunnelBP.levelBP;
        tunnelBP.roomReference = null;
        tunnelBP.levelBP = null;

        if(debugMode){
            //DebugRoomReference(coreRoomReference);
        }

        //Takes the first room reference and constructs the level from it
        ConstructLevel(coreRoomReference);
    }



    //Generates an array of arrays that dictates what room type exist along the tunnel. 
    int[][] SetupTunnel(){
/* 
        Room types are as follows.
        0.  Empty
        1.	HQ
        2.  Standard
        3.	Defense
        4.	Mine
        5.	Reward
        6.	Teleport
        7.	Upgrade
        8.	Split
        9.	Mini Hive
        10. Main Hive
*/

        //decides the length of the path and when the path will next split.
        int tunnelLength        = Random.Range(25, 31);//between 25 and 30
        int nextTunnelSplit     = Random.Range(8, 11);//between 8 and 10
        int nextTunnelMiniHive  = Random.Range(6, 9);//between 6 and 8
        int smallTunnelLength;

        int roomCount = 0;
        //initialize the first array containing all of the other arrays.
        int[][] tunnelLayout    = new int [tunnelLength][];
        
        //decide the room type for each room along the tunnel.
        for(int x = 0; x < tunnelLength; x++){

            //creates a room to split the tunnel creating two new tunnels in the tunnel system
            if(x == (tunnelLength-1)){
                tunnelLayout[x]     = new int[1];
                tunnelLayout[x][0]  = 10;
                roomCount++;
            }
            else if(x == nextTunnelSplit){

                smallTunnelLength = Random.Range(5, 8);//between 5 and 7
                tunnelLayout[x] = new int[smallTunnelLength];

                //build the small tunnel off the main tunnel
                for(int y = 0; y < smallTunnelLength; y++){
                    
                    //if its the start of the tunnel split spawn a split room
                    if(y == 0){
                        tunnelLayout[x][y] = 8;
                        roomCount++;
                    }
                    //if its the end of the branch spawn a reward room
                    else if(y == (smallTunnelLength - 1)){
                        tunnelLayout[x][y] = Random.Range(5, 8);//between 5 and 7
                        roomCount++;
                    }
                    else if(roomCount >= nextTunnelMiniHive){
                        tunnelLayout[x][y] = 9;
                        nextTunnelMiniHive  = Random.Range(6, 9);//between 6 and 8
                        roomCount = 0;
                    }
                    //if its just a section inside the branch then spawn a random room
                    else{
                        tunnelLayout[x][y] = Random.Range(2, 5);//between 2 and 4
                        roomCount++;
                    }
                }
                nextTunnelSplit = nextTunnelSplit + Random.Range(8, 11);//between 8 and 10
            }
            else if(roomCount >= nextTunnelMiniHive){
                tunnelLayout[x]     = new int[1];
                tunnelLayout[x][0]  = 9;
                nextTunnelMiniHive  = Random.Range(6, 9);//between 6 and 8
                roomCount           = 0;
            }
            else{
                tunnelLayout[x] = new int[1];
                tunnelLayout[x][0] = Random.Range(2, 5);//between 2 and 4
                roomCount++;
            }
            
        }

        if(debugMode){
            string outputMain = "The main tunnel is ";
            string outputSide = "The side tunnels are ";

            for(int x = 0; x < tunnelLayout.Length; x++){
                outputMain += tunnelLayout[x][0].ToString();
                outputMain += ", ";

                if(tunnelLayout[x].Length > 1){
                    for (int y = 0; y < tunnelLayout[x].Length; y++){
                        outputSide += tunnelLayout[x][y].ToString();
                        outputSide += ", ";
                    }
                    outputSide += " And ";
                }
            }
            Debug.Log(outputMain);
            Debug.Log(outputSide);  
        }

        return tunnelLayout;
    }



    TunnelBP SetupCoreBlueprint(){

        levelBlueprint = new int[levelWidth, levelHeight];
        levelBlueprint[levelWidth/2, levelHeight/2] = 1;
        int[] newExits = new int[4];
        newExits[1] = 1;
        newExits[3] = 1;

        RoomReference roomReference = new RoomReference(levelWidth/2, levelHeight/2, 1, newExits, 0, 3, 1, null);
        if(debugMode){DebugBlueprint(1, levelWidth/2, levelHeight/2);}//leave this here for debug purposes, shows where the display was called from
        return new TunnelBP(levelBlueprint, roomReference);
    }

    //This BP is used only to check for free space when placing rooms and for a debug display at the end
    //as each room is finalised they are placed in a sorted tree of rooms
    //returns the HQ but all other rooms exist as children of the HQ.
    //left tunnel unput is to indicate if its building a left or right tunnel BP
    TunnelBP SetupTunnelBlueprint(bool leftTunnel, int[][] tunnel, int[,] levelBlueprint, RoomReference coreRoomReference){

        //This room reference is for the room left of the HQ and will be returned here.
        RoomReference tunnelStartReference = new RoomReference();;
        RoomReference tempReferenceOne = new RoomReference();//will represente the previous rooms information
        RoomReference tempReferenceTwo = new RoomReference();//will represent the previous split rooms information
        //I start with the HQ location, I create a room refernce for the first room left of the HQ
        //I have to make new room references every time a new room is made and add them as a child 
        //of the previous room
        int roomXlocation = levelWidth / 2;
        int roomYlocation = levelHeight / 2;

        int splitSavedroomXlocation = -1;
        int splitSavedroomYlocation = -1;

        int entrance    = -1;
        int newExitOne  = -1;
        int newExitTwo  = -1;

        int[] newExits = new int[4];
        bool isSplitPath = false;


        //NEW
        for(int x = 0; x < tunnel.GetLength(0); x++){
            for(int y = 0; y < tunnel[x].Length; y++){

                if(x == 0 && y == 0){
                    
                    if(leftTunnel){
                        roomXlocation--;//set the rooms location left of the core room
                        entrance = 1;
                        newExitOne = 3;
                    }else{
                        roomXlocation++;//set the rooms location right of the core room
                        entrance = 3;
                        newExitOne = 1;
                    }

                    newExitTwo = -1;

                    newExits[entrance] = 1;
                    newExits[newExitOne] = Random.Range(1, 3);//between 1 and 2
                    
                    tunnelStartReference = new RoomReference(roomXlocation, roomYlocation, tunnel[x][y], newExits, entrance, newExitOne, newExitTwo, coreRoomReference);
                    tempReferenceOne = tunnelStartReference;
                    if(debugMode){DebugBlueprint(tunnel[x][y], roomXlocation, roomYlocation);}//leave this here for debug purposes, shows where the display was called from



                //if this is the second room after a split room
                }else if(y == 0 && isSplitPath == true){
                    isSplitPath = false;
                    
                    //the temp room is a stored reference to the split room
                    roomXlocation = splitSavedroomXlocation;
                    roomYlocation = splitSavedroomYlocation;

                    newExitOne = tempReferenceTwo.exitReferences[1];
                    newExitTwo = -1;
                    //if an exit could not be found return null
                    if(newExitOne == -1){
                        return new TunnelBP(null, null);
                    }

                    //get the new exit off of the stored temp ref exit
                    newExits[newExitOne] = Random.Range(1, 3);//between 1 and 2
                    entrance = OppositeExit(tempReferenceTwo.exitReferences[1]);
                    newExits[entrance] = tempReferenceTwo.exits[tempReferenceTwo.exitReferences[1]];

                    //int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit
                    RoomReference newRoomReference = new RoomReference(roomXlocation, roomYlocation, tunnel[x][y], newExits, entrance, newExitOne, newExitTwo, tempReferenceTwo);
                    //add this new room reference as a child to its parent
                    tempReferenceTwo.children.Add(newRoomReference);

                    tempReferenceOne = newRoomReference;
                    if(debugMode){DebugBlueprint(tunnel[x][y], roomXlocation, roomYlocation);}//leave this here for debug purposes, shows where the display was called from
                


                //this is the first room after a split
                }else if(y == 1){

                    //This rooms exit should be the same as the previous rooms exit
                    newExitOne = tempReferenceOne.exitReferences[0];
                    newExitTwo = -1;
                    //if an exit could not be found return null
                    if(newExitOne == -1){
                        return new TunnelBP(null, null);
                    }

                    newExits[newExitOne] = Random.Range(1, 3);//between 1 and 2
                    entrance = OppositeExit(tempReferenceOne.exitReferences[0]);
                    newExits[entrance] = tempReferenceOne.exits[tempReferenceOne.exitReferences[0]];

                    //int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit
                    RoomReference newRoomReference = new RoomReference(roomXlocation, roomYlocation, tunnel[x][y], newExits, entrance, newExitOne, newExitTwo, tempReferenceOne);
                    //add this new room reference as a child to its parent
                    tempReferenceOne.children.Add(newRoomReference);

                    tempReferenceOne = newRoomReference;
                    if(debugMode){DebugBlueprint(tunnel[x][y], roomXlocation, roomYlocation);}//leave this here for debug purposes, shows where the display was called from



                //this is a split room, type 8
                }else if(tunnel[x][y] == 8 && y == 0 && tunnel[x].Length > 1){
                    //Set split path to true and save the current rooms location for the loop after this split tunnel ends
                    isSplitPath = true;

                    splitSavedroomXlocation = roomXlocation;
                    splitSavedroomYlocation = roomYlocation;

                    entrance = OppositeExit(tempReferenceOne.exitReferences[0]);
                    newExits[entrance] = tempReferenceOne.exits[tempReferenceOne.exitReferences[0]];

                    //select a new exit but exclude the existing exit
                    newExitOne = SelectExit(leftTunnel, roomXlocation, roomYlocation, entrance);
                    //if an exit could not be found return null
                    if(newExitOne == -1){
                        return new TunnelBP(null, null);
                    }

                    //select an additional exit and save it for after the new exit
                    newExitTwo = SelectExit(leftTunnel, roomXlocation, roomYlocation, entrance, newExitOne);
                    if(newExitTwo == -1){
                        return new TunnelBP(null, null);
                    }

                    newExits[newExitOne] = Random.Range(1, 3);//between 1 and 2
                    newExits[newExitTwo] = Random.Range(1, 3);//between 1 and 2

                    //int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit
                    RoomReference newRoomReference = new RoomReference(roomXlocation, roomYlocation, tunnel[x][y], newExits, entrance, newExitOne, newExitTwo, tempReferenceOne);
                    //add this new room reference as a child to its parent
                    tempReferenceOne.children.Add(newRoomReference);
                                        
                    //store a copy of this room reference for future use after the split
                    tempReferenceTwo = newRoomReference;
                    tempReferenceOne = newRoomReference;
                    if(debugMode){DebugBlueprint(tunnel[x][y], roomXlocation, roomYlocation);}//leave this here for debug purposes, shows where the display was called from
                


                //if this is a standard room
                }else if(x > 0 && (tunnel[x].Length == 1 || y > 0)){

                    entrance = OppositeExit(tempReferenceOne.exitReferences[0]);
                    newExits[entrance] = tempReferenceOne.exits[tempReferenceOne.exitReferences[0]];

                    //select a new exit but exclude the existing exit
                    newExitOne = SelectExit(leftTunnel, roomXlocation, roomYlocation, entrance);
                    newExitTwo = -1;
                    //if an exit could not be found return null
                    if(newExitOne == -1){
                        return new TunnelBP(null, null);
                    }

                    newExits[newExitOne] = Random.Range(1, 3);//between 1 and 2

                    //int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit
                    RoomReference newRoomReference = new RoomReference(roomXlocation, roomYlocation, tunnel[x][y], newExits, entrance, newExitOne, newExitTwo, tempReferenceOne);
                    
                    //add this new room reference as a child to its parent
                    tempReferenceOne.children.Add(newRoomReference);

                    tempReferenceOne = newRoomReference;
                    if(debugMode){DebugBlueprint(tunnel[x][y], roomXlocation, roomYlocation);}//leave this here for debug purposes, shows where the display was called from


                
                //this is a room just after a split room, its exitB should be directly accross from exitA
                }else{
                    Debug.LogWarning("An exception has occured, please look into this");
                    Debug.Break();
                    return new TunnelBP(null, null);
                }

                //Store the new reserved space in the level blueprint
                levelBlueprint[roomXlocation, roomYlocation] = tunnel[x][y];
                
                switch (newExitOne){
                    case 0:
                        roomYlocation++;
                        break;
                    case 1:
                        roomXlocation++;
                        break;
                    case 2:
                        roomYlocation--;
                        break;
                    case 3:
                        roomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using newExitOne");
                        Debug.LogWarning("newExitOne: " + newExitOne);
                        return new TunnelBP(null, null);
                }
                if(newExitTwo != -1){
                    switch (newExitTwo){
                    case 0:
                        splitSavedroomYlocation++;
                        break;
                    case 1:
                        splitSavedroomXlocation++;
                        break;
                    case 2:
                        splitSavedroomYlocation--;
                        break;
                    case 3:
                        splitSavedroomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using newExitTwo");
                        return new TunnelBP(null, null);
                    }
                    newExitTwo = -1;
                }

                //reset exits for the next loop
                
                for (int i = 0; i < newExits.Length; i++){
                    newExits[i] = 0;
                }
                
            }
        }
        return new TunnelBP(levelBlueprint, tunnelStartReference);
    }



    void ConstructLevel(RoomReference roomRef){

        //what I need to do is take the core rooms reference
        //if its type is zero place the room at its coordantes
        //add the room itsself to its reference.
        //ensure the rooms exits are correct based on its parents exit
        
        //I NEED EACH ROOM REFERENCE TO HAVE A CLEAR UNDERSTANDING OF ITS OWN EXIT TYPES, 1 OR 2


        //checks if the room is the core, in which case has a custom placement
        if(roomRef.type == 1){
            roomRef.room = Instantiate(CoreRoom, new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            roomRef.isRoomPowered = true;

        }else{
            //This block of statments checks what exits are being used and selects from the opproperate list to use for the room.
            if(roomRef.exits[0] != 0 && roomRef.exits[1] != 0 && roomRef.exits[2] != 0){

                roomRef.room = Instantiate(URDRooms[Random.Range(0, URDRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[1] != 0 && roomRef.exits[2] != 0 && roomRef.exits[3] != 0){
                        
                roomRef.room = Instantiate(RDLRooms[Random.Range(0, RDLRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[0] != 0 && roomRef.exits[2] != 0 && roomRef.exits[3] != 0){
                        
                roomRef.room = Instantiate(UDLRooms[Random.Range(0, UDLRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[0] != 0 && roomRef.exits[1] != 0 && roomRef.exits[3] != 0){
                        
                roomRef.room = Instantiate(URLRooms[Random.Range(0, URLRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[0] != 0 && roomRef.exits[3] != 0){
                        
                roomRef.room = Instantiate(ULRooms[Random.Range(0, ULRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[0] != 0 && roomRef.exits[2] != 0){
                        
                roomRef.room = Instantiate(UDRooms[Random.Range(0, UDRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);           
            }else if(roomRef.exits[0] != 0 && roomRef.exits[1] != 0){
                        
                roomRef.room = Instantiate(URRooms[Random.Range(0, URRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);        
            }else if(roomRef.exits[3] != 0 && roomRef.exits[2] != 0){
                        
                roomRef.room = Instantiate(LDRooms[Random.Range(0, LDRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);            
            }else if(roomRef.exits[3] != 0 && roomRef.exits[1] != 0){
                        
                roomRef.room = Instantiate(LRRooms[Random.Range(0, LRRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }else if(roomRef.exits[2] != 0 && roomRef.exits[1] != 0){
                        
                roomRef.room = Instantiate(DRRooms[Random.Range(0, DRRooms.Length)], new Vector3(roomRef.x * roomWidth, roomRef.y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
            }

            if(roomRef.room == null){
                Debug.LogWarning("The fucks happened here?");
                Debug.Break();
            }

            if(roomRef.room != null){
                roomRef.room.GetComponent<Room>().NewRoom(ref roomRef);
            }
        }
 
        if(roomRef.children.Count == 0){


        }else if(roomRef.children.Count == 1){

            ConstructLevel(roomRef.children[0]);

        }else if(roomRef.children.Count == 2){

            ConstructLevel(roomRef.children[0]);
            ConstructLevel(roomRef.children[1]);
        }

    }



    //selects an exit for the room
    int SelectExit(bool leftTunnel, int roomXLocation, int roomYLocation, int entrance){

        int currentIndex;
        int initiallySelectedExitIndex = -1;
        int[] possibleExits;

        if(leftTunnel){
            switch (entrance){
            case 0:

                possibleExits = new int[]{2, 3};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 1:

                possibleExits = new int[]{0, 2, 3};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 2:

                possibleExits = new int[]{0, 3};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 3:

                possibleExits = new int[]{0, 2};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            default:
                possibleExits = new int[]{};
                Debug.LogWarning("Something went wrong selecting a left tunnel exit");
                Debug.Break();
                break;
            }
        }else{
            switch (entrance){
            case 0:

                possibleExits = new int[]{1, 2};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 1:

                possibleExits = new int[]{0, 2};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 2:

                possibleExits = new int[]{0, 1};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            case 3:

                possibleExits = new int[]{0, 1, 2};
                initiallySelectedExitIndex = Random.Range(0, possibleExits.Length);
                break;
            default:
                possibleExits = new int[]{};
                Debug.LogWarning("Something went wrong selecting a right tunnel exit");
                Debug.Break();
                break;
            }
        }
        

        //check if the space the selected exit faces is empty to place a new room
        //loop through possible exits if it is not available.
        currentIndex = initiallySelectedExitIndex;
        if(initiallySelectedExitIndex != -1){
            do{
                if(currentIndex >= possibleExits.Length){
                    currentIndex = 0;
                }

                switch (possibleExits[currentIndex]){
                    case 0:
                        if(levelBlueprint[roomXLocation, roomYLocation + 1] == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 1:
                        if(levelBlueprint[roomXLocation + 1, roomYLocation] == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 2:
                        if(levelBlueprint[roomXLocation, roomYLocation - 1] == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 3:
                        if(levelBlueprint[roomXLocation - 1, roomYLocation] == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                }
                currentIndex++;

            }while(currentIndex != initiallySelectedExitIndex);
        //An exit with free space has not been found.
        }else{
            Debug.LogWarning("Select Exit Returned -1");
            Debug.Break();

            return -1;
        }
        Debug.LogWarning("Select Exit Returned -1");
        Debug.Break();
        return -1;
    }



    //selects an additional exit for split rooms
    int SelectExit(bool leftTunnel, int roomXLocation, int roomYLocation, int entrance, int firstExit){
        
        //possible exits
        //exit A = 0 Exit B = 1
        //exit A = 0 Exit B = 2
        //exit A = 0 Exit B = 3

        //exit A = 1 Exit B = 2
        //exit A = 1 Exit B = 3

        //exit A = 2 Exit B = 3

        int newExit = -1;
        int initiallySelectedExit;
        int[] possibleExits;

        if(leftTunnel){
            if((entrance == 0 && firstExit == 1) || (entrance == 1 && firstExit == 0)){
                possibleExits = new int[]{2, 3};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 0 && firstExit == 2) || (entrance == 2 && firstExit == 0)){
                possibleExits = new int[]{3};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 0 && firstExit == 3) || (entrance == 3 && firstExit == 0)){
                possibleExits = new int[]{2};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 1 && firstExit == 2) || (entrance == 2 && firstExit == 1)){
                possibleExits = new int[]{0, 3};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 1 && firstExit == 3) || (entrance == 3 && firstExit == 1)){
                possibleExits = new int[]{0, 2};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 2 && firstExit == 3) || (entrance == 3 && firstExit == 2)){
                possibleExits = new int[]{0};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }
        }else{
            if((entrance == 0 && firstExit == 1) || (entrance == 1 && firstExit == 0)){
                possibleExits = new int[]{2};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 0 && firstExit == 2) || (entrance == 2 && firstExit == 0)){
                possibleExits = new int[]{1};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 0 && firstExit == 3) || (entrance == 3 && firstExit == 0)){
                possibleExits = new int[]{1, 2};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 1 && firstExit == 2) || (entrance == 2 && firstExit == 1)){
                possibleExits = new int[]{0};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 1 && firstExit == 3) || (entrance == 3 && firstExit == 1)){
                possibleExits = new int[]{0, 2};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }else if((entrance == 2 && firstExit == 3) || (entrance == 3 && firstExit == 2)){
                possibleExits = new int[]{0, 1};
                newExit = possibleExits[Random.Range(0, possibleExits.Length)];

            }
        }

        //check if the space the selected exit faces is empty to place a new room
        //loop through possible exits if it is not available.
        initiallySelectedExit = newExit;

        do{
            
            while(newExit == entrance || newExit == firstExit){
                newExit++;
                if(newExit >= 4){
                    newExit = 0;
                }   
            }
            //now check if the space the exit faces is empty so there is space for a new room
            //if there is space then return the new exit value.
            switch (newExit){
                case 0:
                    if(levelBlueprint[roomXLocation, roomYLocation + 1] == 0){
                        return newExit;
                    }
                    break;
                case 1:
                    if(levelBlueprint[roomXLocation + 1, roomYLocation] == 0){
                        return newExit;
                    }
                    break;
                case 2:
                    if(levelBlueprint[roomXLocation, roomYLocation - 1] == 0){
                        return newExit;
                    }
                    break;
                case 3:
                    if(levelBlueprint[roomXLocation - 1, roomYLocation] == 0){
                        return newExit;
                    }
                    break;
                default:
                    Debug.LogWarning("While loop looking for exit has broken");
                    Debug.Break();
                    return -1;
            }
            //if the exit doenst have free space beyond itself then try the next one.
            newExit++;

        } while(newExit != initiallySelectedExit);

        //No exit that has a free space could be found.
        Debug.LogWarning("Select Exit Returned -1");
        Debug.Break();
        return -1;
    }



    int OppositeExit(int iExit){
        switch (iExit){
            case 0: return 2;
            case 1: return 3;
            case 2: return 0;
            case 3: return 1;
            default:
                Debug.LogWarning("Something went wrong selecting an opposite exit");
                Debug.Break();
                return -1;
        }
    }



    //creates a visual bp for debuging purposes
    void DebugBlueprint(int type, int x, int y){

        //Debug.Log("Placing a room of type: " + type + " at X: " + x + " by Y: " + y);

        switch (type){
            case 0:
                Instantiate(BPRoom0Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 1:
                Instantiate(BPRoom1Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 2:
                Instantiate(BPRoom2Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 3:
                Instantiate(BPRoom3Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 4:
                Instantiate(BPRoom4Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 5:
                Instantiate(BPRoom5Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 6:
                Instantiate(BPRoom6Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 7:
                Instantiate(BPRoom7Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 8:
                Instantiate(BPRoom8Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 9:
                Instantiate(BPRoom9Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            case 10:
                Instantiate(BPRoom10Prefab, new Vector3(x + BPContainer.transform.position.x, y + BPContainer.transform.position.y, 0), Quaternion.identity, BPContainer.transform);
                break;
            default:
                Debug.LogWarning("Something went wrong placing a room, unusual type input");
                break;
        }
    }


    //recursively display the children of the room references?
    void DebugRoomReference(RoomReference roomRef){
        
        if(roomRef.children.Count == 0){
            Debug.Log("Room Type: " + roomRef.type);
            Debug.Log("End of path");

        }else if(roomRef.children.Count == 1){
            Debug.Log("Room Type: " + roomRef.type);
            DebugRoomReference(roomRef.children[0]);

        }else if(roomRef.children.Count == 2){
            Debug.Log("Room Type: " + roomRef.type);
            DebugRoomReference(roomRef.children[0]);
            DebugRoomReference(roomRef.children[1]);
        }
    }
}