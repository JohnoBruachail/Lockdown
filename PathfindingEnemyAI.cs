using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathfindingEnemyAI : MonoBehaviour
{
    public Transform target; //The next target the AI will head for, this needs to be selected based on room, a few potential idea's
    /* 
        The breach itself could coordinate the targets, it can take its spawn room, access the rooms buildings, 
        
            //I'll need to make each room keep a list of buildings
                but the order has to be from closest to farthest??

                    ok, what if, I target the core, if the enemy sign circle collides with any other potential targets along the way it will attack them???

    */
    public float speed = 200f; //speed at which the AI moves

    public float nextWaypointDistance = 3f; //the distance from the current waypoint before it moves onto the next waypoint 

    Path path; //The current path the AI is following (Possibly have this calculated from breach and passed onto AI? Save several repeat calculations?)
    int currentWaypoint = 0; // The current waypoint along the path the AI is currently targeting
    bool reachedEndOfPath; //Bool thats true when we've reached the end of the current path

    Seeker seeker; //reference to seeker script
    Rigidbody2D rb; //reference to rigidbody2D

    // Start is called before the first frame update
    void Start(){
        //get the relevent components on this object.
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //the seeker is responsible for creating paths, I may need to attach this to the breech and pass the created path to each of the flying enemys created.
                        //this enemy position, the targets position, call function when path is done
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update(){
        
        if (path == null){
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }

    }

    void OnPathComplete(Path p){

        //if we have no errors generating the path the set path to created path and start at first waypoint.
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }

    }

}
