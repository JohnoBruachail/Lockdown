using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReference
{
    public int x;
    public int y;
    public int type;
    public int entrance;
    public int[] exits = new int[4];//the arrays index is its direction, 0 = up, 1 = right, 2 = down, 3 = left, its value is the exit type, 0 = none, 1 = left, 2 = right.
    public int[] exitReferences = new int[2];//The two references to each exit in the room.
    public GameObject room;
    public RoomReference parent;
    public List<RoomReference> children = new List<RoomReference>();
    public bool isRoomPowered = false;
    public List<Building> destroyedBuildings = new List<Building>();

    //int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit
    public RoomReference(){

    }
    
    public RoomReference(int x, int y, int type, int[] exits, int entrance, int roomsFirstExit, int roomsSecondExit, RoomReference parent){
        this.x                  = x;
        this.y                  = y;
        this.type               = type;
        this.exits[0]           = exits[0];
        this.exits[1]           = exits[1];
        this.exits[2]           = exits[2];
        this.exits[3]           = exits[3];
        this.entrance           = entrance;
        this.exitReferences[0]  = roomsFirstExit;
        this.exitReferences[1]  = roomsSecondExit;
        this.parent             = parent;
    }

    void repairBuildings(){
        foreach(Building building in destroyedBuildings){
            building.active = true;
        }
    }
}
