
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDLevelGenerator : MonoBehaviour
{
    public bool debugMode;
    //each branch represents the connection paths between rooms in the tree
    private int[][] leftTunnel;
    private int[][] rightTunnel;
    private int leftTunnelLength;
    private int rightTunnelLength;
    private int roomWidth = 63;
    private int roomHeight = 63;
    private int levelWidth = 100;
    private int levelHeight = 100;
    private BPRoom[,] levelBlueprint;

    //The following are a list of arrays containing rooms with different sets of exits
    public GameObject RoomContainer;
    public GameObject HQRoom;
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

    
    //a custom struct for each of the blueprint rooms, denotes type of room and what exit type is used
    //0 = none, 1 = left exit , 2 = middle exit, 3 = right exit.
    public struct BPRoom{
        public int type;
        public int upExit;
        public int rightExit;
        public int downExit;
        public int leftExit;
		
		public BPRoom(int iType, int iUpExit, int iRightExit, int iDownExit, int iLeftExit){
			type      = iType;
            upExit    = iUpExit;
			rightExit = iRightExit;
            downExit  = iDownExit;
            leftExit  = iLeftExit;
		}
    }

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }



    public void NewLevel(){

        BPRoom[,] levelBP;

        //create both tunnels traveling left and right
        leftTunnel  = SetupTunnel();
        rightTunnel = SetupTunnel();

        //run the setupblueprint method, if the method returns null then run it again
        do{
            levelBP = SetupBlueprint();
            if(debugMode){
                if(levelBP == null){
                    Debug.LogWarning("the level is null, start again");
                }
            }
        }
        while(levelBP == null);

        ConstructLevel(levelBP); //build the level based on the contents of the BPRoom
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
        int tunnelLength        = UnityEngine.Random.Range(25, 30);
        int nextTunnelSplit     = UnityEngine.Random.Range(8, 10);
        int nextTunnelMiniHive  = UnityEngine.Random.Range(7, 8);
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
 
                smallTunnelLength = UnityEngine.Random.Range(5, 7);
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
                        tunnelLayout[x][y] = UnityEngine.Random.Range(5, 7);
                        roomCount++;
                    }
                    else if(roomCount >= nextTunnelMiniHive){
                        tunnelLayout[x][y] = 9;
                        nextTunnelMiniHive  = UnityEngine.Random.Range(7, 8);
                        roomCount = 0;
                    }
                    //if its just a section inside the branch then spawn a random room
                    else{
                        tunnelLayout[x][y] = UnityEngine.Random.Range(2, 4);
                        roomCount++;
                    }
                }
                nextTunnelSplit = nextTunnelSplit + UnityEngine.Random.Range(8, 10);
            }
            else if(roomCount >= nextTunnelMiniHive){
                tunnelLayout[x]     = new int[1];
                tunnelLayout[x][0]  = 9;
                nextTunnelMiniHive  = UnityEngine.Random.Range(7, 8);
                roomCount           = 0;
            }
            else{
                tunnelLayout[x] = new int[1];
                tunnelLayout[x][0] = UnityEngine.Random.Range(2, 4);
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



    void ConstructLevel(BPRoom[,] levelBP){
        //run the whole way through the levelBP and spawn whatever room should be spawned, trigger the rooms new room script and input the room layout.
        GameObject newRoom = null;
/* 
        Rooms are 63 pixels accross
        I need to start from the top left corner of the array
        I need to spawn each room based on a multiple of 63
        
*/

        for(int x = 0; x < levelBP.GetLength(0); x++){
            for(int y = 0; y < levelBP.GetLength(1); y++){

                if(levelBP[x,y].type == 0){

                }else if(levelBP[x,y].type == 1){
                    Instantiate(HQRoom, new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    newRoom = null;
                }else{

                    //based on what exits are open on the array do x y or z
                    if(levelBP[x,y].upExit != 0 && levelBP[x,y].rightExit != 0 && levelBP[x,y].downExit != 0){

                        newRoom = Instantiate(URDRooms[UnityEngine.Random.Range(0, URDRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].rightExit != 0 && levelBP[x,y].downExit != 0 && levelBP[x,y].leftExit != 0){
                        
                        newRoom = Instantiate(RDLRooms[UnityEngine.Random.Range(0, RDLRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].upExit != 0 && levelBP[x,y].downExit != 0 && levelBP[x,y].leftExit != 0){
                        
                        newRoom = Instantiate(UDLRooms[UnityEngine.Random.Range(0, UDLRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].upExit != 0 && levelBP[x,y].rightExit != 0 && levelBP[x,y].leftExit != 0){
                        
                        newRoom = Instantiate(URLRooms[UnityEngine.Random.Range(0, URLRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].upExit != 0 && levelBP[x,y].leftExit != 0){
                        
                        newRoom = Instantiate(ULRooms[UnityEngine.Random.Range(0, ULRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].upExit != 0 && levelBP[x,y].downExit != 0){
                        
                        newRoom = Instantiate(UDRooms[UnityEngine.Random.Range(0, UDRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].upExit != 0 && levelBP[x,y].rightExit != 0){
                        
                        newRoom = Instantiate(URRooms[UnityEngine.Random.Range(0, URRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].leftExit != 0 && levelBP[x,y].downExit != 0){
                        
                        newRoom = Instantiate(LDRooms[UnityEngine.Random.Range(0, LDRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].leftExit != 0 && levelBP[x,y].rightExit != 0){
                        
                        newRoom = Instantiate(LRRooms[UnityEngine.Random.Range(0, LRRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }else if(levelBP[x,y].downExit != 0 && levelBP[x,y].rightExit != 0){
                        
                        newRoom = Instantiate(DRRooms[UnityEngine.Random.Range(0, DRRooms.Length)], new Vector3(x * roomWidth, y * roomHeight, 0), Quaternion.identity, RoomContainer.transform);
                    }
                    if(newRoom != null){
                        //newRoom.GetComponent<Room>().NewRoom(levelBP[x,y].type, levelBP[x,y].upExit, levelBP[x,y].rightExit, levelBP[x,y].downExit, levelBP[x,y].leftExit);
                    }
                    
                }
            }
        }


    }

    //creates a 2D array of the map, filling in the rooms based on the tunnels 
    BPRoom[,] SetupBlueprint(){
/* 
        the BP needs to store the location of each room, its entrance and its exit.
        room exits are denoted with a number,
        0 = north
        1 = east
        2 = south
        3 = west

        //create room object with its location, type, entrance, exit, 
*/

        int roomXlocation = levelWidth / 2;
        int roomYlocation = levelHeight / 2;

        levelBlueprint = new BPRoom[levelWidth, levelHeight];
        levelBlueprint[roomXlocation, roomYlocation] = new BPRoom(1, 0, 1, 0, 1);
        Debug.Log("HQ Room");
        if(debugMode){DebugBlueprint(1, roomXlocation, roomYlocation);}

        int splitSavedroomXlocation = -1;
        int splitSavedroomYlocation = -1;
        int splitSavedExit = -1;
        int exitA = -1;
        int exitB = -1;
        int exitC = -1;

        bool isSplitPath = false;

        //exit A
        //exit B
        //exit C

        //Exit A will always store the previous rooms direction
        //Exit B will always store the rooms next room direction
        //Exit C is used at tunnel splits to store the third exit
    
/*
        if x and y are both zero then place the first room with entrance = to 

        if X is 0 and Y's length is larger then 1 we are at a split, so place a split room
        save the current rooms location until X = 0 again and we continue the path

        if X is 0 and split path is set to true then relocate to the saved X and Y and 
        set split path to false again

        in all other cases just place the room as standard.  

        exit A is a direction
        Each exit also has a type

        placement cycle is

        1. Take the existing exitA
        2. Select exitB excluding exitA
        3. Based on both exit and room type place the new room in the 2D BP array
        4. Set exitA for the next room
        5. Repeat
*/
        //build the left tunnel first

        //JOHN, WHATS CAUSING THE CRASH IS IN THE NEXT SECTION OF CODE
        for(int x = 0; x < leftTunnel.GetLength(0); x++){
            for(int y = 0; y < leftTunnel[x].Length; y++){
                if(debugMode){Debug.Log("X: " + x + " Y: " + y);}



                //This is the start of the tunnel
                if(x == 0 && y == 0){

                    exitA = 1;  //set this tunnels first room starting exit facing the HQ.  
                    roomXlocation--;    //set the rooms location next to the HQ.

                    //select a new exit but exclude the existing exit
                    exitB = OppositeExit(exitA);
                    if(exitB == -1){
                        return null;
                    }

                    //create the new BP room based on its exits
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(leftTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(leftTunnel[x][y], roomXlocation, roomYlocation);}





                //This is a normal room placement  
                }else if(y == 0 && isSplitPath == true){
                    isSplitPath = false;
                    //disable the split path bool
                    Debug.Log("splitSavedroomXlocation: " + splitSavedroomXlocation);
                    Debug.Log("splitSavedroomYlocation: " + splitSavedroomYlocation);
                    //take the x and y location from the stored saved location
                    roomXlocation = splitSavedroomXlocation;
                    roomYlocation = splitSavedroomYlocation;
                    //this rooms exit will be equil to the split rooms exit
                    exitB = OppositeExit(splitSavedExit);
                    if(exitB == -1){
                        return null;
                    }
                    //add this to the level blueprint
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(leftTunnel[x][y], splitSavedExit, exitB);
                    //spawn debug if necessary
                    if(debugMode){DebugBlueprint(leftTunnel[x][y], roomXlocation, roomYlocation);}





                }else if(x > 0 && (leftTunnel[x].Length == 1 || y > 0)){
                    //select a new exit but exclude the existing exit
                    exitB = SelectExit(true, roomXlocation, roomYlocation, exitA);
                    //if an exit could not be found
                    if(exitB == -1){
                        return null;
                    }
                    //create the new BP room based on its exits
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(leftTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(leftTunnel[x][y], roomXlocation, roomYlocation);}





                //this is split room
                }else if(y == 0 && leftTunnel[x].Length > 1){
                    //Set split path to true and save the current rooms location for the loop after this split tunnel ends
                    isSplitPath = true;
                    Debug.Log("isSplitPath: " + isSplitPath);

                    //the roomXlocation needs to be saved in a state after the move based on the exit
                    splitSavedroomXlocation = roomXlocation;
                    splitSavedroomYlocation = roomYlocation;

                    //select a new exit but exclude the existing exit
                    exitB = SelectExit(true, roomXlocation, roomYlocation, exitA);
                    if(exitB == -1){
                        return null;
                    }

                    //select an additional exit and save it for after the new exit
                    exitC = SelectExit(true, roomXlocation, roomYlocation, exitA, exitB);
                    if(exitC == -1){
                        return null;
                    }

                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(leftTunnel[x][y], exitA, exitB, exitC);
                    if(debugMode){DebugBlueprint(leftTunnel[x][y], roomXlocation, roomYlocation);}
                    //we are placing a split room, 
                    // 1. find 2 additional exits
                    // 2. bool isSplit true
                    // 3. save the current X and Y location so that when we can jump back to them at the next increase in X.






                //this is a room just after a split room, its exitB should be directly accross from exitA
                }else if(y == 1){
                    //its possible the previous pass did not input the correct co-ordinates for this new room
                    exitB = OppositeExit(exitA);
                    if(exitB == -1){
                        return null;
                    }

                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(leftTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(leftTunnel[x][y], roomXlocation, roomYlocation);}





                //this is a room just after a split room, its exitB should be directly accross from exitA
                }else{
                    Debug.LogWarning("An exception has occured, please look into this");
                    Debug.Break();
                    return null;
                }




  
/* 
                This is done at the end of each room placement
                ExitA is updated so the next loop knows what exit is already being used by the previous room
                the room X and Y location are also updated for the next loop
*/
                switch (exitB){
                    case 0:
                        exitA = 2;
                        roomYlocation++;
                        break;
                    case 1:
                        exitA = 3;
                        roomXlocation++;
                        break;
                    case 2:
                        exitA = 0;
                        roomYlocation--;
                        break;
                    case 3:
                        exitA = 1;
                        roomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using exitB");
                        return null;
                }
                //if exitC is being used then its exit space should be saved and the split saved x and y locations be adjusted 
                if(exitC != -1){
                    switch (exitC){
                    case 0:
                        splitSavedExit = 2;
                        splitSavedroomYlocation++;
                        break;
                    case 1:
                        splitSavedExit = 3;
                        splitSavedroomXlocation++;
                        break;
                    case 2:
                        splitSavedExit = 0;
                        splitSavedroomYlocation--;
                        break;
                    case 3:
                        splitSavedExit = 1;
                        splitSavedroomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using exitC");
                        return null;
                    }
                    exitC = -1;
                }
            }
        }





        roomXlocation = levelWidth / 2;
        roomYlocation = levelHeight / 2;

        //Now the right tunnel
        for(int x = 0; x < rightTunnel.GetLength(0); x++){
            for(int y = 0; y < rightTunnel[x].Length; y++){
                if(debugMode){Debug.Log("X: " + x + " Y: " + y);}



                //This is the start of the tunnel
                if(x == 0 && y == 0){
                    
                    exitA = 3;//set this tunnels first room starting exit facing the HQ.
                    roomXlocation++;//set the rooms location next to the HQ.

                    //select a new exit but exclude the existing exit
                    exitB = OppositeExit(exitA);
                    if(exitB == -1){
                        return null;
                    }

                    //create the new BP room based on its exits
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(rightTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(rightTunnel[x][y], roomXlocation, roomYlocation);}





                //This is a normal room placement  
                }else if(y == 0 && isSplitPath == true){
                    isSplitPath = false;

                    Debug.Log("splitSavedroomXlocation: " + splitSavedroomXlocation);
                    Debug.Log("splitSavedroomYlocation: " + splitSavedroomYlocation);

                    roomXlocation = splitSavedroomXlocation;
                    roomYlocation = splitSavedroomYlocation;
                    
                    exitB = OppositeExit(splitSavedExit);
                    if(exitB == -1){
                        return null;
                    }
                    
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(rightTunnel[x][y], splitSavedExit, exitB);
                    
                    if(debugMode){DebugBlueprint(rightTunnel[x][y], roomXlocation, roomYlocation);}





                }else if(x > 0 && (rightTunnel[x].Length == 1 || y > 0)){
                    //select a new exit but exclude the existing exit
                    exitB = SelectExit(false, roomXlocation, roomYlocation, exitA);
                    //if an exit could not be found
                    if(exitB == -1){
                        return null;
                    }
                    //create the new BP room based on its exits
                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(rightTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(rightTunnel[x][y], roomXlocation, roomYlocation);}





                //this is split room
                }else if(y == 0 && rightTunnel[x].Length > 1){
                    //Set split path to true and save the current rooms location for the loop after this split tunnel ends
                    isSplitPath = true;
                    Debug.Log("isSplitPath: " + isSplitPath);

                    //the roomXlocation needs to be saved in a state after the move based on the exit
                    splitSavedroomXlocation = roomXlocation;
                    splitSavedroomYlocation = roomYlocation;

                    //select a new exit but exclude the existing exit
                    exitB = SelectExit(false, roomXlocation, roomYlocation, exitA);
                    if(exitB == -1){
                        return null;
                    }

                    //select an additional exit and save it for after the new exit
                    exitC = SelectExit(false, roomXlocation, roomYlocation, exitA, exitB);
                    if(exitC == -1){
                        return null;
                    }

                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(rightTunnel[x][y], exitA, exitB, exitC);
                    if(debugMode){DebugBlueprint(rightTunnel[x][y], roomXlocation, roomYlocation);}
                    //we are placing a split room, 
                    // 1. find 2 additional exits
                    // 2. bool isSplit true
                    // 3. save the current X and Y location so that when we can jump back to them at the next increase in X.






                //this is a room just after a split room, its exitB should be directly accross from exitA
                }else if(y == 1){
                    //its possible the previous pass did not input the correct co-ordinates for this new room
                    exitB = OppositeExit(exitA);
                    if(exitB == -1){
                        return null;
                    }

                    levelBlueprint[roomXlocation, roomYlocation] = NewBPRoom(rightTunnel[x][y], exitA, exitB);
                    if(debugMode){DebugBlueprint(rightTunnel[x][y], roomXlocation, roomYlocation);}
                
                
                
                
                
                //this is a room just after a split room, its exitB should be directly accross from exitA
                }else{
                    Debug.LogWarning("An exception has occured, please look into this");
                    Debug.Break();
                    return null;
                }





/* 
                This is done at the end of each room placement
                ExitA is updated so the next loop knows what exit is already being used by the previous room
                the room X and Y location are also updated for the next loop
*/
                switch (exitB){
                    case 0:
                        exitA = 2;
                        roomYlocation++;
                        break;
                    case 1:
                        exitA = 3;
                        roomXlocation++;
                        break;
                    case 2:
                        exitA = 0;
                        roomYlocation--;
                        break;
                    case 3:
                        exitA = 1;
                        roomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using exitB");
                        return null;
                }
                //if exitC is being used then its exit space should be saved and the split saved x and y locations be adjusted 
                if(exitC != -1){
                    switch (exitC){
                    case 0:
                        splitSavedExit = 2;
                        splitSavedroomYlocation++;
                        break;
                    case 1:
                        splitSavedExit = 3;
                        splitSavedroomXlocation++;
                        break;
                    case 2:
                        splitSavedExit = 0;
                        splitSavedroomYlocation--;
                        break;
                    case 3:
                        splitSavedExit = 1;
                        splitSavedroomXlocation--;
                        break;
                    default:
                        Debug.LogWarning("Something went wrong at the switch using exitC");
                        return null;
                    }
                    exitC = -1;
                }
            }
        }
        return levelBlueprint;
    }



    //selects an exit for the room
    int SelectExit(bool leftTunnel, int iRoomXlocation, int iRoomYlocation, int iExitA){

        int currentIndex;
        int initiallySelectedExitIndex = -1;
        int[] possibleExits;

        if(leftTunnel){
            switch (iExitA){
            case 0:

                possibleExits = new int[]{2, 3};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 1:

                possibleExits = new int[]{0, 2, 3};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 2:

                possibleExits = new int[]{0, 3};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 3:

                possibleExits = new int[]{0, 2};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            default:
                possibleExits = new int[]{};
                Debug.LogWarning("Something went wrong selecting a left tunnel exit");
                Debug.Break();
                break;
            }
        }else{
            switch (iExitA){
            case 0:

                possibleExits = new int[]{1, 2};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 1:

                possibleExits = new int[]{0, 2};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 2:

                possibleExits = new int[]{0, 1};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
                break;
            case 3:

                possibleExits = new int[]{0, 1, 2};
                initiallySelectedExitIndex = UnityEngine.Random.Range(0, possibleExits.Length);
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
                        if(levelBlueprint[iRoomXlocation, iRoomYlocation + 1].type == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 1:
                        if(levelBlueprint[iRoomXlocation + 1, iRoomYlocation].type == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 2:
                        if(levelBlueprint[iRoomXlocation, iRoomYlocation - 1].type == 0){
                            return possibleExits[currentIndex];
                        }
                        break;
                    case 3:
                        if(levelBlueprint[iRoomXlocation - 1, iRoomYlocation].type == 0){
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

    

    //selects an additional exit for split rooms, inputs the tunnels general direction
    int SelectExit(bool leftTunnel, int iRoomXlocation, int iRoomYlocation, int iExitA, int iExitB){
        
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
            if((iExitA == 0 && iExitB == 1) || (iExitA == 1 && iExitB == 0)){
                possibleExits = new int[]{2, 3};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 0 && iExitB == 2) || (iExitA == 2 && iExitB == 0)){
                possibleExits = new int[]{3};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 0 && iExitB == 3) || (iExitA == 3 && iExitB == 0)){
                possibleExits = new int[]{2};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 1 && iExitB == 2) || (iExitA == 2 && iExitB == 1)){
                possibleExits = new int[]{0, 3};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 1 && iExitB == 3) || (iExitA == 3 && iExitB == 1)){
                possibleExits = new int[]{0, 2};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 2 && iExitB == 3) || (iExitA == 3 && iExitB == 2)){
                possibleExits = new int[]{0};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }
        }else{
            if((iExitA == 0 && iExitB == 1) || (iExitA == 1 && iExitB == 0)){
                possibleExits = new int[]{2};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 0 && iExitB == 2) || (iExitA == 2 && iExitB == 0)){
                possibleExits = new int[]{1};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 0 && iExitB == 3) || (iExitA == 3 && iExitB == 0)){
                possibleExits = new int[]{1, 2};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 1 && iExitB == 2) || (iExitA == 2 && iExitB == 1)){
                possibleExits = new int[]{0};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 1 && iExitB == 3) || (iExitA == 3 && iExitB == 1)){
                possibleExits = new int[]{0, 2};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }else if((iExitA == 2 && iExitB == 3) || (iExitA == 3 && iExitB == 2)){
                possibleExits = new int[]{0, 1};
                newExit = possibleExits[UnityEngine.Random.Range(0, possibleExits.Length)];

            }
        }

        //check if the space the selected exit faces is empty to place a new room
        //loop through possible exits if it is not available.
        initiallySelectedExit = newExit;

        do{
            
            while(newExit == iExitA || newExit == iExitB){
                newExit++;
                if(newExit >= 4){
                    newExit = 0;
                }   
            }
            //now check if the space the exit faces is empty so there is space for a new room
            //if there is space then return the new exit value.
            switch (newExit){
                case 0:
                    if(levelBlueprint[iRoomXlocation, iRoomYlocation + 1].type == 0){
                        return newExit;
                    }
                    break;
                case 1:
                    if(levelBlueprint[iRoomXlocation + 1, iRoomYlocation].type == 0){
                        return newExit;
                    }
                    break;
                case 2:
                    if(levelBlueprint[iRoomXlocation, iRoomYlocation - 1].type == 0){
                        return newExit;
                    }
                    break;
                case 3:
                    if(levelBlueprint[iRoomXlocation - 1, iRoomYlocation].type == 0){
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

    //return the new room based on exit directions
    BPRoom NewBPRoom(int iType, int iExitA, int iExitB){

        if(debugMode){Debug.Log("Saving a room to the BP with exitA: " + iExitA + " and exitB: " + iExitB);}

        if((iExitA == 0 && iExitB == 1) || (iExitA == 1 && iExitB == 0)){
            return new BPRoom(iType, 1, 1, 0, 0);

        }else if((iExitA == 0 && iExitB == 2) || (iExitA == 2 && iExitB == 0)){
            return new BPRoom(iType, 1, 0, 1, 0);

        }else if((iExitA == 0 && iExitB == 3) || (iExitA == 3 && iExitB == 0)){
            return new BPRoom(iType, 1, 0, 0, 1);

        }else if((iExitA == 1 && iExitB == 2) || (iExitA == 2 && iExitB == 1)){
            return new BPRoom(iType, 0, 1, 1, 0);

        }else if((iExitA == 1 && iExitB == 3) || (iExitA == 3 && iExitB == 1)){
            return new BPRoom(iType, 0, 1, 0, 1);

        }else{
            //return the only other type of room, exitA = 2, exitB = 3
            return new BPRoom(iType, 0, 0, 1, 1);
        }
    }



    //return the new room based on exit directions
    BPRoom NewBPRoom(int iType, int iExitA, int iExitB, int iExitC){
        if(iExitA != 0 && iExitB != 0 && iExitC != 0){
            return new BPRoom(iType, 0, 1, 1, 1);

        }else if(iExitA != 1 && iExitB != 1 && iExitC != 1){
            return new BPRoom(iType, 1, 0, 1, 1);

        }else if(iExitA != 2 && iExitB != 2 && iExitC != 2){
            return new BPRoom(iType, 1, 1, 0, 1);

        }else{
            return new BPRoom(iType, 1, 1, 1, 0);

        }
    }



    //creates a visual bp for debuging purposes
    void DebugBlueprint(int type, int x, int y){

        Debug.Log("Placing a room of type: " + type + " at X: " + x + " by Y: " + y);

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
}